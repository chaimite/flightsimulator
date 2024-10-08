using System;
using System.Linq;

namespace FlightBooking.Core
{
    public class FlightSummaryGenerator : IFlightSummaryGenerator
    {
        private readonly string VERTICAL_WHITE_SPACE = Environment.NewLine + Environment.NewLine;
        private readonly string NEW_LINE = Environment.NewLine;
        private const string INDENTATION = "    ";
        private readonly IPassengerHandler _passengerHandler;
        private readonly IFlightProceedStatusHandler _flightProceedStatusHandler;

        public FlightSummaryGenerator(IPassengerHandler passengerHandler, IFlightProceedStatusHandler flightProceedStatusHandler)
        {
            _passengerHandler = passengerHandler;
            _flightProceedStatusHandler = flightProceedStatusHandler;   
        }
        public string GenerateSummary(ScheduledFlight flight)
        {

            string result = "Flight summary for " + flight.FlightRoute.Title;

            var summaryData = new SummaryData();
            foreach (var passenger in flight.Passengers)
            {
                _passengerHandler.HandlePassenger(passenger, flight, summaryData);
                summaryData.CostOfFlight += flight.FlightRoute.BaseCost;
                summaryData.SeatsTaken++;
            }

            result += VERTICAL_WHITE_SPACE;

            result += "Total passengers: " + summaryData.SeatsTaken;
            result += NEW_LINE;
            result += INDENTATION + "General sales: " + flight.Passengers.Count(p => p.Type == PassengerType.General);
            result += NEW_LINE;
            result += INDENTATION + "Loyalty member sales: " + flight.Passengers.Count(p => p.Type == PassengerType.LoyaltyMember);
            result += NEW_LINE;
            result += INDENTATION + "Airline employee comps: " + flight.Passengers.Count(p => p.Type == PassengerType.AirlineEmployee);
            result += NEW_LINE;
            result += INDENTATION + "Dicounted passenger: " + flight.Passengers.Count(p => p.Type == PassengerType.Discounted);

            result += VERTICAL_WHITE_SPACE;
            result += "Total expected baggage: " + summaryData.TotalExpectedBaggage;

            result += VERTICAL_WHITE_SPACE;

            result += "Total revenue from flight: " + summaryData.ProfitFromFlight;
            result += NEW_LINE;
            result += "Total costs from flight: " + summaryData.CostOfFlight;
            result += NEW_LINE;

            double profitSurplus = summaryData.ProfitFromFlight - summaryData.CostOfFlight;
            summaryData.ProfitSurplus = profitSurplus;
            result += (profitSurplus > 0 ? "Flight generating profit of: " : "Flight losing money of: ") + profitSurplus;

            result += VERTICAL_WHITE_SPACE;

            result += "Total loyalty points given away: " + summaryData.TotalLoyaltyPointsAccrued + NEW_LINE;
            result += "Total loyalty points redeemed: " + summaryData.TotalLoyaltyPointsRedeemed + NEW_LINE;

            result += VERTICAL_WHITE_SPACE;

            result += _flightProceedStatusHandler.HandleFlightProceedStatus(flight, summaryData);
            return result;
        }
    }
}
