namespace Mango.Web.Utility
{
    public static class SD
    {
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public static HttpMethod ToMethod(this ApiType type)
        {
            switch (type)
            {
                case ApiType.GET:
                    return HttpMethod.Get;
                case ApiType.POST:
                    return HttpMethod.Post;
                case ApiType.PUT:
                    return HttpMethod.Put;
                case ApiType.DELETE:
                    return HttpMethod.Delete;
            }
            return HttpMethod.Get;
        }
    }
}
