﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

using System;
using Passcore.Android.Helper;

namespace Passcore.Android.Views
{
    [Activity(Label = "@string/settings", Theme = "@style/AppTheme", ParentActivity = typeof(MainActivity))]
    public class SettingsActivity : AppCompatActivity
    {
        Button BtnApply;
        Switch SwtBlockScreenshot;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_settings);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            BtnApply = FindViewById<Button>(Resource.Id.BtnApply);
            SwtBlockScreenshot = FindViewById<Switch>(Resource.Id.SwtBlockScreenshot);

            SwtBlockScreenshot.Checked = Shared.MainActivity != null ?
                Shared.MainActivity.IsSecure :
                false;

            SwtBlockScreenshot.CheckedChange += SwtBlockScreenshot_CheckedChange;
            BtnApply.Click += BtnApply_Click;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (Shared.MainActivity != null)
                Shared.MainActivity.IsSecure = SwtBlockScreenshot.Checked;
        }

        private void SwtBlockScreenshot_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
            => ActivityHelper.SetSecureFlag(this, SwtBlockScreenshot.Checked);
    }
}