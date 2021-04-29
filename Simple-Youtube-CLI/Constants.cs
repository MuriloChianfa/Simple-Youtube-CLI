using System.Linq;

namespace Simple_Youtube_CLI
{
    public static class Constants
    {
        public const string PersistencePath = @"Persistence\data.db";
        public const int minPasswordLenght = 3;
        public static bool In<T>(this T o, params T[] values)
        {
            if (values == null) return false;

            return values.Contains(o);
        }
    }
}
