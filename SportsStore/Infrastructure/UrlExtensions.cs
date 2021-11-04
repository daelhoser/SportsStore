//namespace SportsStore.Infrastructure
//{
//    public static class UrlExtensions
//    {
//        public static string PathAndQuery(this HttpRequest request) => request.QueryString.HasValue ? $"{request.QueryString}{request.QueryString}" : request.PathAndQuery().ToString();
//    }
//}

using Microsoft.AspNetCore.Http;

namespace SportsStore.Infrastructure
{
    public static class UrlExtensions
    {
        public static string PathAndQuery(this HttpRequest request) =>
            request.QueryString.HasValue
                ? $"{request.Path}{request.QueryString}"
                : request.Path.ToString();
    }
}