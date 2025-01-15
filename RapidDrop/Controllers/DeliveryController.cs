using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/delivery")]
public class DeliveryController : ControllerBase
{
    private readonly DeliveryService _deliveryService;

    public DeliveryController(DeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveDelivery([FromBody] Delivery delivery)
    {
        if (delivery == null)
            return BadRequest("Delivery data is required.");

        await _deliveryService.CreateAsync(delivery);
        return Ok(new { message = "Delivery saved successfully!" });
    }

    [HttpGet]
    public async Task<IActionResult> GetDeliveries()
    {
        var deliveries = await _deliveryService.GetAllAsync();
        return Ok(deliveries);
    }
}
