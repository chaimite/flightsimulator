using FlightBooking.Core;

namespace FlightBooking.Tests
{
    public class FlightProceedStatusHandlerTests
    {
        private readonly FlightProceedStatusHandler _flightProceedStatusHandler;
        private readonly ScheduledFlight _scheduledFlight;
        private readonly SummaryData _summaryData;

        public FlightProceedStatusHandlerTests()
        {
            var alternativePlanesHandler = new AlternativePlanesHandler();
            _flightProceedStatusHandler = new FlightProceedStatusHandler(alternativePlanesHandler);

            var flightRoute = new FlightRoute("London", "Paris")
            {
                BaseCost = 50,
                BasePrice = 100,
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };

            _scheduledFlight = new ScheduledFlight(flightRoute, null);
            _scheduledFlight.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 12 });

            _summaryData = new SummaryData();
        }

        [Fact]
        public void HandleFlightProceedStatus_ShouldReturnFlightMayProceed_WhenBusinessRuleIsRelaxedAndConditionsMet()
        {
            // Arrange
            _scheduledFlight.BusinessRule = ScheduledFlight.BusinessRuleType.Relaxed;
            _summaryData.SeatsTakenByAirlineEmployees = 10;
            _summaryData.SeatsTaken = 10;

            // Act
            var result = _flightProceedStatusHandler.HandleFlightProceedStatus(_scheduledFlight, _summaryData);

            // Assert
            Assert.Equal("THIS FLIGHT MAY PROCEED", result);
        }

        [Fact]
        public void HandleFlightProceedStatus_ShouldReturnFlightMayProceed_WhenBusinessRuleIsStrictAndConditionsMet()
        {
            // Arrange
            _scheduledFlight.BusinessRule = ScheduledFlight.BusinessRuleType.Default;
            _summaryData.ProfitSurplus = 100;
            _summaryData.SeatsTaken = 10;

            // Act
            var result = _flightProceedStatusHandler.HandleFlightProceedStatus(_scheduledFlight, _summaryData);

            // Assert
            Assert.Equal("THIS FLIGHT MAY PROCEED", result);
        }

        [Fact]
        public void HandleFlightProceedStatus_ShouldReturnFlightMayNotProceed_WhenBusinessRuleIsStrictAndConditionsNotMet()
        {
            // Arrange
            _scheduledFlight.BusinessRule = ScheduledFlight.BusinessRuleType.Default;
            _summaryData.ProfitSurplus = -100;
            _summaryData.SeatsTaken = 5;

            // Act
            var result = _flightProceedStatusHandler.HandleFlightProceedStatus(_scheduledFlight, _summaryData);

            // Assert
            Assert.Equal("FLIGHT MAY NOT PROCEED", result);
        }
    }
}