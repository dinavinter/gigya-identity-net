namespace IdentityServer
{
    public static class Constants
    {
        public static class EndPoints
        {
            public const string Token = "token";
            public const string Jwks = "jwks";
            public const string Authorization = "authorization";
            public const string Registration = "register";
            public const string OAuthConfiguration = ".well-known/oauth-authorization-server";
            public const string Form = "form";
            public const string Management = "management";
            public const string MtlsPrefix = "mtls";
            public const string MtlsToken = MtlsPrefix + "/" + Token;
        }
    }
}