using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passcore.Android
{
    class ConfigHelper
    {
        public static string BuildConfigString(string masterKey, string passwd, string enhance)
        {
            return $"MasterKey={masterKey}\n" +
                $"Password={passwd}\n" +
                $"Enhance={enhance}";
        }

        public static string GetConfigString()
            => IOHelper.ReadAllText("config.pc");

        public static (string, string, string) ParseConfigString(string config)
        {
            if (string.IsNullOrWhiteSpace(config))
                return ("", "", "");
            var c = config.Split('\n');
            var mk = "";
            var passwd = "";
            var enhance = "";
            foreach (var s in c)
            {
                var i = s.IndexOf('=');
                if (i < 0)
                    continue;
                var key = s.Substring(0, i).Trim();
                var value = i == s.Length - 1 ? "" : s.Substring(i + 1).Trim();
                switch (key)
                {
                    case "MasterKey":
                        mk = value;
                        break;
                    case "Password":
                        passwd = value;
                        break;
                    case "Enhance":
                        enhance = value;
                        break;
                }
            }
            return (mk, passwd, enhance);
        }

        public static void SaveConfig(string path, string mk, string passwd, string enhance)
        {
            IOHelper.WriteAllText(path, BuildConfigString(mk, passwd, enhance));
        }
    }
}