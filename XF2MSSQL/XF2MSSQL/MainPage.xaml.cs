using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Honeywell.AIDC.CrossPlatform;
using System.Threading;
using Xamarin.Essentials;

namespace XF2MSSQL
{
    public partial class MainPage : ContentPage
    {
        private const string DEFAULT_READER_KEY = "default";
        private Dictionary<string, BarcodeReader> mBarcodeReaders;
        private bool mContinuousScan = false, mOpenReader = true;
        private BarcodeReader mSelectedReader = null;
        private SynchronizationContext mUIContext = SynchronizationContext.Current;
        private int mTotalContinuousScanCount = 0;
        private bool mSoftContinuousScanStarted = false;
        private bool mSoftOneShotScanStarted = false;
        private string deviceModel = null;

        public string SQLreturn { get; set; }
        public int BusinessEntityID { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BusinessEntityID = 1;

            mBarcodeReaders = new Dictionary<string, BarcodeReader>();
            deviceModel = DeviceInfo.Model;


        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SQLreturn = "try SQL query";
            myLabel.Text = SQLreturn;

            string sqlQuery = string.Format("SELECT FirstName FROM person.person WHERE BusinessEntityID = {0}", BusinessEntityID.ToString());

            MySQL mySql = new MySQL();
            myLabel.Text = mySql.GetValue(sqlQuery);

            BusinessEntityID++;
        }

        async void openPersonList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonList());
        }

        async void openProducts(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductList());
        }

        private void OpenReaderSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            mOpenReader = e.Value;
            if (mOpenReader)
            {
                OpenBarcodeReader();
            }
            else
            {
                CloseBarcodeReader();
            }
        }

        private string GetSelectedReaderName()
        {
            int selIndex = mReaderPicker.SelectedIndex;
            if (selIndex >= 0)
            {
                return mReaderPicker.Items[selIndex];
            }
            else
            {
                return null;
            }
        }

        public BarcodeReader GetBarcodeReader(string readerName)
        {
            BarcodeReader reader = null;

            if (readerName == DEFAULT_READER_KEY)
            { // This name was added to the Open Reader picker list if the
              // query for connected barcode readers failed. It is not a
              // valid reader name. Set the readerName to null to default
              // to internal scanner.
                readerName = null;
            }

            if (null == readerName)
            {
                if (mBarcodeReaders.ContainsKey(DEFAULT_READER_KEY))
                {
                    reader = mBarcodeReaders[DEFAULT_READER_KEY];
                }
            }
            else
            {
                if (mBarcodeReaders.ContainsKey(readerName))
                {
                    reader = mBarcodeReaders[readerName];
                }
            }

            if (null == reader)
            {
                // Create a new instance of BarcodeReader object.
                reader = new BarcodeReader(readerName);

                // Add an event handler to receive barcode data.
                // Even though we may have multiple reader sessions, we only
                // have one event handler. In this app, no matter which reader
                // the data come frome it will update the same UI controls.
                reader.BarcodeDataReady += MBarcodeReader_BarcodeDataReady;

                // Add the BarcodeReader object to mBarcodeReaders collection.
                if (null == readerName)
                {
                    mBarcodeReaders.Add(DEFAULT_READER_KEY, reader);
                }
                else
                {
                    mBarcodeReaders.Add(readerName, reader);
                }
            }

            return reader;
        }

        private async void MBarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            // Update the barcode information in the UI thread.
            mUIContext.Post(_ => {
                UpdateBarcodeInfo(e.Data, e.SymbologyName, e.TimeStamp);
            }
                , null);

            if (mContinuousScan)
            {
                mTotalContinuousScanCount++;

                // Measure and display the performance.
                mUIContext.Post(_ => {
                    myLabel.Text = mTotalContinuousScanCount.ToString();
                }
                , null);
            } //endif (mContinuousScan)
            else if (mSoftOneShotScanStarted)
            {
                // Turn off the software trigger.
                await mSelectedReader.SoftwareTriggerAsync(false);
                mSoftOneShotScanStarted = false;
            }
        }
        private void UpdateBarcodeInfo(string data, string symbology, DateTime timestamp)
        {
            mScanDataEditor.Text = data;
            mSymbologyLabel.Text = "Symbology: " + symbology;
            mTimestampLabel.Text = "Timestamp: " + timestamp.ToString();
        }

        public async void CloseBarcodeReader()
        {
            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                if (mSoftOneShotScanStarted || mSoftContinuousScanStarted)
                {
                    // Turn off the software trigger.
                    await mSelectedReader.SoftwareTriggerAsync(false);
                    mSoftOneShotScanStarted = false;
                    mSoftContinuousScanStarted = false;
                }

                BarcodeReader.Result result = await mSelectedReader.CloseAsync();
                if (result.Code == BarcodeReader.Result.Codes.SUCCESS)
                {
                    //mScanButton.IsEnabled = false;
                    // Allow user to select another reader.
                    mReaderPicker.IsEnabled = true;

                    // Disable the Enable Scanning switch.
                    mEnableScanningLabel.IsEnabled = false;
                    mEnableScanningSwitch.IsEnabled = false;
                    // Turn off the Enable Scanning switch.
                    mEnableScanningSwitch.IsToggled = false;

                    // Turn off the Continuous switch.
                    //mContinuousSwitch.IsToggled = false;
                    // Disable the Continuous switch
                    //mContinuousLabel.IsEnabled = false;
                    //mContinuousSwitch.IsEnabled = false;
                }
                else
                {
                    await DisplayAlert("Error", "CloseAsync failed, Code:" + result.Code +
                        " Message:" + result.Message, "OK");
                }
            }
        }

        public async void OpenBarcodeReader()
        {
            if (mOpenReader) // Open Reader switch is in the On state.
            {
                mSelectedReader = GetBarcodeReader(GetSelectedReaderName());
                if (!mSelectedReader.IsReaderOpened)
                {
                    BarcodeReader.Result result = await mSelectedReader.OpenAsync();

                    if (result.Code == BarcodeReader.Result.Codes.SUCCESS ||
                        result.Code == BarcodeReader.Result.Codes.READER_ALREADY_OPENED)
                    {
                        SetScannerAndSymbologySettings();

                        // Prevent user from selecting another reader.
                        mReaderPicker.IsEnabled = false;

                        // Turn on the Enable Scanning switch.
                        mEnableScanningSwitch.IsToggled = true;
                        // Enable the Enable Scanning switch.
                        mEnableScanningLabel.IsEnabled = true;
                        mEnableScanningSwitch.IsEnabled = true;

                        // Clear barcode data information
                        mScanDataEditor.Text = "";
                        mSymbologyLabel.Text = "";
                        mTimestampLabel.Text = "";
                    }
                    else
                    {
                        await DisplayAlert("Error", "OpenAsync failed, Code:" + result.Code +
                            " Message:" + result.Message, "OK");
                    }
                }
            } //endif (mOpenReader)
        }

        private async void SetScannerAndSymbologySettings()
        {
            try
            {
                if (mSelectedReader.IsReaderOpened)
                {
                    Dictionary<string, object> settings = new Dictionary<string, object>()
                    {
                        {mSelectedReader.SettingKeys.TriggerScanMode, mSelectedReader.SettingValues.TriggerScanMode_OneShot },
                        {mSelectedReader.SettingKeys.Code128Enabled, true },
                        {mSelectedReader.SettingKeys.Code39Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8Enabled, true },
                        {mSelectedReader.SettingKeys.Ean8CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Ean13Enabled, true },
                        {mSelectedReader.SettingKeys.Ean13CheckDigitTransmitEnabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25Enabled, true },
                        {mSelectedReader.SettingKeys.Interleaved25MaximumLength, 100 },
                        {mSelectedReader.SettingKeys.Postal2DMode, mSelectedReader.SettingValues.Postal2DMode_Usps }
                    };

                    BarcodeReader.Result result = await mSelectedReader.SetAsync(settings);
                    if (result.Code != BarcodeReader.Result.Codes.SUCCESS)
                    {
                        await DisplayAlert("Error", "Symbology settings failed, Code:" + result.Code +
                                            " Message:" + result.Message, "OK");
                    }
                }
            }
            catch (Exception exp)
            {
                await DisplayAlert("Error", "Symbology settings failed. Message: " + exp.Message, "OK");
            }
        }

        private async void EnableScanningSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!deviceModel.Contains("VM1A"))
            {
                //mScanButton.IsEnabled = e.Value;
                //mContinuousLabel.IsEnabled = e.Value;
                //mContinuousSwitch.IsEnabled = e.Value;
            }

            if (mSelectedReader != null && mSelectedReader.IsReaderOpened)
            {
                BarcodeReader.Result result = await mSelectedReader.EnableAsync(e.Value); // Enables or disables barcode reader
                if (result.Code != BarcodeReader.Result.Codes.SUCCESS)
                {
                    await DisplayAlert("Error", "EnableAsync failed, Code:" + result.Code +
                                        " Message:" + result.Message, "OK");
                }
            }
        }

    }
}
