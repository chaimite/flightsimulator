using System;

namespace FlightBooking.Core
{
    public class PassengerHandler : IPassengerHandler
    {
        public void HandlePassenger(Passenger passenger, ScheduledFlight flight, SummaryData summaryData)
        {
            switch (passenger.Type)
            {
                case PassengerType.General:
                    summaryData.ProfitFromFlight += flight.FlightRoute.BasePrice;
                    summaryData.TotalExpectedBaggage++;
                    break;
                case PassengerType.LoyaltyMember:
                    if (passenger.IsUsingLoyaltyPoints)
                    {
                        int loyaltyPointsRedeemed = Convert.ToInt32(Math.Ceiling(flight.FlightRoute.BasePrice));
                        passenger.LoyaltyPoints -= loyaltyPointsRedeemed;
                        summaryData.TotalLoyaltyPointsRedeemed += loyaltyPointsRedeemed;
                    }
                    else
                    {
                        summaryData.TotalLoyaltyPointsAccrued += flight.FlightRoute.LoyaltyPointsGained;
                        summaryData.ProfitFromFlight += flight.FlightRoute.BasePrice;
                    }
                    summaryData.TotalExpectedBaggage += 2;
                    break;
                case PassengerType.AirlineEmployee:
                    summaryData.SeatsTakenByAirlineEmployees++;
                    summaryData.TotalExpectedBaggage++;
                    break;
                case PassengerType.Discounted:
                    summaryData.CostOfFlight += flight.FlightRoute.BasePrice / 2;
                    break;
            }
        }
    }
}
