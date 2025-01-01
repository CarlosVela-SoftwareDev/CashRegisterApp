namespace POS
{
    public static class CurrencyConfiguration
    {
        public static IReadOnlyList<decimal> Denominations { get; private set; } = new List<decimal>();

        // Sets the denominations for the POS globally
        public static void ConfigureDenominations(IEnumerable<decimal> denominations)
        {
            if (denominations == null || !denominations.Any())
                throw new ArgumentException("Denominations cannot be null or empty.");

            Denominations = denominations.OrderByDescending(d => d).ToList();
        }
    }
}
