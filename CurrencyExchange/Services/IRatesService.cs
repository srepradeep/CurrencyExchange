using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchange.Data;

namespace CurrencyExchange.Service
{
    public interface IRatesService
    {
        Task<CurrencyRates> GetLatestCurrencyExchangeRates();

        Task<CurrencyRates> GetLatestCurrencyExchangeRates(string symbol);
        Task<CurrencyRates> GetCurrencyExchangeRatesOn(DateTime date, string symbol);
    }
}
