namespace POS
{
    // Handles change calculation
    public class CashRegister
    {
        /// <summary>
        /// Calculates the optimal change to return to a customer.
        /// </summary>
        /// <param name="price">The total price of the items.</param>
        /// <param name="payment">The list of denominations and their quantities provided by the customer.</param>
        /// <returns>A list of DenominationQuantity representing the change.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the payment is insufficient.</exception>
        public List<DenominationQuantity> CalculateChange(decimal price, List<DenominationQuantity> payment)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be positive.");

            decimal totalPaid = payment.Sum(p => p.Denomination * p.Quantity);
            if (totalPaid < price)
                throw new InvalidOperationException("Insufficient payment provided.");

            decimal changeDue = totalPaid - price;
            var change = new List<DenominationQuantity>();

            foreach (var denomination in CurrencyConfiguration.Denominations)
            {
                if (changeDue <= 0) break;

                int count = (int)(changeDue / denomination);
                if (count > 0)
                {
                    change.Add(new DenominationQuantity(denomination, count));
                    changeDue -= count * denomination;
                }
            }

            // If exact change cannot be provided, return the maximum possible change
            if (Math.Round(changeDue, 2) > 0)
            {
                foreach (var denomination in CurrencyConfiguration.Denominations)
                {
                    if (changeDue <= 0) break;

                    decimal remaining = changeDue;
                    var existingDenomination = change.FirstOrDefault(c => c.Denomination == denomination);

                    if (existingDenomination != null)
                    {
                        changeDue -= existingDenomination.Denomination * existingDenomination.Quantity;
                    }
                }
            }

            return change;
        }
    }
}
