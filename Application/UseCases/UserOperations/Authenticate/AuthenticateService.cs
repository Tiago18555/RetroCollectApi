using Microsoft.IdentityModel.Tokens;
using RetroCollect.Models;
using RetroCollectApi.CrossCutting;
using RetroCollectApi.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            if (!string.IsNullOrEmpty(jwtResponse.ErrorMsg))
                return GenericResponses.Forbidden(jwtResponse.ErrorMsg);

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

        /// <exception cref="AuthException"></exception>
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

                //UNCOMMENT THIS LATER
                //if (user.VerifiedAt == DateTime.MinValue) return new AuthenticateServiceResponseModel(true, "Please verify this user on your registered email.");

                return new AuthenticateServiceResponseModel(GenerateToken(user), DateTime.Now.AddDays(7));


            }
            catch (InvalidOperationException)
            {
                return new AuthenticateServiceResponseModel();
            }
        }


        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="EncoderFallbackException"></exception>
        internal string GenerateToken(User user)
        {
            var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var credenciais = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("user_id", user.UserId.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                expires: DateTime.Now.AddDays(7),
                audience: configuration["Jwt:ValidAudience"],
                signingCredentials: credenciais,                
                claims: claims                
            );

            System.Console.WriteLine(new JwtSecurityTokenHandler().WriteToken(jwt));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
