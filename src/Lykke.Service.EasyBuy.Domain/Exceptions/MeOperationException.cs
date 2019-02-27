using System;

namespace Lykke.Service.EasyBuy.Domain.Exceptions
{
    public class MeOperationException : Exception
    {
        public MeOperationException(string transferId, int statusCode)
            : base($"Transfer {transferId} resulted in {statusCode} code from ME")
        {
            StatusCode = statusCode;
        }
        
        public int StatusCode { get; private set; }
    }
}
