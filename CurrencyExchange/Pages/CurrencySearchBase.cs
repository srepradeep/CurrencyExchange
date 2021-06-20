using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using CurrencyExchange.Data;
using CurrencyExchange.Service;

namespace CurrencyExchange.Pages
{
    public class CurrencySearchBase : ComponentBase
    {
        [Inject]
        public IRatesService RatesService { get; set; }
        [Parameter]
        public CurrencyRates CurrencyRate { get; set; }
        protected string SearchByCurrency { get; set; } = string.Empty;
        protected bool IsSearchDisabled { get; set; } = true;
        protected string SearchedCurency { get; set; }
        protected string SearchedCurencySymbol { get; set; } = string.Empty;
        protected CurrencyRates SelectedcurrencyRate { get; set; }
        protected string SearchCurrencyExchangeValue { get; set; } = string.Empty;
        protected async Task GetLatestCurrencyValues()
        {
            SelectedcurrencyRate = await Task<CurrencyRates>.Run(() => RatesService.GetLatestCurrencyExchangeRates(SearchedCurencySymbol));
            SearchCurrencyExchangeValue = SelectedcurrencyRate.CurrencyRateDictionary[SearchedCurencySymbol];
            SearchedCurency = "Rate is : " + SearchCurrencyExchangeValue;
        }
        protected void OnSearch()
        {
            SearchedCurency = "";
            if (!string.IsNullOrEmpty(SearchByCurrency))
            {
                string searchCur = CurrencyRate.CurrencyDescription.Values.FirstOrDefault(e => e.StartsWith(SearchByCurrency, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(searchCur))
                {
                    SearchedCurency = "The currency searched is : " + searchCur;
                    IsSearchDisabled = false;
                    SearchByCurrency = searchCur;
                    SearchedCurencySymbol = CurrencyRate.CurrencyDescription.FirstOrDefault(x => x.Value == searchCur).Key;
                }
                else
                {
                    SearchedCurency = "No currency found!, search for a valid currency";
                    IsSearchDisabled = true;
                }
            }
            else
            {
                IsSearchDisabled = true;
                SearchedCurency = "";
                SearchCurrencyExchangeValue = "";
            }
        }
    }


}
