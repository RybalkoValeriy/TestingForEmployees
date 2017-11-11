using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TestingForEmployees.Util
{

    public static class GetValueOfClaimsUsers
    {
        // Метод для получения значения клайма по имени
        public static string GetValue(IEnumerable<Claim> claimCollection, string findTypeName)
        {
            if (claimCollection != null && !string.IsNullOrEmpty(findTypeName))
            {
                try
                {
                    var find = claimCollection.First(x => x.Type == findTypeName);
                    return find.Value;
                }
                catch (Exception)
                {

                    return "";
                }
            }
            return "";
        }
    }
}
