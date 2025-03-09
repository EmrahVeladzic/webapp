namespace backend.Requests
{
    public class LogInRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public LogInRequest()
        {
            this.Username = "";
            this.Password = "";
        }

    }

    public class SignUpRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Language { get; set; }
        public bool SharedAssets { get; set; }

    }



}
