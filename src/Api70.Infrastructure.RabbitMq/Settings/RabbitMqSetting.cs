using System.ComponentModel.DataAnnotations;

namespace Api70.Infrastructure.RabbitMq.Settings;
internal class RabbitMqSetting
{
    [Required]
    public string Host { get; set; }
    [Required]
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    [Required]
    public string VirtualHost { get; set; }
    public bool UseSsl { get; set; }
    [Range(1, 10)]
    public int RetryCount { get; set; }
    public string ClientName { get; set; }
}
