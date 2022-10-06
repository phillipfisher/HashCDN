using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashCDN
{
    public class CDN
    {
        private static readonly HashAlgorithm hasher = SHA256.Create();

        private readonly IFileStorer store;
        
        public CDN(IFileStorer store)
        {
            this.store = store;
        }

        public Task<StoreResponse> Store(byte[] data, string contentType = null, CancellationToken cancellationToken = default)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0) throw new ArgumentNullException(nameof(data));

            var name = ComputeHash(data);

            return store.Store(name, data, contentType, cancellationToken);
        }

        public async Task<StoreResponse> Store(FileInfo fi, string contentType = null, CancellationToken cancellationToken = default)
        {
            if (fi == null) throw new ArgumentNullException(nameof(fi));
            if (!fi.Exists) throw new ArgumentException($"File doesn't exist ({fi.FullName})", nameof(fi));

            return await Store(await File.ReadAllBytesAsync(fi.FullName, cancellationToken), contentType, cancellationToken);
        }

        public async Task<StoreResponse> Store(IFormFile file, string contentType = null, CancellationToken cancellationToken = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (file.Length == 0) return StoreResponse.Error();

            var data = new byte[file.Length];
            using var stream = file.OpenReadStream();
            await stream.ReadAsync(data, 0, data.Length, cancellationToken);

            return await Store(data, contentType ?? file.ContentType, cancellationToken);
        }

        private static string ComputeHash(byte[] data)
        {
            return hasher.ComputeHash(data).Base64UrlEncode();
        }
    }
}
