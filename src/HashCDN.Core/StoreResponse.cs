using System;
using System.Collections.Generic;
using System.Text;

namespace HashCDN
{
    public struct StoreResponse
    {
        public StoreResponseStatus Status;
        public string Name;
        public Uri FullUri;

        public static StoreResponse Error() => new StoreResponse() { Status = StoreResponseStatus.ErrorStoring };
        public static StoreResponse Already(string name, Uri fullUri) => new StoreResponse() 
        {
            Status = StoreResponseStatus.AlreadyStored,
            Name = name,
            FullUri = fullUri,
        };
        public static StoreResponse Stored(string name, Uri fullUri) => new StoreResponse()
        {
            Status = StoreResponseStatus.Stored,
            Name = name,
            FullUri = fullUri,
        };
    }

    public enum StoreResponseStatus
    {
        AlreadyStored,
        Stored,
        ErrorStoring,
    }
}
