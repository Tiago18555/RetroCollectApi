namespace Application.UseCases.UserOperations.Authenticate
{
    public class AuthenticateServiceResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string ErrorMsg { get; set; }

        public AuthenticateServiceResponseModel() { }

        public AuthenticateServiceResponseModel(string Token)
        {
            this.Token = Token;
        }

        public AuthenticateServiceResponseModel(bool success, string Err)
        {
            this.ErrorMsg = Err;
        }

        public AuthenticateServiceResponseModel(string Token, DateTime expiration)
        {
            this.Token = Token;
            this.ExpirationTime = expiration;
        }

        public AuthenticateServiceResponseModel(string Token, string RefreshToken)
        {
            this.Token = Token;
            this.RefreshToken = RefreshToken;
        }
    }
}
