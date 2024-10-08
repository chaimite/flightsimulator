using static FlightBooking.Core.ScheduledFlight;

namespace FlightBooking.Core
{
    public interface IFlightProceedStatusHandler
    {
        string HandleFlightProceedStatus(ScheduledFlight flight, SummaryData summaryData);
    }
}
