using System;

namespace Lykke.Service.EasyBuy.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string name = null, string value = null)
            : base(
                string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value)
                    ? $"{name} equal to {value} not found."
                    : "Entity not found.")
        {
        }
    }
}
