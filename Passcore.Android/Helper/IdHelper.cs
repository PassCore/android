using Android.App;
using Android.Util;

using System;


namespace Passcore.Android.Helper
{
    class IdHelper
    {
        public static (string, Exception) GetUID()
        {
            try
            {
                var context = Application.Context;
                return (global::Android.Provider.Settings.Secure.GetString(
                    context.ContentResolver,
                    global::Android.Provider.Settings.Secure.AndroidId), null);
            }
            catch (Exception ex)
            {
                Log.Error("Passcore/IdHelper", $"Fail to Get device UID!\n{ex.ToString()}");
                return ("", ex);
            }
        }
    }
}