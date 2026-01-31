using System;

namespace RealEstate.API.Middlewares;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Hassas bilgilerin sızmasını önle
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        
        // Clickjacking saldırılarını önle (Sitenin iframe içinde açılmasını engeller)
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        
        // XSS saldırılarına karşı tarayıcı korumasını aç
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        
        // Referrer bilgisini gizle (Hangi siteden gelindiği bilgisini gönderme)
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");

        // HTTPS zorunluluğu (HSTS) - Prod ortamında önemlidir
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

        await _next(context);
    }
}
