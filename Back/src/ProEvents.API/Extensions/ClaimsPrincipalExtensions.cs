using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProEvents.API.Extensions
{
    public static class ClaimsPrincipalExtensions //toda vez que criar metodo de extensao, a classe DEVE ser estatica para conseguir chamar o metodo
    {
        public static string GetUserName(this ClaimsPrincipal user) {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user) {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}