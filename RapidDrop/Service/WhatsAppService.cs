using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

public class WhatsAppService
{
    private readonly string _accessToken;
    private readonly string _phoneNumberId;
    private readonly string _apiBaseUrl;
    private readonly HttpClient _httpClient;

    public WhatsAppService(IConfiguration configuration)
    {
        var settings = configuration.GetSection("WhatsAppAPI");
        _accessToken = settings["AccessToken"];
        _phoneNumberId = settings["PhoneNumberId"];
        _apiBaseUrl = settings["ApiBaseUrl"];

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    public async Task<bool> SendMessage(string recipient, string message)
    {
        var url = $"{_apiBaseUrl}{_phoneNumberId}/messages";
        var payload = new
        {
            messaging_product = "whatsapp",
            to = "+91" + recipient,
            type = "text",
            text = new { body = message }
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        return response.IsSuccessStatusCode;
    }
    public async Task<bool> SendImage(string recipient, byte[] imageBytes)
    {
        // Step 1: Upload image to WhatsApp Media Server
        var uploadUrl = $"{_apiBaseUrl}{_phoneNumberId}/media";
        var mediaContent = new MultipartFormDataContent();

        var fileContent = new ByteArrayContent(imageBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        mediaContent.Add(fileContent, "file", "bill.png");

        var uploadResponse = await _httpClient.PostAsync(uploadUrl, mediaContent);
        if (!uploadResponse.IsSuccessStatusCode)
        {
            Console.WriteLine("Failed to upload image");
            return false;
        }

        // Get the media ID from the response
        var uploadResponseContent = await uploadResponse.Content.ReadAsStringAsync();
        var mediaId = JsonConvert.DeserializeObject<dynamic>(uploadResponseContent).id;

        // Step 2: Send the image to the recipient
        var messageUrl = $"{_apiBaseUrl}{_phoneNumberId}/messages";
        var messagePayload = new
        {
            messaging_product = "whatsapp",
            to = "+91" + recipient,
            type = "image",
            image = new
            {
                id = mediaId
            }
        };

        var messageContent = new StringContent(JsonConvert.SerializeObject(messagePayload), Encoding.UTF8, "application/json");
        var messageResponse = await _httpClient.PostAsync(messageUrl, messageContent);

        return messageResponse.IsSuccessStatusCode;
    }
    public async Task<bool> SendRatingRequest(string recipient)
    {
        var url = $"{_apiBaseUrl}{_phoneNumberId}/messages";

        var payload = new
        {
            messaging_product = "whatsapp",
            to = "+91" + recipient,
            type = "interactive",
            interactive = new
            {
                type = "button",
                body = new
                {
                    text = "Please rate our service! and Get your E-bill"
                },
                action = new
                {
                    buttons = new[]
                    {
                    new
                    {
                        type = "reply",
                        reply = new
                        {
                            id = "rating_1",
                            title = "1 ⭐"
                        }
                    },
                    new
                    {
                        type = "reply",
                        reply = new
                        {
                            id = "rating_2",
                            title = "2 ⭐⭐"
                        }
                    },
                    new
                    {
                        type = "reply",
                        reply = new
                        {
                            id = "rating_3",
                            title = "3 ⭐⭐⭐"
                        }
                    }
                }
                }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        // Log response for debugging
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status Code: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        return response.IsSuccessStatusCode;
    }

}
