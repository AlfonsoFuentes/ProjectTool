using Shared.Models.UserAccounts.Reponses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Infrastructure.Generics
{
    public static class Generics
    {
        public static ClaimsPrincipal SetClaimPrincipal(LoginUserResponse model)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, model.Id!),
                    new(ClaimTypes.Name, model.Email!),
                    new(ClaimTypes.Email, model.Email!),
                    new(ClaimTypes.Role, model.Role!),
                }, "JwtAuth"));
        }

        public static LoginUserResponse GetClaimsFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var claims = token.Claims;

            string Id = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;
            string Name = claims.First(c => c.Type == ClaimTypes.Name).Value!;
            string Email = claims.First(c => c.Type == ClaimTypes.Email).Value!;
            string Role = claims.First(c => c.Type == ClaimTypes.Role).Value!;
            LoginUserResponse result = new()
            {
                Id = Id,
                Email = Email,
                Role = Role,


            };
            return result;
        }

        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }
        public static string SerializeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;

        public static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, Encoding.UTF8, "application/json");
    }
}
