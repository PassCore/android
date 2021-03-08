namespace Passcore.Android.Models
{
    public class Config
    {
        public delegate void ValueHandler();

        public event ValueHandler ValueChanged;


        private string _password, _enhance, _masterKey;
        private bool _isStoreMasterKey, _isStorePassword, _isStoreEnhance;

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


        public bool IsStoreMasterKey
        {
            get => _isStoreMasterKey;
            set
            {
                _isStoreMasterKey = value;
                ValueChanged();
            }
        }
        public bool IsStorePassword
        {
            get => _isStorePassword;
            set
            {
                _isStorePassword = value;
                ValueChanged();
            }
        }
        public bool IsStoreEnhance
        {
            get => _isStoreEnhance;
            set
            {
                _isStoreEnhance = value;
                ValueChanged();
            }
        }
    }
}