namespace RetroCollectApi.Application.UseCases.UserOperations.Authenticate
{
    public class AuthenticateServiceResponseModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string ExpirationTime { get; set; }

        public AuthenticateServiceResponseModel() { }

        public AuthenticateServiceResponseModel(string Token)
        {
            this.Token = Token;
        }
        public AuthenticateServiceResponseModel(string Token, string RefreshToken)
        {
            this.Token = Token;
            this.RefreshToken = RefreshToken;
        }
    }
}
