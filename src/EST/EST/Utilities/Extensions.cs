using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;


namespace EST.Utilities
{
    internal static class Extensions
    {
        public static Credentials ToCredentials(this LoginResult loginResult)
            => new Credentials
            {
                AccessToken = loginResult.AccessToken,
                IdentityToken = loginResult.IdentityToken,
                RefreshToken = loginResult.RefreshToken,
                AccessTokenExpiration = loginResult.AccessTokenExpiration,
            };

        public static Credentials ToCredentials(this RefreshTokenResult refreshTokenResult)
            => new Credentials
            {
                AccessToken = refreshTokenResult.AccessToken,
                IdentityToken = refreshTokenResult.IdentityToken,
                RefreshToken = refreshTokenResult.RefreshToken,
                AccessTokenExpiration = refreshTokenResult.AccessTokenExpiration
            };

        public static T DeepCopy<T>(this T self)
        {
            var serialized = JsonSerializer.Serialize(self);
            return JsonSerializer.Deserialize<T>(serialized);
        }
    }
}
