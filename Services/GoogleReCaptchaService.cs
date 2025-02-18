namespace Pizza_Star.Services
{
    public class GoogleReCaptchaService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleReCaptchaService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyAsync(string token)
        {
            var secret = _configuration["GoogleReCaptcha:SecretKey"];
            var response = await _httpClient.GetStringAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}");

            return response.Contains("\"success\": true");
        }
    }
}
