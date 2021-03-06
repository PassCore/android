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
    class IdHelper
    {
        public (string, Exception) GetUID()
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
                return ("", null);
            }
        }
    }
}