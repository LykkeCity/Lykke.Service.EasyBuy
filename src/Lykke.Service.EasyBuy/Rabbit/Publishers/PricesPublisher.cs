using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.EasyBuy.Domain.Services;
using Lykke.Service.EasyBuy.DomainServices.Extensions;
using Lykke.Service.EasyBuy.Settings.ServiceSettings.Rabbit.Publishers;

namespace Lykke.Service.EasyBuy.Rabbit.Publishers
{
    public class PricesPublisher : IPricesPublisher, IDisposable
    {
        private readonly PublisherSettings _publisherSettings;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;

        private RabbitMqPublisher<Contract.Price> _publisher;

        public PricesPublisher(
            PublisherSettings publisherSettings,
            ILogFactory logFactory)
        {
            _publisherSettings = publisherSettings;
            _logFactory = logFactory;
            _log = logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .ForPublisher(_publisherSettings.ConnectionString, _publisherSettings.Exchange);

            _publisher = new RabbitMqPublisher<Contract.Price>(_logFactory, settings)
                .SetSerializer(new JsonMessageSerializer<Contract.Price>())
                .DisableInMemoryQueuePersistence()
                .Start();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }

        public void Dispose()
        {
            _publisher?.Stop();
        }

        public async Task PublishAsync(Domain.Entities.Prices.Price price)
        {
            try
            {
                await _publisher.ProduceAsync(Mapper.Map<Contract.Price>(price));
            }
            catch (Exception exception)
            {
                _log.WarningWithDetails("An error occurred while publishing price.", exception, price);
                throw;
            }
        }
    }
}
