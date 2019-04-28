using System;

namespace Lykke.Service.EasyBuy.Domain.Exceptions
{
    public class ExchangeOperationException : Exception
    {
        public ExchangeOperationException()
        {
        }
        
        public ExchangeOperationException(string message)
            : base(message)
        {
        }
    }
}
