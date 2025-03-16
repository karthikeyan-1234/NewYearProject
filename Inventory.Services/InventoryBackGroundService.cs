using Confluent.Kafka;

using Inventory.Domain;
using Inventory.Domain.Contracts;
using Inventory.Domain.Entities;
using Inventory.Services.Contracts;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class InventoryBackGroundService : BackgroundService
    {
        ConsumerConfig config;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<InventoryBackGroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public InventoryBackGroundService(ILogger<InventoryBackGroundService> logger, IConfiguration configuration, IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
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
            var topics = new string[] { "PurchaseDetails.Added", "PurchaseDetails.Updated", "SaleDetails.Added", "SaleDetails.Updated" };
            try
            {
                _consumer.Subscribe(topics);
            }
            catch (KafkaException ex)
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

            Console.WriteLine("Inventory Background Service started...");

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
                                var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();

                                #region Single Purchase Detail CRUD Operations

                                if (consumeResult.Topic == "PurchaseDetail.Added")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var purchaseDetail = JsonSerializer.Deserialize<PurchaseDetail>(consumeResult.Message.Value)!;
                                    await inventoryService.AddPurchaseDetail(purchaseDetail);
                                }

                                if (consumeResult.Topic == "PurchaseDetail.Updated")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var purchaseDetail = JsonSerializer.Deserialize<PurchaseDetail>(consumeResult.Message.Value)!;
                                    await inventoryService.UpdatePurchaseDetail(purchaseDetail);
                                }

                                if (consumeResult.Topic == "PurchaseDetail.Deleted")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var purchaseDetail = JsonSerializer.Deserialize<PurchaseDetail>(consumeResult.Message.Value)!;
                                    await inventoryService.RemovePurchaseDetail(purchaseDetail.Id);
                                }

                                #endregion

                                #region Multiple Purchase Detail CRUD Operations

                                if (consumeResult.Topic == "PurchaseDetails.Added")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var purchaseDetails = JsonSerializer.Deserialize<List<PurchaseDetail>>(consumeResult.Message.Value)!;
                                    await inventoryService.AddPurchaseDetails(purchaseDetails);

                                }

                                if (consumeResult.Topic == "PurchaseDetails.Updated")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var purchaseDetails = JsonSerializer.Deserialize<List<PurchaseDetail>>(consumeResult.Message.Value)!;
                                    await inventoryService.UpdatePurchaseDetails(purchaseDetails);
                                }

                                if (consumeResult.Topic == "PurchaseDetails.Deleted")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var purchaseDetails = JsonSerializer.Deserialize<List<PurchaseDetail>>(consumeResult.Message.Value)!;
                                    await inventoryService.RemovePurchaseDetailsAsync(purchaseDetails);
                                }

                                #endregion

                                #region Multiple Sales Detail CRUD Operations

                                if (consumeResult.Topic == "SaleDetails.Added")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var salesDetails = JsonSerializer.Deserialize<List<SaleDetail>>(consumeResult.Message.Value)!;
                                    await inventoryService.AddSalesDetailsAsync(salesDetails);
                                }

                                if (consumeResult.Topic == "SaleDetails.Updated")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var salesDetails = JsonSerializer.Deserialize<List<SaleDetail>>(consumeResult.Message.Value)!;
                                    await inventoryService.UpdateSalesDetailsAsync(salesDetails);
                                }

                                if (consumeResult.Topic == "SalesDetails.Deleted")
                                {
                                    _logger.LogInformation($"Received message: {consumeResult.Message.Value}");
                                    var salesDetails = JsonSerializer.Deserialize<List<SaleDetail>>(consumeResult.Message.Value)!;
                                    await inventoryService.RemoveSalesDetailsAsync(salesDetails);
                                }

                                #endregion
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
