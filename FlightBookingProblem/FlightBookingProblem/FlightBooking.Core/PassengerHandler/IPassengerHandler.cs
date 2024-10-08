namespace FlightBooking.Core
{
    public interface IPassengerHandler
    {
        void HandlePassenger(Passenger passenger, ScheduledFlight flight, SummaryData summaryData);
    }
}
