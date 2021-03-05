namespace Passcore.Android
{
    class PasswordStrengthCheck
    {
        public enum PasswdStrength
        {
            Invalid,
            Weak,
            Normal,
            Strong
        };

        public static PasswdStrength GetPasswdStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return PasswdStrength.Invalid;
            int iNum = 0, iLtt = 0, iSym = 0;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') iNum++;
                else if (c >= 'a' && c <= 'z') iLtt++;
                else if (c >= 'A' && c <= 'Z') iLtt++;
                else iSym++;
            }
            if (iLtt == 0 && iSym == 0) return PasswdStrength.Weak;
            if (iNum == 0 && iLtt == 0) return PasswdStrength.Weak;
            if (iNum == 0 && iSym == 0) return PasswdStrength.Weak;
            if (password.Length <= 6) return PasswdStrength.Weak;
            if (iLtt == 0) return PasswdStrength.Normal;
            if (iSym == 0) return PasswdStrength.Normal;
            if (iNum == 0) return PasswdStrength.Normal;
            if (password.Length <= 10) return PasswdStrength.Normal;
            return PasswdStrength.Strong;
        }
    }
}