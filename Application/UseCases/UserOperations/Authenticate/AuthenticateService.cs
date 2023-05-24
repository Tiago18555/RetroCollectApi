using Microsoft.IdentityModel.Tokens;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

namespace RetroCollectApi.Application.UseCases.UserOperations.Authenticate
{
    public class AuthenticateService : IAuthenticateService
    {
        private IConfiguration configuration { get; set; }
        private IUserRepository repository { get; set; }
        public AuthenticateService(IUserRepository _repository, IConfiguration configuration)
        {
            this.repository = _repository;
            this.configuration = configuration;
        }

        public ResponseModel Login(AuthenticateServiceRequestModel credentials)
        {
            AuthenticateServiceResponseModel jwtResponse = Authenticate(credentials);

            return jwtResponse.Token.IsNullOrEmpty() ?
                GenericResponses.Unauthorized("Email ou senha incorretos.") : jwtResponse.Ok();
        }

        public ResponseModel ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return GenericResponses.Unauthorized();

            token = token.Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                DateTime tokenExpiration = jwtToken.ValidTo.AddHours(-3);

                return token.Authenticated($"JWT token válido até {tokenExpiration}");
            }
            catch
            {
                return GenericResponses.Unauthorized();
            }
        }

        internal AuthenticateServiceResponseModel Authenticate(AuthenticateServiceRequestModel authenticateServiceRequestModel)
        {
            try
            {
                User user;

                user = repository.SingleOrDefault(x => x.Username == authenticateServiceRequestModel.Username);

                if (user == null || !BCryptNet.Verify(authenticateServiceRequestModel.Password, user.Password))
                {
                    return new AuthenticateServiceResponseModel();
                }
                return new AuthenticateServiceResponseModel(GenerateToken(user), DateTime.Now.AddDays(7));


            }
            catch (InvalidOperationException)
            {
                return new AuthenticateServiceResponseModel();
            }
        }



        internal string GenerateToken(User user)
        {
            var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var credenciais = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);


            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                expires: DateTime.Now.AddDays(7),
                audience: configuration["Jwt:ValidAudience"],
                signingCredentials: credenciais
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
