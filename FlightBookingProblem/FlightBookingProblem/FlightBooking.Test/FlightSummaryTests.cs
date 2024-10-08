using FlightBooking.Core;
using Moq;

namespace FlightBooking.Tests
{
    public class FlightSummaryGeneratorTests : IDisposable
    {
        private readonly Mock<PassengerHandler> _mockPassengerHandler;
        private readonly AlternativePlanesHandler _mockAlternativePlanesHandler;
        private readonly FlightProceedStatusHandler _mockFlightProceedStatusHandler;
        private readonly FlightSummaryGenerator _flightSummaryGenerator;
        private ScheduledFlight _scheduledFlight;

        public FlightSummaryGeneratorTests()
        {
            _mockPassengerHandler = new Mock<PassengerHandler>();
            _mockAlternativePlanesHandler = new AlternativePlanesHandler(); ;
            _mockFlightProceedStatusHandler = new FlightProceedStatusHandler(_mockAlternativePlanesHandler);
            _flightSummaryGenerator = new FlightSummaryGenerator(_mockPassengerHandler.Object, _mockFlightProceedStatusHandler);
            InitializeScheduledFlight();
        }
        private void InitializeScheduledFlight()
        {
            var flightRoute = new FlightRoute("London", "Paris")
            {
                BaseCost = 50,
                BasePrice = 100,
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };

            _scheduledFlight = new ScheduledFlight(flightRoute, _flightSummaryGenerator);
            _scheduledFlight.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 12 });
        }

        public void Dispose()
        {
            InitializeScheduledFlight();
        }

        [Fact]
        public void GenerateSummary_ShouldReturnCorrectSummary()
        {
            // Arrange
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Steve", Age = 30 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Mark", Age = 12 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "James", Age = 36 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Jane", Age = 32 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "John", Age = 29, LoyaltyPoints = 1000, IsUsingLoyaltyPoints = true });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Sarah", Age = 45, LoyaltyPoints = 1250, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Jack", Age = 60, LoyaltyPoints = 50, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Trevor", Age = 47 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Alan", Age = 34 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Suzy", Age = 21 });

            // Act
            var summary = _flightSummaryGenerator.GenerateSummary(_scheduledFlight);

            // Assert
            var expectedOutput = "Flight summary for London to Paris\r\n\r\n" +
                                 "Total passengers: 10\r\n" +
                                 "    General sales: 6\r\n" +
                                 "    Loyalty member sales: 3\r\n" +
                                 "    Airline employee comps: 1\r\n" +
                                 "    Dicounted passenger: 0\r\n\r\n" +
                                 "Total expected baggage: 13\r\n\r\n" +
                                 "Total revenue from flight: 800\r\n" +
                                 "Total costs from flight: 500\r\n" +
                                 "Flight generating profit of: 300\r\n\r\n" +
                                 "Total loyalty points given away: 10\r\n" +
                                 "Total loyalty points redeemed: 100\r\n\r\n\r\n" +
                                 "THIS FLIGHT MAY PROCEED";

            Assert.Equal(expectedOutput, summary);
        }
        [Fact]
        public void GenerateSummary_ShouldReturnCorrectSummaryForDiscounted()
        {
            // Arrange
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.Discounted, Name = "Steve", Age = 30 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Mark", Age = 12 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "James", Age = 36 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Jane", Age = 32 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "John", Age = 29, LoyaltyPoints = 1000, IsUsingLoyaltyPoints = true });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Sarah", Age = 45, LoyaltyPoints = 1250, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Jack", Age = 60, LoyaltyPoints = 50, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Trevor", Age = 47 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Alan", Age = 34 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Suzy", Age = 21 });

            // Act
            var summary = _flightSummaryGenerator.GenerateSummary(_scheduledFlight);

            // Assert
            var expectedOutput = "Flight summary for London to Paris\r\n\r\n" +
                                 "Total passengers: 10\r\n" +
                                 "    General sales: 5\r\n" +
                                 "    Loyalty member sales: 3\r\n" +
                                 "    Airline employee comps: 1\r\n" +
                                 "    Dicounted passenger: 1\r\n\r\n" +
                                 "Total expected baggage: 12\r\n\r\n" +
                                 "Total revenue from flight: 700\r\n" +
                                 "Total costs from flight: 550\r\n" +
                                 "Flight generating profit of: 150\r\n\r\n" +
                                 "Total loyalty points given away: 10\r\n" +
                                 "Total loyalty points redeemed: 100\r\n\r\n\r\n" +
                                 "THIS FLIGHT MAY PROCEED";

            Assert.Equal(expectedOutput, summary);
        }
        [Fact]
        public void GenerateSummary_ShouldReturnCorrectSummary_WhenCapacityNotEnoughDoNotProceed()
        {
            // Arrange
            _scheduledFlight.Planes = new List<Plane>();
            _scheduledFlight.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 5 });

            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.Discounted, Name = "Steve", Age = 30 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Mark", Age = 12 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "James", Age = 36 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Jane", Age = 32 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "John", Age = 29, LoyaltyPoints = 1000, IsUsingLoyaltyPoints = true });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Sarah", Age = 45, LoyaltyPoints = 1250, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Jack", Age = 60, LoyaltyPoints = 50, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Trevor", Age = 47 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Alan", Age = 34 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Suzy", Age = 21 });

            // Act
            var summary = _flightSummaryGenerator.GenerateSummary(_scheduledFlight);

            // Assert
            var expectedOutput = "Flight summary for London to Paris\r\n\r\n" +
                                    "Total passengers: 10\r\n" +
                                    "    General sales: 5\r\n" +
                                    "    Loyalty member sales: 3\r\n" +
                                    "    Airline employee comps: 1\r\n" +
                                    "    Dicounted passenger: 1\r\n\r\n" +
                                    "Total expected baggage: 12\r\n\r\n" +
                                    "Total revenue from flight: 700\r\n" +
                                    "Total costs from flight: 550\r\n" +
                                    "Flight generating profit of: 150\r\n\r\n" +
                                    "Total loyalty points given away: 10\r\n" +
                                    "Total loyalty points redeemed: 100\r\n\r\n\r\n" +
                                    "FLIGHT MAY NOT PROCEED\n";

            Assert.Equal(expectedOutput, summary);
        }
        [Fact]
        public void GenerateSummary_ShouldReturnCorrectSummary_WhenCapacityNotEnough_ShowAlternative()
        {
            // Arrange
            _scheduledFlight.Planes = new List<Plane>();
            _scheduledFlight.Planes.Add(new Plane { Id = 2, Name = "ATR 640", NumberOfSeats = 20 });
            _scheduledFlight.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 5 });

            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.Discounted, Name = "Steve", Age = 30 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Mark", Age = 12 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "James", Age = 36 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Jane", Age = 32 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "John", Age = 29, LoyaltyPoints = 1000, IsUsingLoyaltyPoints = true });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Sarah", Age = 45, LoyaltyPoints = 1250, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Jack", Age = 60, LoyaltyPoints = 50, IsUsingLoyaltyPoints = false });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Trevor", Age = 47 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Alan", Age = 34 });
            _scheduledFlight.AddPassenger(new Passenger { Type = PassengerType.General, Name = "Suzy", Age = 21 });

            // Act
            var summary = _flightSummaryGenerator.GenerateSummary(_scheduledFlight);

            // Assert
            var expectedOutput = "Flight summary for London to Paris\r\n\r\n" +
                                    "Total passengers: 10\r\n" +
                                    "    General sales: 5\r\n" +
                                    "    Loyalty member sales: 3\r\n" +
                                    "    Airline employee comps: 1\r\n" +
                                    "    Dicounted passenger: 1\r\n\r\n" +
                                    "Total expected baggage: 12\r\n\r\n" +
                                    "Total revenue from flight: 700\r\n" +
                                    "Total costs from flight: 550\r\n" +
                                    "Flight generating profit of: 150\r\n\r\n" +
                                    "Total loyalty points given away: 10\r\n" +
                                    "Total loyalty points redeemed: 100\r\n\r\n\r\n" +
                                    "FLIGHT MAY NOT PROCEED\n" +
                                    "Other more suitable aircrafts are: ATR 640 could handle this flight. ";
            Assert.Equal(expectedOutput, summary);
        }
    }
}