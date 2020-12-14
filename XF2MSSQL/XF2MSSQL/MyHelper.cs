using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XF2MSSQL
{
    public class MyHelper
    {
        protected string GetSetting(string setting)
        {
            // Get the assembly this code is executing in
            var assembly = Assembly.GetExecutingAssembly();

            // Look up the resource names and find the one that ends with settings.json
            // Your resource names will generally be prefixed with the assembly's default namespace
            // so you can short circuit this with the known full name if you wish
            var resName = assembly.GetManifestResourceNames()
                ?.FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));

            // Load the resource file
            var file = assembly.GetManifestResourceStream(resName);

            // Stream reader to read the whole file
            var sr = new StreamReader(file);

            // Read the json from the file
            var json = sr.ReadToEnd();

            // Parse out the JSON
            var j = JObject.Parse(json);

            return j.Value<string>(setting);
        }
    }
}
