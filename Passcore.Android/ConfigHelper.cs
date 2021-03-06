using Android.Util;

using System;

namespace Passcore.Android
{
    class ConfigHelper
    {
        private static string GetDeviceKey()
        {
            var m = IdHelper.GetUID();
            if (m.Item2 != null)
                Log.Error("Passcore/DeviceKey", $"Failed to get device UID.\n{m.Item2.ToString()}");
            return IdHelper.GetUID().Item1;
        }


        public static string BuildConfigString(string masterKey, string passwd, string enhance)
        {
            return $"MasterKey={masterKey}\n" +
                $"Password={passwd}\n" +
                $"Enhance={enhance}";
        }

        public static string GetConfigString()
        {
            var c = IOHelper.ReadAllText("config.pc");
            if (string.IsNullOrWhiteSpace(c))
                return "";
            try
            {
                return AesHelper.Decrypt(GetDeviceKey(), c);
            }
            catch (Exception ex)
            {
                Log.Info("Passcore/DeviceKey", $"Failed to GetConfigString()\n{ex.ToString()}");
                return "";
            }
        }

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
                var value = i == s.Length - 1 ? "" : s[(i + 1)..].Trim();
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
            IOHelper.WriteAllText(path, AesHelper.Encrypt(GetDeviceKey(), BuildConfigString(mk, passwd, enhance)));
        }
    }
}