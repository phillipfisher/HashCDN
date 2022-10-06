using System;
using System.Collections.Generic;
using System.Text;

namespace HashCDN.Azure
{
    internal class AzureStorageOptions
    {
        public string AccountName { get; set; } = string.Empty;
        public string AccountKey { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
        public string PublicUrl { get; set; } = string.Empty;
        public Uri PublicUri { get; private set; }

        internal void EnsureIsValid()
        {
            if (string.IsNullOrWhiteSpace(AccountName))
                throw new ArgumentException(nameof(AccountName) + " is required and cannot be empty.\n (ex: \"myblob\")", nameof(AccountName));
            if (!AccountNameIsValid())
                throw new ArgumentException(nameof(AccountName) + " is not valid.\n (ex: \"myblob\")", nameof(AccountName));

            if (string.IsNullOrWhiteSpace(AccountKey))
                throw new ArgumentException(nameof(AccountKey) + " is required and cannot be empty.", nameof(AccountKey));

            if (string.IsNullOrWhiteSpace(ContainerName))
                throw new ArgumentException(nameof(ContainerName) + " is required and cannot be empty.\n (ex: \"MyContainerName\")", nameof(ContainerName));
            if (!ContainerNameIsValid())
                throw new ArgumentException(nameof(ContainerName) + " is not valid.\n (ex: \"MyContainerName\")", nameof(ContainerName));

            if (string.IsNullOrWhiteSpace(PublicUrl))
            {
                PublicUri = new Uri("https://" + AccountName + ".blob.core.windows.net/" + ContainerName + "/");
            }
            else
            {
                if (!Uri.TryCreate(PublicUrl, UriKind.Absolute, out Uri PublicUri))
                    throw new ArgumentException(nameof(PublicUri) + " is not valid.\n (ex: \"https://cdn.mysite.com\")", nameof(PublicUri));

                if (PublicUri.PathAndQuery != "")
                    throw new ArgumentException(nameof(PublicUri) + " is not valid.\n (ex: \"https://cdn.mysite.com\")", nameof(PublicUri));

                PublicUri = new Uri(PublicUri, ContainerName + "/");
            }
        }

        private bool AccountNameIsValid()
        {
            AccountName = AccountName.Trim().ToLower();

            if (AccountName.Length < 3)
                return false;

            if (AccountName.Length > 24)
                return false;

            for (int i = 0; i < AccountName.Length; i++)
            {
                var c = AccountName[i];

                if (c >= '0' && c <= '9')
                    continue;

                if (c >= 'a' && c <= 'z')
                    continue;

                return false;
            }

            return true;
        }

        private bool ContainerNameIsValid()
        {
            ContainerName = ContainerName.Trim();

            if (ContainerName.Length == 0)
                return false;

            if (ContainerName.Length > 1024)
                return false;

            for (int i = 0; i < ContainerName.Length; i++)
            {
                var c = ContainerName[i];

                if (c >= '0' && c <= '9')
                    continue;

                if (c >= 'a' && c <= 'z')
                    continue;

                if (c >= 'A' && c <= 'Z')
                    continue;

                if (c == '-')
                    continue;

                if (c == '_')
                    continue;

                return false;
            }

            return true;
        }
    }
}
