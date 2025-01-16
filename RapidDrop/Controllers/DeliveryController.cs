using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/delivery")]
public class DeliveryController : ControllerBase
{
    private readonly DeliveryService _deliveryService;
    private readonly WhatsAppService _whatsAppService;

    public DeliveryController(DeliveryService deliveryService, WhatsAppService whatsAppService)
    {
        _deliveryService = deliveryService;
        _whatsAppService = whatsAppService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveDelivery([FromBody] Delivery delivery)
    {
        if (delivery == null)
            return BadRequest("Delivery data is required.");

        await _deliveryService.CreateAsync(delivery);
        var ratingService = await _whatsAppService.SendRatingRequest(delivery.WhatsAppNumber);
        return Ok(new { message = "Delivery saved successfully!" });
    }

    [HttpGet]
    public async Task<IActionResult> GetDeliveries()
    {
        var deliveries = await _deliveryService.GetAllAsync();
        return Ok(deliveries);
    }
}
