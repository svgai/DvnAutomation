using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ForensicsObjects
{
    public class AccountObject : ForensicsObject
    {
        private readonly Dictionary<string, AccountType> _knownSidsMapping =
            new Dictionary<string, AccountType>
            {
                {"S-1-0", AccountType.NullAuthority},
                {"S-1-0-0", AccountType.Nobody},
                {"S-1-1", AccountType.WorldAuthority},
                {"S-1-1-0", AccountType.Everyone},
                {"S-1-2", AccountType.LocalAuthority},
                {"S-1-3", AccountType.CreatorAuthority},
                {"S-1-4", AccountType.NonUniqueAuthority},
                {"S-1-5", AccountType.NtAuthority},
                {"S-1-5-1", AccountType.Dialup},
                {"S-1-5-2", AccountType.Network},
                {"S-1-5-7", AccountType.Anonymous},
                {"S-1-5-9", AccountType.EnterpriseDomainControllers},
                {"S-1-5-18", AccountType.OperatingSystem},
                {"S-1-5-19", AccountType.LocalService},
                {"S-1-5-20", AccountType.NetworkService},
                {"S-1-5-90", AccountType.VirtualAccount}, // the windows manager class
                {"S-1-5-96", AccountType.VirtualAccount}, // the driver host
            };

        private bool _callback;
        private string _displayName;
        private string _domain;
        private string _homePath;
        private string _name;
        private string _profilePath;
        private string _samAccountName;
        private string _sid;

        internal AccountObject()
        {
            Type = ObjectTypes.Account;
        }

        public AccountObject(string sid, string name, string domain)
        {
            Type = ObjectTypes.Account;
            Sid = sid;
            Name = name;
            Domain = domain;
            MaskHashes = GetObjectIndexHashes();
            _callback = true;
        }

        /// <summary>
        ///     Gets or sets the account SID.
        /// </summary>
        public string Sid
        {
            get => _sid;
            set
            {
                _sid = value;
                if (value != null)
                {
                    var typeMappingName = _sid.Length >= 8 ? _sid.Substring(0, 8) : _sid;
                    if (_knownSidsMapping.ContainsKey(typeMappingName))
                        AccountType = _knownSidsMapping[typeMappingName];
                }

                if (_callback) MaskHashes = GetObjectIndexHashes();
            }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = !string.IsNullOrEmpty(value) ? value.ToLower() : string.Empty;
                if (_callback) MaskHashes = GetObjectIndexHashes();
            }
        }

        /// <summary>
        ///     Gets or sets the domain name.
        /// </summary>
        public string Domain
        {
            get => _domain;
            set
            {
                _domain = !string.IsNullOrEmpty(value) ? value.ToLower() : string.Empty;
                if (_domain.Contains(".")) _domain = _domain.Substring(0, _domain.IndexOf('.'));
                if (_callback) MaskHashes = GetObjectIndexHashes();
            }
        }

        /// <summary>
        ///     Gets or sets the account type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountType AccountType { get; set; }

        /// <summary>
        ///     Gets or sets the SAM account name
        /// </summary>
        internal string SamAccountName
        {
            get => _samAccountName;
            set => _samAccountName = value?.ToLower();
        }

        internal string DisplayName
        {
            get => _displayName;
            set => _displayName = value?.ToLower();
        }

        /// <summary>
        ///     Gets or sets the expiration timestamp
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        internal long ExpireTimestamp { get; set; }

        [JsonConverter(typeof(TimestampConverter))]
        internal long MembershipExpireTimestamp { get; set; }

        /// <summary>
        ///     Gets or sets the home path
        /// </summary>
        internal string HomePath
        {
            get => _homePath;
            set => _homePath = value?.ToLower();
        }

        /// <summary>
        ///     Gets or sets the password set timestamp
        /// </summary>
        [JsonConverter(typeof(TimestampConverter))]
        internal long PasswordSetTimestamp { get; set; }

        /// <summary>
        ///     Gets or sets the primary group Id
        /// </summary>
        internal int PrimaryGroupId { get; set; }

        /// <summary>
        ///     Gets or sets the profile path
        /// </summary>
        public string ProfilePath
        {
            get => _profilePath;
            internal set => _profilePath = value?.ToLower();
        }

        internal UserAccessControls UserAccountControl { get; set; }

        [JsonIgnore] public long CurrentTimestamp { get; set; }


        public override bool Equals(object obj)
        {
            return obj is AccountObject other && Equals(UserAccountControl, other.UserAccountControl) &&
                   CurrentTimestamp == other.CurrentTimestamp &&
                   _displayName == other._displayName &&
                   _domain == other._domain &&
                   ExpireTimestamp == other.ExpireTimestamp &&
                   _homePath == other._homePath &&
                   _name == other._name &&
                   PasswordSetTimestamp == other.PasswordSetTimestamp &&
                   PrimaryGroupId == other.PrimaryGroupId &&
                   _profilePath == other._profilePath &&
                   _samAccountName == other._samAccountName &&
                   _sid == other._sid &&
                   AccountType == other.AccountType;
        }

        /// <summary>
        ///     The get hash code.
        /// </summary>
        /// <returns>
        ///     The hash of a forensics object
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _domain != null ? _domain.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_sid != null ? _sid.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     Merges the object with another forensics object
        /// </summary>
        /// <param name="obj">
        ///     The object to merge with.
        /// </param>
        /// <returns>
        ///     The true if merge was successful.
        /// </returns>
        public override MergeResult Merge(ForensicsObject obj)
        {
            // Cannot merge two objects of different types
            if (Type != obj.Type)
            {
                Debug.WriteLine($"Cannot merge object of Type {Type} with object of Type {obj.Type}");
                return MergeResult.TypeMismatch;
            }

            var localSid = Sid;
            var localName = Name;
            var localDomain = Domain;
            var localSamAccountName = SamAccountName;
            var localDisplayName = DisplayName;
            var localHomePath = HomePath;
            var localProfilePath = ProfilePath;

            var other = (AccountObject)obj;
            if (!string.IsNullOrEmpty(other.Sid))
            {
                if (!string.IsNullOrEmpty(Sid) && !Equals(Sid, other.Sid)) return MergeResult.PropertiesConflict;
                localSid = other.Sid;
            }

            if (!string.IsNullOrEmpty(other.Name))
            {
                if (!string.IsNullOrEmpty(Name) && Name != other.Name) return MergeResult.PropertiesConflict;
                localName = other.Name;
            }

            if (!string.IsNullOrEmpty(other.Domain))
            {
                if (!string.IsNullOrEmpty(Domain) && Domain != other.Domain) return MergeResult.PropertiesConflict;
                localDomain = other.Domain;
            }

            if (!string.IsNullOrEmpty(other.SamAccountName))
                localSamAccountName = other.CurrentTimestamp >= CurrentTimestamp || string.IsNullOrEmpty(SamAccountName)
                    ? other.SamAccountName
                    : SamAccountName;
            if (!string.IsNullOrEmpty(other.DisplayName))
                localDisplayName = other.CurrentTimestamp >= CurrentTimestamp || string.IsNullOrEmpty(DisplayName)
                    ? other.DisplayName
                    : DisplayName;

            var localExpireTimestamp = other.CurrentTimestamp >= CurrentTimestamp
                ? other.ExpireTimestamp
                : ExpireTimestamp;
            if (!string.IsNullOrEmpty(other.HomePath))
                localHomePath = other.CurrentTimestamp >= CurrentTimestamp || string.IsNullOrEmpty(HomePath)
                    ? other.HomePath
                    : HomePath;

            var localPasswordSetTimestamp = other.CurrentTimestamp >= CurrentTimestamp
                ? other.PasswordSetTimestamp
                : PasswordSetTimestamp;
            var localPrimaryGroupId = other.CurrentTimestamp >= CurrentTimestamp
                ? other.PrimaryGroupId
                : PrimaryGroupId;
            if (!string.IsNullOrEmpty(other.ProfilePath))
                localProfilePath = other.CurrentTimestamp >= CurrentTimestamp || string.IsNullOrEmpty(ProfilePath)
                    ? other.ProfilePath
                    : ProfilePath;
            if (other.UserAccountControl != null)
            {
                if (UserAccountControl == null) UserAccountControl = other.UserAccountControl;
                else
                    UserAccountControl = other.CurrentTimestamp >= CurrentTimestamp
                        ? UserAccountControl.Merge(other.UserAccountControl)
                        : other.UserAccountControl.Merge(UserAccountControl);
            }

            try
            {
                Properties = Properties.Union(obj.Properties).ToDictionary(s => s.Key,
                    s => s.Value);
            }
            catch (Exception)
            {
                return MergeResult.PropertiesConflict;
            }

            Sid = localSid;
            Name = localName;
            Domain = localDomain;
            SamAccountName = localSamAccountName;
            DisplayName = localDisplayName;
            ExpireTimestamp = localExpireTimestamp;
            HomePath = localHomePath;
            PasswordSetTimestamp = localPasswordSetTimestamp;
            PrimaryGroupId = localPrimaryGroupId;
            ProfilePath = localProfilePath;
            CurrentTimestamp = other.CurrentTimestamp >= CurrentTimestamp ? other.CurrentTimestamp : CurrentTimestamp;
            return MergeResult.Success;
        }

        /// <summary>
        ///     Gets type-related warehouse masks of an object
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GetIndexMasks()
        {
            return new List<int>
            {
                0,
                2, // Name
                3, // Name + Domain
                4, // Sid
                5, // Sid + Domain
                6, // Sid + Name
                7 // Sid + Name + Domain
            };
        }

        private string GetObjectHash(int mask)
        {
            var hash = MD5.Create();
            var inputSBuilder = new StringBuilder();
            if (((mask >> 2) & 1) == 1)
            {
                if (!string.IsNullOrEmpty(Sid)) inputSBuilder.Append(Sid);
                else return null;
            }

            if (((mask >> 1) & 1) == 1)
            {
                if (!string.IsNullOrEmpty(Name)) inputSBuilder.Append(Name);
                else return null;
            }

            if (((mask >> 0) & 1) == 1)
            {
                if (!string.IsNullOrEmpty(Domain)) inputSBuilder.Append(Domain);
                else return null;
            }

            if (string.IsNullOrEmpty(inputSBuilder.ToString())) return null;

            var data = hash.ComputeHash(Encoding.UTF8.GetBytes(inputSBuilder.ToString()));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        private Dictionary<int, string> GetObjectIndexHashes()
        {
            return GetIndexMasks().ToDictionary(x => x, GetObjectHash).Where(x => !string.IsNullOrEmpty(x.Value))
                .ToDictionary(x => x.Key, y => y.Value);
        }

        private void AddProperty(string propertyKey, string propertyValue)
        {
            switch (propertyKey)
            {
                case ObjectPropertyNames.Sid:
                    Sid = propertyValue;
                    break;
                case ObjectPropertyNames.Name:
                    Name = propertyValue;
                    break;
                case ObjectPropertyNames.Domain:
                    Domain = propertyValue;
                    break;
                case ObjectPropertyNames.SamAccountName:
                    SamAccountName = propertyValue;
                    break;
                case ObjectPropertyNames.DisplayName:
                    DisplayName = propertyValue;
                    break;
                case ObjectPropertyNames.ExpireTimestamp:
                    ExpireTimestamp = ForensicsTimestampParser.Parse(propertyValue);
                    break;
                case ObjectPropertyNames.PasswordSetTimestamp:
                    PasswordSetTimestamp = ForensicsTimestampParser.Parse(propertyValue);
                    break;
                case ObjectPropertyNames.HomePath:
                    HomePath = propertyValue;
                    break;
                case ObjectPropertyNames.PrimaryGroupId:
                    PrimaryGroupId = int.Parse(propertyValue);
                    break;
                case ObjectPropertyNames.ProfilePath:
                    ProfilePath = propertyValue;
                    break;
                case ObjectPropertyNames.UserAccountControl:
                    UserAccountControl = UserAccessControls.FromString(propertyValue);
                    break;
                default:
                    if (Properties == null) Properties = new Dictionary<string, string>();
                    Properties[propertyKey] = propertyValue;
                    break;
            }
        }
    }
}

