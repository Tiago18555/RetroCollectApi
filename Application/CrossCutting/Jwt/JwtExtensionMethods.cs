using System.Security.Claims;

namespace Application.CrossCutting.Jwt
{
    public static class JwtExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="givenId"></param>
        /// <returns></returns>
        public static bool IsTheRequestedOneId(this ClaimsPrincipal user, long givenId) =>
            givenId.ToString().Equals(user.GetUserId());


        /// <summary>
        /// Verifica se a função  da token corresponde, a função fornecida
        /// </summary>
        /// <param name="user">Claims do usuário</param>
        /// <param name="role">Nome da role para comparação</param>
        /// <returns></returns>
        public static bool HasRole(this ClaimsPrincipal user, string role)
        {
            Claim userClaim = user.Claims.First(c => c.Type == ClaimTypes.Role);
            return userClaim.Value.Equals(role);
        }

        /// <summary>
        /// Retorna o ID de user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static long GetUserId(this ClaimsPrincipal user)
        {
            var result = user.Claims.First(c => c.Type.Equals("Id")).Value;
            if (result.Equals(null)) { throw new Exception($"Helper/GenericExtensionMethods.cs GetUserId: Método first retornou {result}"); }
            return long.Parse(result);
        }
    }
}
