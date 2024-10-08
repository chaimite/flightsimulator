using FlightBooking.Core;

namespace FlightBooking.Tests
{
    public class PassengerHandlerTests
    {
        private readonly PassengerHandler _passengerHandler;
        private readonly ScheduledFlight _scheduledFlight;
        private readonly SummaryData _summaryData;

        public PassengerHandlerTests()
        {
            _passengerHandler = new PassengerHandler();

            var flightRoute = new FlightRoute("London", "Paris")
            {
                BaseCost = 50,
                BasePrice = 100,
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };

            _scheduledFlight = new ScheduledFlight(flightRoute, null);
            _summaryData = new SummaryData();
        }

        [Fact]
        public void HandlePassenger_ShouldHandleGeneralPassenger()
        {
            // Arrange
            var passenger = new Passenger { Type = PassengerType.General };

            // Act
            _passengerHandler.HandlePassenger(passenger, _scheduledFlight, _summaryData);

            // Assert
            Assert.Equal(100, _summaryData.ProfitFromFlight);
            Assert.Equal(1, _summaryData.TotalExpectedBaggage);
        }

        [Fact]
        public void HandlePassenger_ShouldHandleAirlineEmployeePassenger()
        {
            // Arrange
            var passenger = new Passenger { Type = PassengerType.AirlineEmployee };

            // Act
            _passengerHandler.HandlePassenger(passenger, _scheduledFlight, _summaryData);

            // Assert
            Assert.Equal(0, _summaryData.ProfitFromFlight);
            Assert.Equal(1, _summaryData.TotalExpectedBaggage);
        }
        [Fact]
        public void HandlePassenger_ShouldHandleLoyaltyMemberPassenger_UsingLoyaltyPoints()
        {
            // Arrange
            var passenger = new Passenger { Type = PassengerType.LoyaltyMember, LoyaltyPoints = 1000, IsUsingLoyaltyPoints = true };

            // Act
            _passengerHandler.HandlePassenger(passenger, _scheduledFlight, _summaryData);

            // Assert
            Assert.Equal(0, _summaryData.ProfitFromFlight);
            Assert.Equal(2, _summaryData.TotalExpectedBaggage);
            Assert.Equal(100, _summaryData.TotalLoyaltyPointsRedeemed);
            Assert.Equal(900, passenger.LoyaltyPoints);
        }

        [Fact]
        public void HandlePassenger_ShouldHandleLoyaltyMemberPassenger_NotUsingLoyaltyPoints()
        {
            // Arrange
            var passenger = new Passenger { Type = PassengerType.LoyaltyMember, LoyaltyPoints = 1000, IsUsingLoyaltyPoints = false };

            // Act
            _passengerHandler.HandlePassenger(passenger, _scheduledFlight, _summaryData);

            // Assert
            Assert.Equal(100, _summaryData.ProfitFromFlight);
            Assert.Equal(2, _summaryData.TotalExpectedBaggage);
            Assert.Equal(0, _summaryData.TotalLoyaltyPointsRedeemed);
            Assert.Equal(1000, passenger.LoyaltyPoints);
        }

        [Fact]
        public void HandlePassenger_ShouldHandleDiscountedPassenger()
        {
            // Arrange
            var passenger = new Passenger { Type = PassengerType.Discounted };

            // Act
            _passengerHandler.HandlePassenger(passenger, _scheduledFlight, _summaryData);

            // Assert
            Assert.Equal(50, _summaryData.CostOfFlight);
            Assert.Equal(0, _summaryData.TotalExpectedBaggage);
            Assert.Equal(0, _summaryData.TotalLoyaltyPointsRedeemed);
        }
    }
}