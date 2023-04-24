using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace NotenTool.Api.Test.Integration;

internal class AllowAnonymousAuthentionHandlerProvider : IAuthenticationHandlerProvider
{
    public async Task<IAuthenticationHandler?> GetHandlerAsync(HttpContext context, string authenticationScheme)
    {
        return await Task.FromResult(new AuthenticationRequestHandler());
    }
}

internal class AuthenticationRequestHandler : IAuthenticationRequestHandler
{
    Task IAuthenticationHandler.InitializeAsync(AuthenticationScheme scheme, HttpContext context) =>
        Task.CompletedTask;

    Task<AuthenticateResult> IAuthenticationHandler.AuthenticateAsync() =>
        Task.FromResult(
            AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>()), "")));

    Task IAuthenticationHandler.ChallengeAsync(AuthenticationProperties? properties) =>
        Task.CompletedTask;

    Task IAuthenticationHandler.ForbidAsync(AuthenticationProperties? properties) =>
        Task.CompletedTask;

    Task<bool> IAuthenticationRequestHandler.HandleRequestAsync() =>
        Task.FromResult(false);
}

internal class AllowAnonymousAuthenticationService : IAuthenticationService
{
    public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), "")));
    }

    public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new System.NotImplementedException();
    }

    public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal,
        AuthenticationProperties? properties)
    {
        throw new System.NotImplementedException();
    }

    public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new System.NotImplementedException();
    }
}