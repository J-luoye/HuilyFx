using System;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var keySecret = new ApiAuthen.KeySecret.KeyAndSecret();

            Console.WriteLine(keySecret.aa());

            var t = (long)1557456300 * 10000000;

            DateTime t2 = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse("1970-1-1 00:00:00")).Add(new TimeSpan(t));

            Console.WriteLine(t2);

            Console.ReadLine();



        }
    }
}
