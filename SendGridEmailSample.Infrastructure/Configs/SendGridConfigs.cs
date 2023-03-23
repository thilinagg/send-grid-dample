namespace SendGridEmailSample.Infrastructure.Configs;

public class SendGridConfigs
{
    public string ApiKey { get; set; } = default!;
    public string SenderEmail { get; set; } = default!;
    public string SenderName { get; set; } = default!;
    public string TemplateId { get; set; } = default!;
}
