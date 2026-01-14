namespace Infrastructure;

public static class GlobalConstants
{
    public static class Coordinates
    {
        public const double Company_Lat = 32.072059916717016;
        public const double Company_Lon = 34.82851580681851;
    }

    public static class deliveryMethodMaxDistanceKm
    {
        public const int FootMinDistanceKm = 1; //1 - 2
        public const int FootMaxDistanceKm = 2;

        public const int BicycleMinDistanceKm = 2; //2 - 5
        public const int BicycleMaxDistanceKm = 5;

        public const int MotorCycleMinDistanceKm = 3; //3 - 10
        public const int MotorCycleMaxDistanceKm = 10;

        public const int CarMinDistanceKm = 5; //5 - 25
        public const int CarMaxDistanceKm = 25;
    }

    public static class AverageSpeedKmH
    {
        public const double WalkingSpeedKmH = 5;
        public const double BicycleSpeedKmH = 15;
        public const double MotorCycleSpeedKmH = 40;
        public const double CarSpeedKmH = 60;
    }

}
