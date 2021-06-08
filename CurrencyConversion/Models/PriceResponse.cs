namespace CurrencyConversion.Models
{
    public class PriceResponse
    {
        public PriceResponse(double amount, string toCurrency)
        {
            Amount = amount;
            Currency = toCurrency;
        }
        /// <summary>
        /// Amount in target currency
        /// </summary>
        public double Amount { get; private set; }

        /// <summary>
        /// Target currency
        /// </summary>
        public string Currency { get; set; }
    }
}
