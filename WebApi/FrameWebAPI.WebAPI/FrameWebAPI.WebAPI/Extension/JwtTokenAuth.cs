namespace FrameWebAPI.WebAPI.Extension
{
    public class JwtTokenAuth
    {
        private readonly RequestDelegate _next;

        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        private void PreProceed(HttpContext next)
        {
        }

        private void PostProceed(HttpContext next)
        {
        }

        public Task Invoke(HttpContext httpContext)
        {
            PreProceed(httpContext);
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                PostProceed(httpContext);
                return _next(httpContext);
            }
            string text = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            try
            {
                _ = text.Length;
                _ = 128;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} middleware wrong:{ex.Message}");
            }
            PostProceed(httpContext);
            return _next(httpContext);
        }
    }
}
