using CrossCutting;
using CrossCutting.Providers;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Application.UseCases.UserOperations.Authenticate
{
    public class AuthenticateUsecase : IAuthenticateUsecase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IDateTimeProvider _timeProvider;
        public AuthenticateUsecase(IUserRepository _repository, IConfiguration configuration, IDateTimeProvider dateTimeProvider)
        {
            this._repository = _repository;
            this._configuration = configuration;
            this._timeProvider = dateTimeProvider;
        }

        public ResponseModel Login(AuthenticateServiceRequestModel credentials)
        {
            AuthenticateServiceResponseModel jwtResponse = Authenticate(credentials);

            if (!string.IsNullOrEmpty(jwtResponse.ErrorMsg))
                return ResponseFactory.Forbidden(jwtResponse.ErrorMsg);

            return string.IsNullOrEmpty(jwtResponse.Token) ?
                ResponseFactory.Unauthorized("Email ou senha incorretos.") : jwtResponse.Ok();            
        }

        public ResponseModel ValidateJwtToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return ResponseFactory.Unauthorized();

            token = token.Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
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
                return ResponseFactory.Unauthorized();
            }
        }

        /// <exception cref="AuthException"></exception>
        internal AuthenticateServiceResponseModel Authenticate(AuthenticateServiceRequestModel authenticateServiceRequestModel)
        {
            try
            {
                User user;

                user = _repository.SingleOrDefault(x => x.Username == authenticateServiceRequestModel.Username);

                if (user == null || !BCryptNet.Verify(authenticateServiceRequestModel.Password, user.Password))
                {
                    return new AuthenticateServiceResponseModel();
                }

                if (user.VerifiedAt == DateTime.MinValue) return new AuthenticateServiceResponseModel(true, "Please verify this user on your registered email.");

                return new AuthenticateServiceResponseModel(GenerateToken(user), _timeProvider.UtcNow.AddDays(7));


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
            var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credenciais = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("user_id", user.UserId.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                expires: _timeProvider.UtcNow.AddDays(7),
                audience: _configuration["Jwt:ValidAudience"],
                signingCredentials: credenciais,                
                claims: claims                
            );

            System.Console.WriteLine(new JwtSecurityTokenHandler().WriteToken(jwt));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
