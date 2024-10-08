using System.Collections.Generic;

namespace FlightBooking.Core
{
    public class ScheduledFlight
    {
        private readonly IFlightSummaryGenerator _flightSummaryGenerator;

        public ScheduledFlight(FlightRoute flightRoute, IFlightSummaryGenerator flightSummaryGenerator)
        {
            FlightRoute = flightRoute;
            Passengers = new List<Passenger>();
            _flightSummaryGenerator = flightSummaryGenerator;
        }

        public FlightRoute FlightRoute { get; private set; }
        public Plane Aircraft { get; private set; }
        public List<Passenger> Passengers { get; private set; }

        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        public void SetAircraftForRoute(Plane aircraft)
        {
            Aircraft = aircraft;
        }
        
        public string GetSummary()
        {
            return _flightSummaryGenerator.GenerateSummary(this);
        }
        public BusinessRuleType BusinessRule { get; set; }
        public List<Plane> Planes { get; set; }


        public enum BusinessRuleType
        {
            Relaxed,
            Default
        }
    }
}
