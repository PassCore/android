using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

using Xamarin.Essentials;
using Passcore.Android.Helper;

namespace Passcore.Android.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText EdtMasterKey,
                 EdtPassword,
                 EdtEnhanceField;

        Button BtnClear,
                 BtnRandom,
                 BtnGenerate,
                 BtnSave,
                 BtnSettings;

        CheckBox CkbIsCharRequired,
                 ChkIsWeakPasswd;

        SeekBar SkbLength;

        TextView TxvPasswdLength,
            TxvVersion;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Shared.MainActivity = this;

            ConfigHelper.ParseConfigString(ConfigHelper.GetConfigString());

            #region Components Define
            BtnClear = FindViewById<Button>(Resource.Id.BtnClean);
            BtnGenerate = FindViewById<Button>(Resource.Id.BtnGenerate);
            BtnRandom = FindViewById<Button>(Resource.Id.BtnRandom);
            BtnSave = FindViewById<Button>(Resource.Id.BtnSave);
            BtnSettings = FindViewById<Button>(Resource.Id.BtnSettings);

            EdtMasterKey = FindViewById<EditText>(Resource.Id.EdtMasterKey);
            EdtPassword = FindViewById<EditText>(Resource.Id.EdtPassword);
            EdtEnhanceField = FindViewById<EditText>(Resource.Id.EdtEnhanceField);

            SkbLength = FindViewById<SeekBar>(Resource.Id.SeekBar);

            TxvPasswdLength = FindViewById<TextView>(Resource.Id.TxvPasswdLength);

            CkbIsCharRequired = FindViewById<CheckBox>(Resource.Id.CkbIsCharRequired);
            ChkIsWeakPasswd = FindViewById<CheckBox>(Resource.Id.ChkIsWeakPasswd);

            TxvVersion = FindViewById<TextView>(Resource.Id.TxvVersion);
            #endregion

            #region initialise UI value 
            SetSeekBar();

            TxvVersion.Text = $"{ProjectInfo.AppName}({ProjectInfo.AppVersion})\n" +
                $"{Copyright}";

            FlashTxvPasswdLength();

            EdtMasterKey.Text = Shared.Config.MasterKey;

            (EdtMasterKey.Text, EdtPassword.Text, EdtEnhanceField.Text) =
                (Shared.Config.MasterKey, Shared.Config.Password, Shared.Config.Enhance);
            #endregion

            #region Evt
            SkbLength.ProgressChanged += SkbLength_ProgressChanged;

            ChkIsWeakPasswd.CheckedChange += ChkIsWeakPasswd_CheckedChange;

            BtnClear.Click += BtnClear_Click;
            BtnGenerate.Click += BtnGenerate_Click;
            BtnRandom.Click += BtnRandom_Click;
            BtnSave.Click += BtnSave_Click;
            BtnSettings.Click += BtnSettings_Click;
            EdtEnhanceField.AfterTextChanged += EdtEnhanceField_AfterTextChanged;
            EdtMasterKey.AfterTextChanged += EdtMasterKey_AfterTextChanged;
            EdtPassword.AfterTextChanged += EdtPassword_AfterTextChanged;
            #endregion
        }

        private void EdtPassword_AfterTextChanged(object sender, global::Android.Text.AfterTextChangedEventArgs e)
        {
            Shared.Config.MasterKey = EdtPassword.Text;
        }

        private void EdtMasterKey_AfterTextChanged(object sender, global::Android.Text.AfterTextChangedEventArgs e)
        {
            Shared.Config.MasterKey = EdtMasterKey.Text;
        }

        private void EdtEnhanceField_AfterTextChanged(object sender, global::Android.Text.AfterTextChangedEventArgs e)
        {
            Shared.Config.Enhance = EdtEnhanceField.Text;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SettingsActivity));
            StartActivity(intent);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ConfigHelper.SaveConfig("config.pc");
        }

        private void ChkIsWeakPasswd_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            SetSeekBar();
        }

        private void SetSeekBar()
        {
            SkbLength.Max = PasswordLengthHelper.GetMax(ChkIsWeakPasswd.Checked);
        }

        private GenerateMode GetGenerateMode()
            => CkbIsCharRequired.Checked switch
            {
                true => GenerateMode.WithChar,
                _ => GenerateMode.NoChar,
            };

        private void BtnRandom_Click(object sender, EventArgs e)
        {
            var dic = GetDict(GetGenerateMode());
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < PasswdLength; ++i)
            {
                var m = dic[KRNG.GetInt(0, dic.Length)].dict;
                sb.Append(m[KRNG.GetInt(0, m.Length)]);
            }
            ShowPassword(sb.ToString());
        }

        private void SkbLength_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
            => FlashTxvPasswdLength();

        private void FlashTxvPasswdLength()
            => TxvPasswdLength.Text = $"Password Length: {PasswdLength}";

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            var mode = GetDict(GetGenerateMode());

            if (string.IsNullOrWhiteSpace(EdtMasterKey.Text) || string.IsNullOrWhiteSpace(EdtPassword.Text))
            {
                // Invalid
            }

            string pw = Passcore.GeneratePassword(mode,
                PasswdLength,
                EdtMasterKey.Text,
                EdtPassword.Text,
                EdtEnhanceField.Text
            );
            ShowPassword(pw);
        }

        private void ShowPassword(string pswd)
        {
            var a = new global::Android.App.AlertDialog.Builder(this).Create();
            a.SetTitle(Resources.GetString(Resource.String.result));
            a.SetMessage($"{pswd}\n\nStrength: {PasswordStrengthCheck.GetPasswdStrength(pswd)}");
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

        public int PasswdLength
        {
            get => PasswordLengthHelper.GetLength(SkbLength.Progress, ChkIsWeakPasswd.Checked);
        }

        private bool _isSecure = false;
        public bool IsSecure
        {
            get => _isSecure;
            set
            {
                ActivityHelper.SetSecureFlag(this, value);
                _isSecure = value;
            }
        }

    }
}
