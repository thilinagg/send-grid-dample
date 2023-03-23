using System.ComponentModel;

namespace SendGridEmailSample.Domain.Enums;

public enum EmailStatus
{
    [Description("failed")]
    Failed = 0,
    [Description("processed")]
    Processed = 1,
    [Description("delivered")]
    Delivered = 2,
    [Description("open")]
    Open = 3,
    [Description("click")]
    Click = 4,
}
