using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppController : ControllerBase
{
    private readonly WhatsAppService _whatsAppService;

    public WhatsAppController(WhatsAppService whatsAppService)
    {
        _whatsAppService = whatsAppService;
    }

    // Endpoint to send a plain text message
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] WhatsAppMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.Recipient) || string.IsNullOrEmpty(request.Message))
            return BadRequest("Recipient and message are required.");

        var success = await _whatsAppService.SendMessage(request.Recipient, request.Message);

        if (success)
            return Ok(new { message = "Message sent successfully!" });
        else
            return StatusCode(500, "Failed to send message.");
    }

    // New endpoint to send a rating request with quick replies
    [HttpPost("send-rating")]
    public async Task<IActionResult> SendRatingRequest([FromBody] RatingRequest request)
    {
        if (string.IsNullOrEmpty(request.Recipient))
            return BadRequest("Recipient is required.");

        var success = await _whatsAppService.SendRatingRequest(request.Recipient);

        if (success)
            return Ok(new { message = "Rating request sent successfully!" });
        else
            return StatusCode(500, "Failed to send rating request.");
    }
}

// Request models
public class WhatsAppMessageRequest
{
    public string Recipient { get; set; }
    public string Message { get; set; }
}

public class RatingRequest
{
    public string Recipient { get; set; }
}
