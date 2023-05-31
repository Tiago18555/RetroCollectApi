using System.Security.Claims;

namespace RetroCollectApi.CrossCutting
{
    public static class ClaimHelper
    {
        /// <summary>
        /// Check if user_id of Claims and other user_id match
        /// </summary>
        /// <param name="user"></param>
        /// <param name="givenId"></param>
        /// <returns><seelang true></seelang> if they match</returns>
        public static bool IsTheRequestedOneId(this ClaimsPrincipal user, Guid givenId) =>
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
