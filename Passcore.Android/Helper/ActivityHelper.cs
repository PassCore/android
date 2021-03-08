using Android.App;
using Android.Views;

namespace Passcore.Android.Helper
{
    public class ActivityHelper
    {
        public static void SetSecureFlag(Activity activity, bool isSecure)
        {
            if (isSecure)
            {
                activity.Window.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);
            }
            else
            {
                activity.Window.ClearFlags(WindowManagerFlags.Secure);
            }
        }
    }
}