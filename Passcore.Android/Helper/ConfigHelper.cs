using Android.Util;

using Newtonsoft.Json;

using Passcore.Android.Views;

using System;

namespace Passcore.Android.Helper
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

        public static string BuildConfigString(Models.Config c)
        {
            return JsonConvert.SerializeObject(c);
        }

        public static string BuildConfigString()
        {
            return BuildConfigString(Shared.Config);
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

        public static void ParseConfigString(string config)
        {
            if (string.IsNullOrWhiteSpace(config))
            {
                Shared.Config.ValueChanged += () =>
                {
                    SaveConfig("config.pc");
                };
            }
            try
            {
                Shared.Config = JsonConvert.DeserializeObject<Models.Config>(config);
            }
            catch (Exception ex)
            {
                Log.Info("Passcore/Config", $"Failed to ParseConfigString()\n{ex.ToString()}");
            }
            finally
            {
                Shared.Config.ValueChanged += () =>
                {
                    SaveConfig("config.pc");
                };
            }
        }

        public static void SaveConfig(string path)
        {
            var c = Shared.Config;
            c.Clean();
            IOHelper.WriteAllText(path, AesHelper.Encrypt(GetDeviceKey(), BuildConfigString(c)));
        }
    }
}