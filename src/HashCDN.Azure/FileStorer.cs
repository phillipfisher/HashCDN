using Azure.Storage.Blobs;
using Azure.Storage;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Net.Http.Headers;

namespace HashCDN.Azure
{
    internal class FileStorer : IFileStorer
    {
        private readonly ILogger log;
        private readonly AzureStorageOptions settings;
        private readonly BlobContainerClient client;

        public FileStorer(ILogger<FileStorer> log, IOptions<AzureStorageOptions> settings)
        {
            this.log = log;
            this.settings = settings.Value;

            this.settings.EnsureIsValid();

            client = CreateClient();
        }

        public async Task<StoreResponse> Store(string name, byte[] data, string contentType, CancellationToken cancellationToken)
        {
            try
            {
                var finalUri = new Uri(settings.PublicUri, name);

                log.LogDebug($"Attempting to store data in Azure for hash: {name} and data of lenght: {data.LongLength}");
                BlobClient blobClient = client.GetBlobClient(name);
                if (await blobClient.ExistsAsync(cancellationToken))
                    return StoreResponse.Already(name, finalUri);

                var options = new BlobUploadOptions()
                {
                    AccessTier = AccessTier.Hot,
                };
                if (contentType != null)
                    options.HttpHeaders = new BlobHttpHeaders() { ContentType = contentType };

                await blobClient.UploadAsync(new BinaryData(data), options, cancellationToken);

                log.LogDebug($"Stored successfully data in Azure for hash: {name}");

                return StoreResponse.Stored(name, finalUri);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Error storing data for hash: {name} in Azure");
                return StoreResponse.Error();
            }
        }

        private BlobContainerClient CreateClient()
        {
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(settings.AccountName, settings.AccountKey);

            var blobUri = new Uri("https://" + settings.AccountName + ".blob.core.windows.net");

            var service = new BlobServiceClient(blobUri, sharedKeyCredential);

            var container = service.GetBlobContainerClient(settings.ContainerName);
            container.CreateIfNotExists(PublicAccessType.Blob);

            var policy = container.GetAccessPolicy().Value;
            if (policy.BlobPublicAccess != PublicAccessType.Blob)
            {
                container.SetAccessPolicy(PublicAccessType.Blob);
            }

            return container;
        }
    }
}
