using System;
using System.Reflection;
using System.Security;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

using Xamarin.Essentials;

namespace Passcore.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText EdtMasterKey,
                 EdtPassword,
                 EdtEnhanceField;

        Button BtnClear,
                 BtnRandom,
                 BtnGenerate;

        CheckBox CkbIsCharRequired,
                 ChkIsWeakPasswdAllowed;

        SeekBar SkbLength;

        TextView TxvPasswdLength,
            TxvVersion;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            BtnClear = FindViewById<Button>(Resource.Id.BtnClean);
            BtnGenerate = FindViewById<Button>(Resource.Id.BtnGenerate);
            BtnRandom = FindViewById<Button>(Resource.Id.BtnRandom);

            EdtMasterKey = FindViewById<EditText>(Resource.Id.EdtMasterKey);
            EdtPassword = FindViewById<EditText>(Resource.Id.EdtPassword);
            EdtEnhanceField = FindViewById<EditText>(Resource.Id.EdtEnhanceField);

            SkbLength = FindViewById<SeekBar>(Resource.Id.SeekBar);

            TxvPasswdLength = FindViewById<TextView>(Resource.Id.TxvPasswdLength);

            CkbIsCharRequired = FindViewById<CheckBox>(Resource.Id.CkbIsCharRequired);
            ChkIsWeakPasswdAllowed = FindViewById<CheckBox>(Resource.Id.ChkIsWeakPasswdAllowed);

            BtnClear.Click += BtnClear_Click;
            BtnGenerate.Click += BtnGenerate_Click;

            TxvVersion = FindViewById<TextView>(Resource.Id.TxvVersion);

            TxvVersion.Text = $"{ProjectInfo.AppName}({ProjectInfo.AppVersion})\n" +
                $"{Copyright}";

            SkbLength.ProgressChanged += SkbLength_ProgressChanged;

            TxvVersion = FindViewById<TextView>(Resource.Id.TxvVersion);

        }

        private void SkbLength_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
            => TxvPasswdLength.Text = $"Password Length: {SkbLength.Progress}";

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            // TODO: FIX
            var mode = GetDict(GenerateMode.WithChar);

            if (string.IsNullOrWhiteSpace(EdtMasterKey.Text) || string.IsNullOrWhiteSpace(EdtPassword.Text))
            {
                // Invalid
            }

            string pw = Passcore.GeneratePassword(mode,
                SkbLength.Progress,
                EdtMasterKey.Text,
                EdtPassword.Text,
                EdtEnhanceField.Text
            );
            ShowPassword(pw);
        }

        private void ShowPassword(string pswd)
        {
            var a = new global::Android.App.AlertDialog.Builder(this).Create();
            a.SetTitle(Resources.GetString(Resource.String.your_passwd));
            a.SetMessage(pswd);
            a.SetButton(Resources.GetString(Resource.String.ok), (s, a) => { });
            a.SetButton2(Resources.GetString(Resource.String.copy_to_clipboard), async (s, a) =>
            {
                await Clipboard.SetTextAsync(pswd);
            });
            a.Show();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            EdtMasterKey.Text = EdtPassword.Text = EdtEnhanceField.Text = default;
        }

        enum GenerateMode
        {
            WithChar,
            NoChar,
            NumberOnly,
            SymbolOnly,
            LatinOnly
        }

        private (char[] dict, int occur)[] GetDict(GenerateMode gm)
            => gm switch
            {
                GenerateMode.WithChar => new (char[] dict, int occur)[]
                {
                    (Tables.LatinLower, 0),
                    (Tables.LatinUpper, 0),
                    (Tables.Number, 0),
                    (Tables.Symbol, 0)
                },
                GenerateMode.NoChar => new (char[] dict, int occur)[]
                {
                    (Tables.LatinLower, 0),
                    (Tables.LatinUpper, 0),
                    (Tables.Number, 0)
                },
                GenerateMode.NumberOnly => new (char[] dict, int occur)[]
                {
                    (Tables.Number, 0)
                },
                GenerateMode.SymbolOnly => new (char[] dict, int occur)[]
                {
                    (Tables.Symbol, 0)
                },
                GenerateMode.LatinOnly => new (char[] dict, int occur)[]
                {
                    (Tables.LatinLower, 0),
                    (Tables.LatinUpper, 0)
                },
                _ => throw new NotImplementedException(),
            };

        public string Copyright
        {
            get => Resources.GetString(Resource.String.copyright);
        }

    }
}
