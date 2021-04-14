using System.IO;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Passcore.Android.Helper;

namespace Passcore.Android.Views
{
    [Activity(Label = "@string/settings", Theme = "@style/AppTheme", ParentActivity = typeof(MainActivity))]
    public class SettingsActivity : AppCompatActivity
    {
        Switch SwtBlockScreenshot,
            SwtSaveEnhance,
            SwtSavePassword,
            SwtSaveMasterKey,
            SwtSavePasswordLength,
            SwtSaveIsCharRequired;

        Button BtnResetAll;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // TODO: Store it
            SwtBlockScreenshot = FindViewById<Switch>(Resource.Id.SwtBlockScreenshot);
            SwtSaveEnhance = FindViewById<Switch>(Resource.Id.SwtSaveEnhance);
            SwtSavePassword = FindViewById<Switch>(Resource.Id.SwtSavePassword);
            SwtSaveMasterKey = FindViewById<Switch>(Resource.Id.SwtSaveMasterKey);
            SwtSavePasswordLength = FindViewById<Switch>(Resource.Id.SwtSavePasswordLength);
            SwtSaveIsCharRequired = FindViewById<Switch>(Resource.Id.SwtSaveIsCharRequired);
            BtnResetAll = FindViewById<Button>(Resource.Id.BtnResetAll);

            SwtSaveEnhance.Checked = Shared.Config.IsStoreEnhance;
            SwtSavePassword.Checked = Shared.Config.IsStorePassword;
            SwtSaveMasterKey.Checked = Shared.Config.IsStoreMasterKey;
            SwtSavePasswordLength.Checked = Shared.Config.IsStorePasswordLength;
            SwtSaveIsCharRequired.Checked = Shared.Config.IsStoreCharRequired;

            SwtBlockScreenshot.Checked = Shared.MainActivity != null ? Shared.MainActivity.IsSecure : false;

            SwtBlockScreenshot.CheckedChange += SwtBlockScreenshot_CheckedChange;
            SwtSaveMasterKey.CheckedChange += SwtSaveMasterKey_CheckedChange;
            SwtSaveEnhance.CheckedChange += SwtSaveEnhance_CheckedChange;
            SwtSavePassword.CheckedChange += SwtSavePassword_CheckedChange;
            SwtSavePasswordLength.CheckedChange += SwtSavePasswordLength_CheckedChange;
            SwtSaveIsCharRequired.CheckedChange += SwtSaveIsCharRequired_CheckedChange;
            BtnResetAll.Click += BtnResetAll_Click;
        }

        private void BtnResetAll_Click(object sender, System.EventArgs e)
        {
            var a = new global::Android.App.AlertDialog.Builder(this).Create();
            a.SetTitle(Resources.GetString(Resource.String.caution));
            a.SetMessage(Resources.GetString(Resource.String.reset_all_warn));
            a.SetButton(Resources.GetString(Resource.String.ok), (s, d) =>
            {
                File.Delete(Path.Combine(IOHelper.GetDataPath(), "config.pc"));
                System.Environment.Exit(0);
            });
            a.SetButton2(Resources.GetString(Resource.String.cancel), (s, d) => { });
            a.Show();
        }

        private void SwtSaveIsCharRequired_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Shared.Config.IsStoreCharRequired = SwtSaveIsCharRequired.Checked;
        }

        private void SwtSavePasswordLength_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Shared.Config.IsStorePasswordLength = SwtSavePasswordLength.Checked;
        }

        private void SwtSavePassword_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Shared.Config.IsStorePassword = SwtSavePassword.Checked;
        }

        private void SwtSaveEnhance_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Shared.Config.IsStoreEnhance = SwtSaveEnhance.Checked;
        }

        private void SwtSaveMasterKey_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Shared.Config.IsStoreMasterKey = SwtSaveMasterKey.Checked;
        }

        private void SwtBlockScreenshot_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ActivityHelper.SetSecureFlag(this, SwtBlockScreenshot.Checked);
            if (Shared.MainActivity != null)
                Shared.MainActivity.IsSecure = SwtBlockScreenshot.Checked;
        }
    }
}