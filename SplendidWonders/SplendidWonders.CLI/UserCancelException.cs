namespace SplendidWonders.CLI
{
    [Serializable]
    internal class UserCancelException : Exception
    {
        public UserCancelException()
        {
        }

        public UserCancelException(string? message) : base(message)
        {
        }

        public UserCancelException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}