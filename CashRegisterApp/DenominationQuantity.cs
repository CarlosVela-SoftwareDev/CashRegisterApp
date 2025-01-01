namespace POS
{
    // Represents a single bill or coin and its quantity
    public class DenominationQuantity
    {
        public decimal Denomination { get; }
        public int Quantity { get; }

        public DenominationQuantity(decimal denomination, int quantity)
        {
            if (denomination <= 0)
                throw new ArgumentException("Denomination must be positive.");
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Denomination = denomination;
            Quantity = quantity;
        }

        public override string ToString() => $"{Quantity} x {Denomination:C}";
    }
}
