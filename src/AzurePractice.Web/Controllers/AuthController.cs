using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;

using System.Text;
using System.Text.Json;

namespace AzurePractice.Web.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly GraphServiceClient _graphServiceClient;
    private readonly ITokenAcquisition _tokenAcquisition;

    public AuthController(
        GraphServiceClient graphServiceClient,
        ITokenAcquisition tokenAcquisition)
    {
        _graphServiceClient = graphServiceClient;
        _tokenAcquisition = tokenAcquisition;
    }

    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { "User.Read" })]
    [HttpGet("access-token")]
    public async Task<IActionResult> AccessToken()
    {
        var accessToken =
            await _tokenAcquisition.GetAccessTokenForUserAsync(
                new[] { "User.Read" });

        var parts = accessToken.Split('.');

        var payload =
            JsonSerializer.Deserialize<JsonElement>(
                DecodeBase64Url(parts[1]));

        return Ok(payload);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            IsAuthenticated = User.Identity?.IsAuthenticated,
            Name = User.Identity?.Name,
            Claims = User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            })
        });
    }

    [Authorize]
    [HttpGet("token")]
    public async Task<IActionResult> Token()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");

        if (string.IsNullOrWhiteSpace(idToken))
        {
            return NotFound("No ID token was found.");
        }

        var parts = idToken.Split('.');

        return Ok(new
        {
            Header = JsonSerializer.Deserialize<JsonElement>(
                DecodeBase64Url(parts[0])),

            Payload = JsonSerializer.Deserialize<JsonElement>(
                DecodeBase64Url(parts[1])),

            SignatureLength = parts[2].Length
        });
    }

    [Authorize]
    [AuthorizeForScopes(Scopes = new[] { "User.Read" })]
    [HttpGet("graph-me")]
    public async Task<IActionResult> GraphMe()
    {
        var user = await _graphServiceClient.Me.GetAsync();

        return Ok(new
        {
            user?.Id,
            user?.DisplayName,
            user?.Mail,
            user?.UserPrincipalName
        });
    }

    private static string DecodeBase64Url(string value)
    {
        var base64 = value
            .Replace('-', '+')
            .Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Encoding.UTF8.GetString(
            Convert.FromBase64String(base64));
    }
}