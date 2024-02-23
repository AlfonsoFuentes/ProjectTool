namespace Client.Infrastructure.Managers.CurrencyApis
{
    public interface IRate
    {
        Task<ConversionRate> GetRates();
    }
    internal class Rates : IRate
    {
        private readonly HttpClient _httpClient;

        public Rates(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ConversionRate> GetRates()
        {
            string URLString = "https://v6.exchangerate-api.com/v6/634bdff9683829177dafb6e4/latest/USD";
            var response = await _httpClient.GetAsync(URLString);
            var result = await response.ToObject<API_Obj>();
            var rates = new ConversionRate();
            rates.COP = result != null && result.result == "success" ? result.conversion_rates.COP : 4000;
            rates.EUR = result != null && result.result == "success" ? result.conversion_rates.EUR : 1;
            return rates;
        }
    }
    public class API_Obj
    {
        public string result { get; set; } = string.Empty;
        public string documentation { get; set; } = string.Empty;
        public string terms_of_use { get; set; } = string.Empty;

        public string base_code { get; set; } = string.Empty;
        public ConversionRate conversion_rates { get; set; } = null!;
    }

    public class ConversionRate
    {
        public double COP { get; set; }


        public double EUR { get; set; }

    }
    public static class DependencyContainer
    {
        public static IServiceCollection CurrencyService(
            this IServiceCollection services)
        {
            services.AddScoped<IRate, Rates>();



            return services;
        }

    }
}
