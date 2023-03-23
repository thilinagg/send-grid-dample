using Microsoft.AspNetCore.Mvc;
using SendGridEmailSample.Application.Commands;
using SendGridEmailSample.Application.Queries;

namespace SendGridEmailSample.Web.Controllers;

public class EmailAlertController : ApiControllerBase
{
    private readonly ILogger<EmailAlertController> _logger;

    public EmailAlertController(ILogger<EmailAlertController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> SendEmail(EmailSendProcessorCommand command)
    {
        _logger.LogInformation("SendEmail endpoint excuted");
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllEmail()
    {
        _logger.LogInformation("GetAllEmail endpoint excuted");
        var result = await Mediator.Send(new GetAllEmailQuery());
        return Ok(result);
    }

    [HttpPost]
    [Route("status-update")]
    public async Task<IActionResult> StatusUpdate(List<SendGridStatusUpdateCommand> sendGridEvents)
    {
        _logger.LogInformation("StatusUpdate no content endpoint excuted");
        foreach (var sendGridEvent in sendGridEvents)
        {
            await Mediator.Send(sendGridEvent);
        }
        return Ok();
    }
}
