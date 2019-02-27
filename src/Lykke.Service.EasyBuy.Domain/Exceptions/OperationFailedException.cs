using System;

namespace Lykke.Service.EasyBuy.Domain.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message)
            : base(message)
        {
        }
        
        public OperationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
