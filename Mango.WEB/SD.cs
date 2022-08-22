namespace Mango.WEB
{
    public static class SD // Static details
    {
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string CouponAPIBase { get; set; }


        public enum APIType { GET, POST, PUT, DELETE }


    }
}
