namespace FlightBooking.Core
{
    public class SummaryData
    {
        public double CostOfFlight { get; set; }
        public double ProfitFromFlight { get; set; }
        public int TotalLoyaltyPointsAccrued { get; set; }
        public int TotalLoyaltyPointsRedeemed { get; set; }
        public int TotalExpectedBaggage { get; set; }
        public int SeatsTaken { get; set; }
        public int GeneralSales { get; set; }
        public int LoyaltyMemberSales { get; set; }
        public int AirlineEmployeeComps { get; set; }
        public double ProfitSurplus { get; set; }
        public int SeatsTakenByAirlineEmployees { get; set; }
    }
}
