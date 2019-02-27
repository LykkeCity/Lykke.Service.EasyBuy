namespace Lykke.Service.EasyBuy.Domain.Exceptions
{
    public class MeNotEnoughFundsException : MeOperationException
    {
        public MeNotEnoughFundsException(string transferId) : base(transferId, 401)
        {
        }
    }
}
