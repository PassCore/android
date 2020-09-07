using System;
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
        EditText edtMasterKey,
                 edtPassword,
                 edtEnhanceField;

        Button   btnClear,
                 btnRandom,
                 btnGenerate;

        CheckBox ckbIsCharRequired,
                 ckbIsShortLengthRequired;

        SeekBar  skbLength;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btnClear = FindViewById<Button>(Resource.Id.Clean);
            btnGenerate = FindViewById<Button>(Resource.Id.Generate);
            btnRandom = FindViewById<Button>(Resource.Id.Random);

            edtMasterKey = FindViewById<EditText>(Resource.Id.MasterKey);
            edtPassword = FindViewById<EditText>(Resource.Id.Password);
            edtEnhanceField = FindViewById<EditText>(Resource.Id.EnhanceField);

            skbLength = FindViewById<SeekBar>(Resource.Id.SeekBar);

            btnClear.Click += btnClear_Click;
            btnGenerate.Click += btnGenerate_Click;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // TODO: FIX
            var mode = GetDict(GenerateMode.WithChar);

            if (string.IsNullOrWhiteSpace(edtMasterKey.Text) || string.IsNullOrWhiteSpace(edtPassword.Text))
            {
                // Invalid
            }

            string pw = Passcore.GeneratePassword(mode,
                skbLength.Progress,
                edtMasterKey.Text,
                edtPassword.Text,
                edtEnhanceField.Text
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            edtMasterKey.Text = edtPassword.Text = edtEnhanceField.Text = default;
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

    }
}
