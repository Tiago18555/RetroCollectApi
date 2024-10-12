using System.Security.Claims;
using Domain.Exceptions;

namespace CrossCutting;

public static class ClaimHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="givenId"></param>
    /// <returns><seelang true></seelang> if they match</returns>
    /// <exception cref="NullClaimException"></exception>
    public static bool IsTheRequestedOneId(this ClaimsPrincipal user, Guid givenId)
    {
        if (user.GetUserId() == Guid.Empty)            
            throw new NullClaimException(@"Claim 'user' cannot be null");            

        return givenId.Equals(user.GetUserId());
    }


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
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        if (!user.Claims.Any(c => c.Type.Equals("user_id"))) return Guid.Empty;
        var result = user.Claims.FirstOrDefault(c => c.Type.Equals("user_id")).Value;
        if (result.Equals(default) || result.Equals(null)) { throw new Exception($"Helper/GenericExtensionMethods.cs GetUserId: Método first retornou {result}"); }
        return Guid.Parse(result);
    }
}
