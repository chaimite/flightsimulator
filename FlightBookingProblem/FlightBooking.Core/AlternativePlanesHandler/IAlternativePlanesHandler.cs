using System.Collections.Generic;

namespace FlightBooking.Core
{
    public interface IAlternativePlanesHandler
    {
       string GetAlternativePlanes(List<Plane> availablePlanes, int numberOfSeatsTaken);
    }
}
