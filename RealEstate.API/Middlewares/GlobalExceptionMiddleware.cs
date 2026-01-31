using System.Net;
using System.Text.Json;

namespace RealEstate.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // İstek sorunsuz ise devam et
            await _next(context);
        }
        catch (Exception ex)
        {
            // Hata varsa yakala ve yönet
            _logger.LogError(ex, "Sunucu Hatası: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Varsayılan hata kodu: 500 (Internal Server Error)
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Hata mesajı objesi (Standart bir format belirliyoruz)
        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "Sunucu kaynaklı bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.", // Prod için generic mesaj
            Detailed = exception.Message // Dev ortamında hatayı görmek için (Canlıya çıkarken burayı silebilirsin)
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}