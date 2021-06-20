using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using CurrencyExchange.Data;
using CurrencyExchange.Service;


namespace CurrencyExchange.Pages
{
    public class CurrencyHistorySearchBase : ComponentBase
    {
        [Inject]
        protected IRatesService RatesService { get; set; }
        [Parameter]
        public CurrencyRates CurrencyRate { get; set; }
        protected string SearchByCurrencyAndDate { get; set; } = string.Empty;
        protected string SearchedCurencyDate { get; set; }
        protected string SearchedCurencyDateSymbol { get; set; } = string.Empty;
        protected DateTime SelectedDate { get; set; } = DateTime.Now;
        protected string SearchCurrencyDateExchangeValue { get; set; } = string.Empty;
        protected DateTime TodaysDate { get; set; } = DateTime.Now;
        protected bool IsdateSearchDisabled { get; set; } = true;
        private CurrencyRates SelectedcurrencyRateOn { get; set; }
        protected void OnGetCurrency()
        {
            SearchedCurencyDate = "";
            if (!string.IsNullOrEmpty(SearchByCurrencyAndDate))
            {
                string searchCur = CurrencyRate.CurrencyDescription.Values.FirstOrDefault(e => e.StartsWith(SearchByCurrencyAndDate, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(searchCur))
                {
                    SearchedCurencyDate = "The currency searched is : " + searchCur;
                    IsdateSearchDisabled = false;
                    SearchByCurrencyAndDate = searchCur;
                    SearchedCurencyDateSymbol = CurrencyRate.CurrencyDescription.FirstOrDefault(x => x.Value == searchCur).Key;
                }
                else
                {
                    SearchedCurencyDate = "No currency found!, search for a valid currency";
                    IsdateSearchDisabled = true;
                    SearchedCurencyDateSymbol = string.Empty;
                }
            }
            else
            {
                IsdateSearchDisabled = true;
                SearchedCurencyDate = "";
                SearchedCurencyDateSymbol = "";
            }
        }
        protected void DateChanged(ChangeEventArgs e)
        {
            var myVal = e.Value.ToString();
            SelectedDate = DateTime.Parse(myVal);
            ValidateDate();
        }
        protected async Task GetCurrencyValuesOn()
        {
            SelectedcurrencyRateOn = await Task<CurrencyRates>.Run(() => RatesService.GetCurrencyExchangeRatesOn(SelectedDate, SearchedCurencyDateSymbol));
            SearchCurrencyDateExchangeValue = SelectedcurrencyRateOn.CurrencyRateDictionary[SearchedCurencyDateSymbol];
            SearchedCurencyDate = "Rate is : " + SearchCurrencyDateExchangeValue;
        }
        protected void ValidateDate()
        {
            if (SelectedDate > DateTime.Now)
            {
                SearchedCurencyDate = "Future date is not allowed!,Select a past date.";
                IsdateSearchDisabled = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(SearchedCurencyDateSymbol))
                {
                    IsdateSearchDisabled = false;
                }
            }
        }
    }
    
}
