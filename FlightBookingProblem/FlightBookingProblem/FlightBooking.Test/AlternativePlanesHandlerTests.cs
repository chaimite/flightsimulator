using FlightBooking.Core;

namespace FlightBooking.Tests
{
    public class AlternativePlanesHandlerTests
    {
        private readonly AlternativePlanesHandler _alternativePlanesHandler;

        public AlternativePlanesHandlerTests()
        {
            _alternativePlanesHandler = new AlternativePlanesHandler();
        }

        [Fact]
        public void GetAlternativePlanes_ShouldReturnEmptyString_WhenNoSuitablePlanes()
        {
            // Arrange
            var availablePlanes = new List<Plane>
            {
                new Plane { Name = "Plane A", NumberOfSeats = 10 },
                new Plane { Name = "Plane B", NumberOfSeats = 15 }
            };
            int numberOfSeatsTaken = 20;

            // Act
            var result = _alternativePlanesHandler.GetAlternativePlanes(availablePlanes, numberOfSeatsTaken);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetAlternativePlanes_ShouldReturnSuitablePlanes_WhenThereAreSuitablePlanes()
        {
            // Arrange
            var availablePlanes = new List<Plane>
            {
                new Plane { Name = "Plane A", NumberOfSeats = 10 },
                new Plane { Name = "Plane B", NumberOfSeats = 25 },
                new Plane { Name = "Plane C", NumberOfSeats = 30 }
            };
            int numberOfSeatsTaken = 20;

            // Act
            var result = _alternativePlanesHandler.GetAlternativePlanes(availablePlanes, numberOfSeatsTaken);

            // Assert
            var expectedOutput = "Other more suitable aircrafts are: Plane B could handle this flight. Plane C could handle this flight. ";
            Assert.Equal(expectedOutput, result);
        }
    }
}