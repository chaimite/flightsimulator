using System;
using System.Collections.Generic;
using System.Text;

namespace FlightBooking.Core
{
    public class AlternativePlanesHandler : IAlternativePlanesHandler
    {
        public string GetAlternativePlanes(List<Plane> availablePlanes, int numberOfSeatsTaken)
        {
            var stringBuilder = new StringBuilder();
            var flag = false;
            stringBuilder.Append("Other more suitable aircrafts are: ");
            foreach (Plane plane in availablePlanes)
            {
                if (plane.NumberOfSeats >= numberOfSeatsTaken)
                {
                    flag = true;
                    stringBuilder.Append(plane.Name + " could handle this flight. ");
                }
            }
            return !flag ? String.Empty : stringBuilder.ToString();
        }
    }
}
