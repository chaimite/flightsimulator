using System;
using System.Collections.Generic;
using FlightBooking.Core;

namespace FlightBookingProblem
{
    class Program
    {
        private static ScheduledFlight _scheduledFlight;
        private static PassengerDataHandler _passengerDataHandler;

        static void Main(string[] args)
        {
            SetupAirlineData();
            
            string command = "";
            do
            {
                //TODO add try catch block to ensure program doesnt break
                command = Console.ReadLine() ?? "";
                var enteredText = command.ToLower().Trim();
                if (enteredText.Contains("print summary"))
                {
                    Console.WriteLine();
                    Console.WriteLine(_scheduledFlight.GetSummary());
                }
                else if (enteredText.Contains("relaxed"))
                {
                    _scheduledFlight.BusinessRule = ScheduledFlight.BusinessRuleType.Relaxed;
                }
                else if (enteredText.Contains("default"))
                {
                    _scheduledFlight.BusinessRule = ScheduledFlight.BusinessRuleType.Default;
                }
                else if (enteredText.Contains("add general"))
                {
                    _passengerDataHandler.AddPassengerData(enteredText, _scheduledFlight, PassengerType.General);
                }
                else if (enteredText.Contains("add loyalty"))
                {
                    _passengerDataHandler.AddPassengerData(enteredText, _scheduledFlight, PassengerType.LoyaltyMember);
                }
                else if (enteredText.Contains("add discounted"))
                {
                    _passengerDataHandler.AddPassengerData(enteredText, _scheduledFlight, PassengerType.Discounted);
                }
                else if (enteredText.Contains("add airline"))
                {
                    _passengerDataHandler.AddPassengerData(enteredText, _scheduledFlight, PassengerType.AirlineEmployee);
                }
                else if (enteredText.Contains("exit"))
                {
                    Environment.Exit(1);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("UNKNOWN INPUT");
                    Console.ResetColor();
                }
            } while (command != "exit");
        }

        private static void SetupAirlineData()
        {
            FlightRoute londonToParis = new FlightRoute("London", "Paris")
            {
                BaseCost = 50, 
                BasePrice = 100, 
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };
            _passengerDataHandler = new PassengerDataHandler();
            var passengerHandler = new PassengerHandler();
            var alternativePlanesHandler = new AlternativePlanesHandler();
            var flightProceedStatusHandler = new FlightProceedStatusHandler(alternativePlanesHandler);
            var summaryGenerator = new FlightSummaryGenerator(passengerHandler, flightProceedStatusHandler);
            _scheduledFlight = new ScheduledFlight(londonToParis, summaryGenerator);
            _scheduledFlight.BusinessRule = ScheduledFlight.BusinessRuleType.Default;
            // add alternative planes
            _scheduledFlight.Planes = new List<Plane>();
            _scheduledFlight.Planes.Add(new Plane { Id = 2, Name = "ATR 640", NumberOfSeats = 20 });
            _scheduledFlight.Planes.Add(new Plane { Id = 124, Name = "Bombardier Q400", NumberOfSeats = 35 });

            _scheduledFlight.SetAircraftForRoute(
                new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 12 });
            
        }
    }
}
