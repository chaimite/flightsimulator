namespace FlightBooking.Core
{
    public interface IFlightSummaryGenerator
    {
        string GenerateSummary(ScheduledFlight flight);
    }
}
