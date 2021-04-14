namespace Passcore.Android.Models
{
    public class Config
    {
        public delegate void ValueHandler();

        public event ValueHandler ValueChanged = delegate { }; // { } can prevent NRE

        private string _masterKey;
        public string MasterKey
        {
            get => _masterKey;
            set
            {
                _masterKey = value;
                if (IsStoreMasterKey)
                    ValueChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                if (IsStorePassword)
                    ValueChanged();
            }
        }

        private string _enhance;
        public string Enhance
        {
            get => _enhance;
            set
            {
                _enhance = value;
                if (IsStoreEnhance)
                    ValueChanged();
            }
        }

        private bool _isStoreMasterKey;
        public bool IsStoreMasterKey
        {
            get => _isStoreMasterKey;
            set
            {
                ValueChanged();
                _isStoreMasterKey = value;
            }
        }

        private bool _isStorePassword;
        public bool IsStorePassword
        {
            get => _isStorePassword;
            set
            {
                _isStorePassword = value;
                ValueChanged();
            }
        }


        private bool _isStoreEnhance;
        public bool IsStoreEnhance
        {
            get => _isStoreEnhance;
            set
            {
                _isStoreEnhance = value;
                ValueChanged();
            }
        }

        private bool _isCharRequired = true;
        public bool IsCharRequired
        {
            get => _isCharRequired;
            set
            {
                _isCharRequired = value;
                if (IsStoreCharRequired)
                    ValueChanged();
            }
        }


        private bool _isStoreCharRequired;
        public bool IsStoreCharRequired
        {
            get => _isStoreCharRequired;
            set
            {
                _isStoreCharRequired = value;
                ValueChanged();
            }
        }

        private bool _isWeakPasswd;
        public bool IsWeakPasswd
        {
            get => _isWeakPasswd;
            set
            {
                _isWeakPasswd = value;
                if (IsStorePasswordLength)
                    ValueChanged();
            }
        }


        private int _passwordLengthIndex;
        public int PasswordLengthIndex
        {
            get => _passwordLengthIndex;
            set
            {
                _passwordLengthIndex = value;
                if (IsStorePasswordLength)
                    ValueChanged();
            }
        }


        private bool _isStorePasswordLength;
        public bool IsStorePasswordLength
        {
            get => _isStorePasswordLength;
            set
            {
                _isStorePasswordLength = value;
                ValueChanged();
            }
        }

        public void Clean()
        {
            if (!IsStoreEnhance)
                Enhance = null;
            if (!IsStoreMasterKey)
                MasterKey = null;
            if (!IsStorePassword)
                Password = null;
            if (!IsStorePasswordLength)
            {
                IsWeakPasswd = false;
                PasswordLengthIndex = 0;
            }
            if (!IsStoreCharRequired)
                IsCharRequired = true;
        }
    }
}