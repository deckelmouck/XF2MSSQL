using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XF2MSSQL
{
    public partial class App : Application
    {
        private App mBarcodeReaderApp;
        private MainPage mMainPage;

        public App()
        {
            InitializeComponent();

            mMainPage = new MainPage();

            MainPage = new NavigationPage(mMainPage);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mMainPage.OpenBarcodeReader();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            mMainPage.CloseBarcodeReader();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mMainPage.OpenBarcodeReader();
        }

    }
}
