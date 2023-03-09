namespace NoKates.Identity.Extensions
{
    public static class StringExtensions
    {
        public static string ToNormalFormat(this string value)
            => value.Trim().ToLower();
    }
}
