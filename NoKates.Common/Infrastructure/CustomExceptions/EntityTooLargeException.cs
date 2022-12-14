namespace NoKates.Common.Infrastructure.CustomExceptions
{
    public class EntityTooLargeException : RequestException
    {
        public EntityTooLargeException(string message) : base(413, message) { }
    }
}
