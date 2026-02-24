using System;
using System.Collections.Generic;
using System.Text;
using static Android.Provider.Telephony.Mms;

namespace calcDistance.Calculations
{
    //  A static class that contains methods for calculating the distance
    //  between two geographical points using the Haversine formula.
    internal class Calculation
    {
        //  Converts degrees to radians.
        static public double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
        //  Calculates the distance in kilometers between two points specified
        //  by their latitude and longitude.
        static public double CalculateDistance(
            double lat1, double lng1,
            double lat2, double lng2)
        {
            double rLat1 = ToRadians(lat1);
            double rLat2 = ToRadians(lat2);
            double rLon1 = ToRadians(lng1);
            double rLon2 = ToRadians(lng2);
        
            double distance = 6371 * Math.Acos(
            Math.Cos(rLat1) *
            Math.Cos(rLat2) *
            Math.Cos(rLon2 - rLon1) +
            Math.Sin(rLat1) *
            Math.Sin(rLat2)
            );

        return distance;
        }

        static public double CalculateFuelConsumption(double distance, double consumption)
        {
            return (distance / 100) * consumption;
        }
    }
}
