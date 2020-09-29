namespace com.mercaderia.bono.Notificaciones.SMS
{
    public class BonusSMS
    {
        public string message { get; set; } = string.Empty;
        public string FechaEnvio { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
        public decimal balance { get; set; } = 0;
        public string user { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string bonusCode { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string key { get; set; } = string.Empty;

    }
}
