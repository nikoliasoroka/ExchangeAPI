using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services.Identity;
using ExchangeAPI.Modules.Identity.Domain.Entities;
using ExchangeAPI.Shared.Common.Constants;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;
using ExchangeAPI.Shared.Common.Models.Result.Implementations.Generics;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace ExchangeAPI.Modules.Identity.Application.Features.Account.Commands.Login;

public class UserLoginCommand : IRequest<IResult<UserLoginResponse>>
{
    [Required]
    [DefaultValue("admin")]
    public string UserName { get; set; }
    [Required]
    [DefaultValue("Admin1!")]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, IResult<UserLoginResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;
    private readonly IActivityService _activityService;
    private readonly ITokenService _tokenService;

    public UserLoginCommandHandler(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager, 
        IUserService userService, 
        IActivityService activityService, 
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
        _activityService = activityService;
        _tokenService = tokenService;
    }


    public async Task<IResult<UserLoginResponse>> Handle(UserLoginCommand command, CancellationToken cancellationToken)
    {
        try
        {

            var signInResult = await _signInManager.PasswordSignInAsync(command.UserName, command.Password, command.RememberMe, true);

            if (signInResult is { Succeeded: false, IsLockedOut: false, RequiresTwoFactor: false })
            {
                await _activityService.SaveActivity(command.UserName, "Authentication", "Username or password is incorrect.", cancellationToken);

                return Result<UserLoginResponse>.CreateFailed(ErrorModel.WrongPasswordOrUserName);
            }

            var user = await _userManager.FindByNameAsync(command.UserName);

            if (user is null)
                return Result<UserLoginResponse>.CreateFailed(ErrorModel.UserNotFound);

            if (user.IsBlocked)
            {
                await _activityService.SaveActivity(command.UserName, "Authentication", "User is blocked.", cancellationToken);

                return Result<UserLoginResponse>.CreateFailed(ErrorModel.UserIsBlocked);
            }

            var actionTypeResult = await _userService.UserHasActionType(SharedActionTypes.LoginBackend, user);

            if (!actionTypeResult)
            {
                await _activityService.SaveActivity(command.UserName, "Authentication", "User does not have access to BL4SEA backend.", cancellationToken);

                return Result<UserLoginResponse>.CreateFailed(ErrorModel.UserDoesNotHaveAccess);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                await _activityService.SaveActivity(command.UserName, "Authentication", "Pending verification of E-mail address.", cancellationToken);

                return Result<UserLoginResponse>.CreateFailed(ErrorModel.EmailIsNotConfirmed);
            }

            if (signInResult.RequiresTwoFactor)
            {
                await _activityService.SaveActivity(command.UserName, "Authentication", "Requires two factor authentication.", cancellationToken);

                if (user.UserTokens.Any(x => x.Name.Equals("AuthenticatorKey")) && user.TwoFactorEnd.HasValue && System.Convert.ToDateTime(user.TwoFactorEnd.Value) >= DateTime.UtcNow)
                {
                    var userLoginResponse = await _tokenService.GenerateWebTokenResponse(user, true, cancellationToken);

                    return Result<UserLoginResponse>.CreateSuccess(userLoginResponse);
                }
                else
                {
                    var userLoginResponse = await _tokenService.GenerateWebTokenResponse(user, false, cancellationToken);

                    return Result<UserLoginResponse>.CreateSuccess(userLoginResponse);
                }
            }

            if (signInResult.IsLockedOut)
            {
                await _activityService.SaveActivity(command.UserName, "Authentication", "User is locked out.", cancellationToken);

                return Result<UserLoginResponse>.CreateFailed(ErrorModel.UserIsLockedOut);
            }

            var response = await _tokenService.GenerateWebTokenResponse(user, true, cancellationToken);

            await _activityService.SaveActivity(command.UserName, "Authentication", "User is signed in.", cancellationToken);

            return Result<UserLoginResponse>.CreateSuccess(response);
        }
        catch (Exception e)
        {
            return Result<UserLoginResponse>.CreateFailed(e.Message);
        }
    }
}