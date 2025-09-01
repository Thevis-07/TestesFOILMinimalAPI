namespace TestesFOILMinimalApi.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public bool UseStartTls { get; set; } = true;
        public string FromName { get; set; } = "Sua Marca";
        public string FromEmail { get; set; } = "no-reply@seudominio.com";
    }
}
