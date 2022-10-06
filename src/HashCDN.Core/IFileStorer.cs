using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashCDN
{
    public interface IFileStorer
    {
        Task<StoreResponse> Store(string name, byte[] data, string contentType, CancellationToken cancellationToken);
    }
}
