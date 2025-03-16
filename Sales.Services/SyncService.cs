using Confluent.Kafka;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Sales.Domain.Contracts;
using Sales.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


namespace Sales.Services
{
    public class SyncService : BackgroundService
    {
        ConsumerConfig config;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<SyncService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;


        public SyncService(ILogger<SyncService> logger,IConfiguration configuration, IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {

            var bootStrapServer = configuration!.GetSection("Kafka:BootstrapServers").Value;

            config = new ConsumerConfig
            {
                BootstrapServers = bootStrapServer,
                GroupId = "my-consumer-group2", // Unique group ID
                AutoOffsetReset = AutoOffsetReset.Latest // Start consuming from the beginning

            };

            _consumer = new ConsumerBuilder<Ignore, string>(config)
                .Build();

            _scopeFactory = serviceScopeFactory;

            _logger = logger;

            //Create and initialize topics string array
            var topics = new string[] { "Product.Listing","ProductType.Listing" };
            try
            {
                _consumer.Subscribe(topics);
            }
            catch(KafkaException ex)
            {
                _logger.LogError($"An Kafka error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);

                        if (consumeResult != null)
                        {
                            using (var scope = _scopeFactory.CreateScope())
                            { 
                                var salesService = scope.ServiceProvider.GetRequiredService<ISalesService>();

                                /*if (consumeResult.Topic == "Product.Added")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    await salesService.SetProductInfoInCacheAsync(consumeResult.Message.Value);
                                }

                                if (consumeResult.Topic == "Product.Listing")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    await salesService.LoadProductInfoInCacheAsync(consumeResult.Message.Value);
                                }

                                if (consumeResult.Topic == "ProductType.Listing")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    await salesService.LoadProductTypeInfoInCacheAsync(consumeResult.Message.Value);
                                }*/
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Consume error: {e.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"An error occurred: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
            finally
            {
                _consumer.Close();
            }

        }
    }
}
