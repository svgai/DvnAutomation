namespace ForensicsObjects
{
    public class UserAccessControls
    {
        public bool? AccountEnabled { get; set; }
        public bool? HomeDirectoryRequired { get; set; }
        public bool? PasswordRequired { get; set; }
        public bool? TempDuplicateAccount { get; set; }
        public bool? NormalAccount { get; set; }
        public bool? MnsLogonAccount { get; set; }
        public bool? InterDomainTrustAccount { get; set; }
        public bool? WorkstationTrustAccount { get; set; }
        public bool? ServerTrustAccount { get; set; }
        public bool? PasswordCanExpire { get; set; }
        public bool? AccountUnlocked { get; set; }
        public bool? EncryptedTextPasswordAllowed { get; set; }
        public bool? SmartCardRequired { get; set; }
        public bool? TrustedForDelegation { get; set; }
        public bool? Delegated { get; set; }
        public bool? UsesDesKeyOnly { get; set; }
        public bool? RequirePreAuth { get; set; }
        public bool? PasswordExpired { get; set; }
        public bool? TrustedToAuthForDelegation { get; set; }
        public bool? IncludeAuthorizationInformation { get; set; }

        public UserAccessControls Merge(UserAccessControls other)
        {
            if (other.AccountEnabled != null) AccountEnabled = other.AccountEnabled;
            if (other.HomeDirectoryRequired != null) HomeDirectoryRequired = other.HomeDirectoryRequired;
            if (other.PasswordRequired != null) PasswordRequired = other.PasswordRequired;
            if (other.TempDuplicateAccount != null) TempDuplicateAccount = other.TempDuplicateAccount;
            if (other.NormalAccount != null) NormalAccount = other.NormalAccount;
            if (other.MnsLogonAccount != null) MnsLogonAccount = other.MnsLogonAccount;
            if (other.InterDomainTrustAccount != null) InterDomainTrustAccount = other.InterDomainTrustAccount;
            if (other.WorkstationTrustAccount != null) WorkstationTrustAccount = other.WorkstationTrustAccount;
            if (other.ServerTrustAccount != null) ServerTrustAccount = other.ServerTrustAccount;
            if (other.PasswordCanExpire != null) PasswordCanExpire = other.PasswordCanExpire;
            if (other.AccountUnlocked != null) AccountUnlocked = other.AccountUnlocked;
            if (other.EncryptedTextPasswordAllowed != null)
                EncryptedTextPasswordAllowed = other.EncryptedTextPasswordAllowed;
            if (other.SmartCardRequired != null) SmartCardRequired = other.SmartCardRequired;
            if (other.TrustedForDelegation != null) TrustedForDelegation = other.TrustedForDelegation;
            if (other.Delegated != null) Delegated = other.Delegated;
            if (other.UsesDesKeyOnly != null) UsesDesKeyOnly = other.UsesDesKeyOnly;
            if (other.RequirePreAuth != null) RequirePreAuth = other.RequirePreAuth;
            if (other.PasswordExpired != null) PasswordExpired = other.PasswordExpired;
            if (other.TrustedToAuthForDelegation != null) TrustedToAuthForDelegation = other.TrustedToAuthForDelegation;
            if (other.IncludeAuthorizationInformation != null)
                IncludeAuthorizationInformation = other.IncludeAuthorizationInformation;
            return this;
        }

        public static UserAccessControls FromString(string value)
        {
            return new UserAccessControls();
        }
    }
}
