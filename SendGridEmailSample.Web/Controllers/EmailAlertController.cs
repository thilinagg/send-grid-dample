using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SendGridEmailSample.Application.Commands;
using SendGridEmailSample.Application.Queries;
using SendGridEmailSample.Web.Hubs;

namespace SendGridEmailSample.Web.Controllers;

public class EmailAlertController : ApiControllerBase
{
    private readonly ILogger<EmailAlertController> _logger;
    private readonly IHubContext<EmailStatusChangeEventHub> _hub;

    public EmailAlertController(ILogger<EmailAlertController> logger, IHubContext<EmailStatusChangeEventHub> hub)
    {
        _logger = logger;
        _hub = hub;
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
            var response = await Mediator.Send(sendGridEvent);
            if (response is not null)
                await _hub.Clients.All.SendAsync("StatusUpdated", response);
        }
        return Ok();
    }

    [HttpGet]
    [Route("hub")]
    public async Task<IActionResult> HubAll()
    {
        await _hub.Clients.All.SendAsync("StatusUpdated", new { Id= "C418FE82-E076-4519-550C-08DB2C07C03E".ToLower(), Status = 3});
        return Ok();
    }
}
