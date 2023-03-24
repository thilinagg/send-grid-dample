using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SendGridEmailSample.Application.Commands.Send;
using SendGridEmailSample.Application.Commands.UpdateStatus;
using SendGridEmailSample.Application.Queries.EmailList;
using SendGridEmailSample.Web.Hubs;

namespace SendGridEmailSample.Web.Controllers;

public class EmailAlertController : ApiControllerBase
{
    private readonly IHubContext<EmailStatusChangeEventHub> _hub;

    public EmailAlertController(IHubContext<EmailStatusChangeEventHub> hub)
    {
        _hub = hub;
    }

    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> SendEmail(EmailSendProcessorCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllEmail()
    {
        var result = await Mediator.Send(new GetAllEmailQuery());
        return Ok(result);
    }

    [HttpPost]
    [Route("status-update")]
    public async Task<IActionResult> StatusUpdate(List<SendGridStatusUpdateCommand> sendGridEvents)
    {
        foreach (var sendGridEvent in sendGridEvents)
        {
            var response = await Mediator.Send(sendGridEvent);
            if (response is not null)
                await _hub.Clients.All.SendAsync("StatusUpdated", response);
        }

        return Ok();
    }

    [HttpGet]
    [Route("hub-test")]
    public async Task<IActionResult> HubTest(int statusId)
    {
        await _hub.Clients.All.SendAsync("StatusUpdated",
            new { Id = "C418FE82-E076-4519-550C-08DB2C07C03E".ToLower(), Status = statusId });
        return Ok();
    }
}
