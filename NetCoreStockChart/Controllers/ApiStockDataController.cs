using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreStockChart.Models;
using YahooFinanceApi;

namespace NetCoreStockChart.Controllers
{
    public class ApiStockDataController : ControllerBase
    {
        [Route("~/api/ApiStockData/{ticker}/{start}/{end}/{period}")]
        [HttpGet]

        //https://localhost:7006/api/apistockdata/ibm/2023-06-01/2023-07-01/daily
        public async Task<List<StockPriceModel>> GetStockData(string ticker, string start, string end, string period)
        {
            var p = Period.Daily;
            if (period.ToLower() == "weekly") p = Period.Weekly;
            else if (period.ToLower() == "monthly") p = Period.Monthly;
            var startDate = DateTime.Parse(start);
            var endDate = DateTime.Parse(end);
            
            var hist = await Yahoo.GetHistoricalAsync(ticker, startDate, endDate, p);

            List<StockPriceModel> models = new List<StockPriceModel>();
            foreach (var data in hist)
            {
                models.Add(new StockPriceModel
                {
                    Ticker = ticker,
                    Date = data.DateTime.ToString("yyyy-MM-dd"),
                    Open = data.Open,
                    High = data.High,
                    Low = data.Low,
                    Close = data.Close,
                    AdjustedClose = data.AdjustedClose,
                    Volume = data.Volume
                }); 
            }
            return models;
        }
    }
}
