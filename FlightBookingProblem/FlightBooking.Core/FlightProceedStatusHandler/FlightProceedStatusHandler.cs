using static FlightBooking.Core.ScheduledFlight;

namespace FlightBooking.Core
{
    public class FlightProceedStatusHandler : IFlightProceedStatusHandler
    {
        private readonly AlternativePlanesHandler _alternativePlanesHandler;
        public FlightProceedStatusHandler(AlternativePlanesHandler alternativePlanesHandler)
        {
            _alternativePlanesHandler = alternativePlanesHandler;
        }
        public string HandleFlightProceedStatus(ScheduledFlight flight, SummaryData summaryData)
        {
            string result;
            if (flight.BusinessRule == BusinessRuleType.Relaxed)
            {
                if (IsRevenueGeneratedExceedingCost(summaryData.SeatsTakenByAirlineEmployees, flight.FlightRoute.MinimumTakeOffPercentage) &&
                    IsAircraftCapacityExceeded(summaryData.SeatsTaken, flight.Aircraft.NumberOfSeats))
                {
                    result =  "THIS FLIGHT MAY PROCEED";
                }
                else
                {
                    result = "FLIGHT MAY NOT PROCEED";
                }
            }
            else
            {
                if (IsFlightProfitable(summaryData.ProfitSurplus) &&
                    IsAircraftCapacityExceeded(summaryData.SeatsTaken, flight.Aircraft.NumberOfSeats) &&
                    IsMinimumTakeOffPercentageAchieved(summaryData.SeatsTaken, flight.Aircraft.NumberOfSeats, flight.FlightRoute.MinimumTakeOffPercentage))
                {
                    result = "THIS FLIGHT MAY PROCEED";
                }
                else
                {
                    result = "FLIGHT MAY NOT PROCEED";
                }
            }
            if (summaryData.SeatsTaken > flight.Aircraft.NumberOfSeats)
            {
                result += "\n";
                result += _alternativePlanesHandler.GetAlternativePlanes(flight.Planes, summaryData.SeatsTaken);
            }
            return result;
        }
        private bool IsAircraftCapacityExceeded(int seatsTakenInFlight, int aircraftCapacity)
        {
            return seatsTakenInFlight < aircraftCapacity;
        }

        private bool IsFlightProfitable(double profitSurplus)
        {
            return profitSurplus > 0;
        }

        private bool IsMinimumTakeOffPercentageAchieved (int seatsTaken, int aircraftNumberOfSeats,double minimumTakeOffPercentage)
        {
            return seatsTaken / (double)aircraftNumberOfSeats > minimumTakeOffPercentage;
        }

        private bool IsRevenueGeneratedExceedingCost(int numberOfAirlineEmployees, double minimumPercentageOfPassengersRequired)
        {
            return numberOfAirlineEmployees > minimumPercentageOfPassengersRequired;
        }
    }
}
