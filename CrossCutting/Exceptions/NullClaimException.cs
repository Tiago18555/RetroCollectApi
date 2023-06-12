namespace RetroCollectApi.CrossCutting
{
    public class NullClaimException : Exception
    {
        public NullClaimException(string message) : base(message)
        {

        }
    }

}
