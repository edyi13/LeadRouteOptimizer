namespace LeadRouteOptimizer.Domain.Services
{
    public static class Haversine
    {
        /// <summary>
        /// calculates the distance in kilometers between two geographic coordinates
        /// harvesine formula works on a sphere, so the result is an approximation
        /// https://en.wikipedia.org/wiki/Haversine_formula
        /// </summary>
        /// returns>The distance in kilometers</returns>
        public static decimal DistanceKm(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const double R = 6371.0; // planet earth radius km

            static double ToRad(decimal deg) => (double)deg * Math.PI / 180.0;

            var dLat = ToRad(lat2 - lat1);
            var dLon = ToRad(lon2 - lon1);

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var km = R * c;

            return (decimal)km;
        }
    }
}
