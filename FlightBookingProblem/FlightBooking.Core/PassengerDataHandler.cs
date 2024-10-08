using System;

namespace FlightBooking.Core
{
    public class PassengerDataHandler
    {
        public void AddPassengerData(string enteredText, ScheduledFlight scheduledFlight, PassengerType passengerType)
        {
            string[] passengerSegments = enteredText.Split(' ');
            if (passengerSegments.Length < 4)
                return;

            var newPassenger = new Passenger
            {
                Type = passengerType,
                Name = passengerSegments[2],
                Age = Convert.ToInt32(passengerSegments[3])
            };

            if (passengerType == PassengerType.General && passengerSegments.Length == 4)
            {
                scheduledFlight.AddPassenger(newPassenger);
            }
            else if (passengerType == PassengerType.LoyaltyMember && passengerSegments.Length == 6)
            {
                newPassenger.LoyaltyPoints = Convert.ToInt32(passengerSegments[4]);
                newPassenger.IsUsingLoyaltyPoints = Convert.ToBoolean(passengerSegments[5]);
                scheduledFlight.AddPassenger(newPassenger);
            }
            else if (passengerType == PassengerType.AirlineEmployee && passengerSegments.Length == 4)
            {
                scheduledFlight.AddPassenger(newPassenger);
            }
            else if (passengerType == PassengerType.Discounted && passengerSegments.Length == 4)
            {
                scheduledFlight.AddPassenger(newPassenger);
            }
        }
    }
}
