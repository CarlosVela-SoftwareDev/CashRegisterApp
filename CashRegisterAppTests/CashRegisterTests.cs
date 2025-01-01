// Unit Tests for CashRegister
namespace POS.Tests
{
    [TestClass]
    public class CashRegisterTests
    {
        private CashRegister _register;

        [TestInitialize]
        public void Setup()
        {
            // Configure denominations for testing
            CurrencyConfiguration.ConfigureDenominations(new[] { 100m, 50m, 20m, 10m, 5m, 2m, 1m, 0.50m, 0.25m, 0.10m, 0.05m, 0.01m });
            _register = new CashRegister();
        }

        [TestMethod]
        public void CalculateChange_CorrectChangeForExactPayment()
        {
            decimal price = 20.00m;
            var payment = new List<DenominationQuantity>
            {
                new DenominationQuantity(20m, 1)
            };

            var change = _register.CalculateChange(price, payment);

            Assert.AreEqual(0, change.Count, "No change should be returned for exact payment.");
        }

        [TestMethod]
        public void CalculateChange_CorrectChangeForSimpleCase()
        {
            decimal price = 15.75m;
            var payment = new List<DenominationQuantity>
            {
                new DenominationQuantity(20m, 1)
            };

            var change = _register.CalculateChange(price, payment);

            Assert.AreEqual(2, change.Count);
            Assert.AreEqual(2, change[0].Quantity); // $4.00 in $2 denominations
            Assert.AreEqual(1, change[1].Quantity); // $0.25 in $0.25 denominations
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CalculateChange_ThrowsForInsufficientPayment()
        {
            decimal price = 30.00m;
            var payment = new List<DenominationQuantity>
            {
                new DenominationQuantity(20m, 1)
            };

            _register.CalculateChange(price, payment);
        }

        [TestMethod]
        public void CalculateChange_HandlesComplexChange()
        {
            decimal price = 47.63m;
            var payment = new List<DenominationQuantity>
            {
                new DenominationQuantity(50m, 1),
                new DenominationQuantity(5m, 1)
            };

            var change = _register.CalculateChange(price, payment);

            Assert.AreEqual(5, change.Count);
            Assert.AreEqual(1, change[0].Quantity); // $5.00
            Assert.AreEqual(1, change[1].Quantity); // $2.00
            Assert.AreEqual(1, change[2].Quantity); // $0.25
            Assert.AreEqual(1, change[3].Quantity); // $0.10
            Assert.AreEqual(2, change[4].Quantity); // $0.01
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateChange_ThrowsForNegativePrice()
        {
            var payment = new List<DenominationQuantity>
            {
                new DenominationQuantity(20m, 1)
            };

            _register.CalculateChange(-10m, payment);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DenominationQuantity_ThrowsForNegativeQuantity()
        {
            new DenominationQuantity(10m, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DenominationQuantity_ThrowsForNonPositiveDenomination()
        {
            new DenominationQuantity(0m, 1);
        }

        [TestMethod]
        public void ConfigureDenominations_SetsDenominationsCorrectly()
        {
            CurrencyConfiguration.ConfigureDenominations(new[] { 50m, 20m, 10m });
            CollectionAssert.AreEqual(new[] { 50m, 20m, 10m }, (List<decimal>)CurrencyConfiguration.Denominations);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigureDenominations_ThrowsForEmptyList()
        {
            CurrencyConfiguration.ConfigureDenominations(new decimal[0]);
        }
    }
}
