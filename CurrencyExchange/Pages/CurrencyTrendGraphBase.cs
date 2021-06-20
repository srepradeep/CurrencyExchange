using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using CurrencyExchange.Data;
using CurrencyExchange.Service;

namespace CurrencyExchange.Pages
{

    public class CurrencyTrendGraphBase : ComponentBase
    {
        [Inject]
        protected IRatesService RatesService { get; set; }
        
        [Parameter]
        public CurrencyRates CurrencyRate { get; set; }

        public List<string> xAxis = new List<string>();
        public List<double> yAxis = new List<double>();
        private CurrencyRates chartCurrencyRate { get; set; }

        public string jsonString { get; set; } = string.Empty;
        public int currencyTrendTimeFrame { get; set; }
        public string SelectedChartCurrency { get; set; }

        protected string chartExchangeValue { get; set; } = string.Empty;
        public async Task CalculateChartData(string selectedChartCurrency, int currencyTrendTimeFrame)
        {
            if (!string.IsNullOrEmpty(selectedChartCurrency) && (currencyTrendTimeFrame > 0))
            {
                ClearChartData();
                int dateIndex = -1;
                for (int i = 0; i < currencyTrendTimeFrame; i++)
                {
                    DateTime SelectedDate = DateTime.Now.AddDays(dateIndex);
                    chartCurrencyRate = await Task<CurrencyRates>.Run(() => RatesService.GetCurrencyExchangeRatesOn(SelectedDate, selectedChartCurrency));
                    chartExchangeValue = chartCurrencyRate.CurrencyRateDictionary[selectedChartCurrency];

                    yAxis.Add(Double.Parse(chartExchangeValue));
                    xAxis.Add(SelectedDate.Day.ToString());
                    dateIndex -= 1;
                }
            }
        }
        public async Task SetChartCurrencyValues(ChangeEventArgs e)
        {
            SelectedChartCurrency = e.Value.ToString();
            if (!string.IsNullOrWhiteSpace(SelectedChartCurrency) && SelectedChartCurrency != "--select--")
            {
                await CalculateChartData(SelectedChartCurrency, currencyTrendTimeFrame);

                RateChart rateChartObject = new RateChart();
                rateChartObject.SetData(xAxis, yAxis.ToArray());
                jsonString = rateChartObject.JsonString;
            }
            else
            {
                SelectedChartCurrency = "";
            }
        }

        public async Task SetChartTrendValue(ChangeEventArgs e)
        {
            string selectedTimeFrame = e.Value.ToString();
            if (!string.IsNullOrWhiteSpace(selectedTimeFrame) && selectedTimeFrame != "--select--")
            {
                currencyTrendTimeFrame = int.Parse(selectedTimeFrame);
                await CalculateChartData(SelectedChartCurrency, currencyTrendTimeFrame);

                RateChart rateChartObject = new RateChart();
                rateChartObject.SetData(xAxis, yAxis.ToArray());
                jsonString = rateChartObject.JsonString;
            }
            else
            {
                currencyTrendTimeFrame = 0;
            }
        }


        void ClearChartData()
        {
            xAxis.Clear();
            yAxis.Clear();
        }
    }
}
