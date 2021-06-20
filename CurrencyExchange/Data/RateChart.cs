using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyExchange.Data
{
    class RateChart
    {
        public string JsonString { get; set; }
        public RateChart()
        {

        }
        public void SetData(List<string> xAxis, double[] yAxis)
        {
            var json = new
            {
                chart = new
                {
                    type = "line"
                },
                title = new
                {
                    text = "Currency Trend"
                },
                subtitle = new
                {
                    text = "Source: OpenExchanges.org"
                },
                xAxis = new
                {
                    Categories = xAxis,
                },
                yAxis = new
                {
                    title = new
                    {
                        text = "Currency Rates"
                    }
                },
                plotOptions = new
                {
                    line = new
                    {
                        dataLabels = new
                        {
                            enabled = true
                        },
                        enableMouseTracking = true
                    }
                },
                series = new dynamic []
                {
                    new {name = "Currency" },
                    new {data = yAxis }
                }
            };
            this.JsonString = JsonConvert.SerializeObject(json);
        }
    }
}
