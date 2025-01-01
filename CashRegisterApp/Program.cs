namespace POS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure denominations (example for the US)
            CurrencyConfiguration.ConfigureDenominations(new[] { 100m, 50m, 20m, 10m, 5m, 2m, 1m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m });
            var register = new CashRegister();
            // Example usage
            try
            {
                decimal price = 0.99m;
                var payment = new List<DenominationQuantity>
                {
                    new DenominationQuantity(1, 1)
                };
                var change = register.CalculateChange(price, payment);
                Console.WriteLine("Change:");
                foreach (var item in change)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
