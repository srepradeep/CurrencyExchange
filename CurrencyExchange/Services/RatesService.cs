using CurrencyExchange.Utilities;
using CurrencyExchange.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace CurrencyExchange.Service
{
    public class RatesService : IRatesService
    {
        private readonly  string apiBasicUri = "https://openexchangerates.org/api/latest.json?app_id=d6a829e240ef4cbfbafe6726a99f35b5";

        private readonly string apiCurrencyDescriptionUri = "https://openexchangerates.org/api/currencies.json?app_id=d6a829e240ef4cbfbafe6726a99f35b5";

        private readonly string apiCurrencyOnDateBaseUri = "https://openexchangerates.org/api/historical/";

        private readonly string apiEndUri = ".json?app_id=d6a829e240ef4cbfbafe6726a99f35b5&symbols=";

        public string  SelectedHistoryDate { get; set; }

        private CurrencyDescription _currencyDescription;

        private Dictionary<string, string> _currencyDescriptionDictionary; 

        public RatesService()
        {
            LoadCurrencyDescription();
        }
        public async void LoadCurrencyDescription()
        {
            _currencyDescription = await HttpHelper.Get<CurrencyDescription>(apiCurrencyDescriptionUri);
            _currencyDescriptionDictionary = GetValues(_currencyDescription);
        }
        public async Task<CurrencyRates> GetCurrencyExchangeRatesOn(DateTime date, string symbol)
        {
            //if (!string.IsNullOrEmpty(symbol))
            SelectedHistoryDate = date.ToString("yyyy-MM-dd");
            //SelectedHistoryDate = string.Format("{0}-{1}-{2}", date.Year.ToString(), date.Month.ToString(), date.Day.ToString());
            return await GetRateHistory(SelectedHistoryDate, symbol);
        }
        public async Task<CurrencyRates> GetLatestCurrencyExchangeRates(string symbol)
        {
            return await GetRate(symbol);
        }

        public async Task<CurrencyRates> GetRateHistory(string selectedHistoryDate,string symbol)
        {
            try
            {
                CurrencyRates currencyRate = null;
                string apiDateURL = "";
                apiDateURL = string.Concat(apiCurrencyOnDateBaseUri,selectedHistoryDate,apiEndUri,symbol);
                Root rootCurrencyObject = await HttpHelper.Get<Root>(apiDateURL);
                
                CurrencyRates currRate = new CurrencyRates();
                if (rootCurrencyObject != null)
                {
                    Dictionary<string, string> currencyRateDictionary = GetValues(rootCurrencyObject.rates);
                    currencyRate = new CurrencyRates();
                    currencyRate.ReceivedTime = DateTime.Now;
                    currencyRate.CurrencyRateDictionary = currencyRateDictionary;
                    currencyRate.CurrencyDescription = _currencyDescriptionDictionary;
                }
                return currencyRate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<CurrencyRates> GetLatestCurrencyExchangeRates()
        {
            return await GetRate();
        }
        public async Task<CurrencyRates> GetRate(string symbol=null)
        {
            try
            {
                CurrencyRates currencyRate = null;
                Root rootCurrencyObject = null;
                if (symbol == null)
                {
                    rootCurrencyObject = await HttpHelper.Get<Root>(apiBasicUri);
                }
                else
                {
                    string requestURL = string.Concat(apiBasicUri, "&symbols=", symbol);
                    rootCurrencyObject = await HttpHelper.Get<Root>(requestURL);
                }
               
                CurrencyRates currRate = new CurrencyRates();
                if (rootCurrencyObject != null)
                {
                    Dictionary<string, string> currencyRateDictionary = GetValues(rootCurrencyObject.rates);
                    currencyRate = new CurrencyRates();
                    currencyRate.ReceivedTime = DateTime.Now;
                    currencyRate.CurrencyRateDictionary = currencyRateDictionary;
                    currencyRate.CurrencyDescription = _currencyDescriptionDictionary;

                }
                return currencyRate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public Dictionary<string, string> GetValues(object obj)
        {
            return obj
                    .GetType()
                    .GetProperties()
                    .ToDictionary(p => p.Name, p => p.GetValue(obj).ToString());
        }
    }
}
