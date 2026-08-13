namespace Credit_Wallet.Services
{
    public static  class DateTimeHelper
    {
        public static DateTime NormalizeToMilliseconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Year,
                                dateTime.Month,
                                dateTime.Day,
                                dateTime.Hour,
                                dateTime.Minute,
                                dateTime.Second,
                                dateTime.Millisecond,
                                DateTimeKind.Utc);
        }
    }
}
