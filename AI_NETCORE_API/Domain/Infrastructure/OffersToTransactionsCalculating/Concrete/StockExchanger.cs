﻿using System;
using Domain.BusinessObject;
using Domain.Infrastructure.Logging.Abstract;
using Domain.Infrastructure.OffersToTransactionsCalculating.Abstract;
using Domain.Infrastructure.OffersToTransactionsCalculating.Request.Abstract;
using Domain.Infrastructure.OffersToTransactionsCalculating.Response.Abstract;
using Domain.Infrastructure.TransactionProcessing;
using Domain.Infrastructure.TransactionProcessing.Responses.Abstract;
using Domain.Providers.BuyOffers.Abstract;
using Domain.Providers.BuyOffers.Request.Abstract;
using Domain.Providers.BuyOffers.Request.Concrete;
using Domain.Providers.BuyOffers.Response.Abstract;
using Domain.Providers.Common.Enum;
using Domain.Providers.Configurations.Abstract;
using Domain.Providers.Configurations.Request.Concrete;
using Domain.Providers.Configurations.Response.Abstract;
using Domain.Providers.Configurations.Response.Concrete;
using Domain.Providers.SellOffers.Abstract;
using Domain.Providers.SellOffers.Request.Abstract;
using Domain.Providers.SellOffers.Request.Concrete;
using Domain.Providers.SellOffers.Response.Abstract;

namespace Domain.Infrastructure.OffersToTransactionsCalculating.Concrete
{
    public class StockExchanger : IStockExchanger
    {
        private readonly ISellOfferProvider _sellOfferProvider;
        private readonly IBuyOffersProvider _buyOffersProvider;
        private readonly ILogger _logger;
        private readonly IConfigurationsProvider _configurationsProvider;


        public StockExchanger(ISellOfferProvider sellOfferProvider, IBuyOffersProvider buyOffersProvider, ILogger logger, IConfigurationsProvider configurationsProvider)
        {
            _sellOfferProvider = sellOfferProvider;
            _buyOffersProvider = buyOffersProvider;
            _logger = logger;
            _configurationsProvider = configurationsProvider;
        }

        public IStockExchangeResponse StockExchange(IStockExchangeRequest stockExchangeRequest)
        {
            try
            {
                int companyId = stockExchangeRequest.CompanyId;
                IGetConfigurationResponse quantityFromConfiguration = new GetConfigurationResponse(new Configuration("offerWindowSize", 2), 0); //_configurationsProvider.GetConfiguration(new GetConfigurationRequest("configValue"));



                if (quantityFromConfiguration.ProvideResult != ProvideEnumResult.Success)
                {
                    //TODO inform
                }

                IGetSellOffersToStockExecutionRequest getSellOffersToStockExecutionRequest = new GetSellOffersToStockExecutionRequest(quantityFromConfiguration.Configuration.Value, companyId);
                IGetSellOffersToStockExecutionResponse sellOffersToStockExecutionResponse = _sellOfferProvider.GetSellOfferToStockExecute(getSellOffersToStockExecutionRequest);
                if (sellOffersToStockExecutionResponse.ProvideResult != ProvideEnumResult.Success)
                {
                    //TODO inform
                }
                IGetBuyOffersToStockExecutionRequest buyOffersToStockExecutionRequest = new GetBuyOffersToStockExecutionRequest(quantityFromConfiguration.Configuration.Value, companyId);
                IGetBuyOffersToStockExecutionResponse buyOffersToStockExecutionResponse = _buyOffersProvider.GetBuyOfferToStockExecution(buyOffersToStockExecutionRequest);
                if (buyOffersToStockExecutionResponse.ProvideResult != ProvideEnumResult.Success)
                {
                    //TODO inform
                }

                TransactionWindow transactionWindow = new TransactionWindow(buyOffersToStockExecutionResponse.BuyOffers,
                    sellOffersToStockExecutionResponse.SellOffers, quantityFromConfiguration.Configuration.Value);

                if (!transactionWindow.IsValid)
                {
                    //TODO inform
                }

                IProcessingTransactionWindowResult processingTransactionWindowResult = transactionWindow.Process(_logger);

                if (!processingTransactionWindowResult.SomethingDone)
                {
                    //TODO inform
                }

                //TODO Do Changes on database and return result

            }
            catch (Exception ex)
            {
                _logger.Log(ex);

            }
            throw new NotImplementedException();

        }
    }
}
