namespace ExchangeAPI.Shared.Common.Constants;

public static class ErrorModel
{
    public const string DataNotFound = "DATA_NOT_FOUND";
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string AccessDenied = "ACCESS_DENIED";
    public const string ResourceNotFound = "THE_SPECIFIED_RESOURCE_WAS_NOT_FOUND";
    public const string Forbidden = "FORBIDDEN";
    public const string ValidationFailures = "VALIDATION_FAILURES";

    public const string WrongPasswordOrUserName = "WRONG_PASSWORD_OR_USERNAME";
    public const string InvalidToken = "INVALID_TOKEN";
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string UserDoesNotHaveAccess = "USER_DOES_NOT_HAVE_ACCESS";
    public const string UserIsBlocked = "USER_IS_BLOCKED"; 
    public const string EmailIsNotConfirmed = "EMAIL_IS_NOT_CONFIRMED";
    public const string UserIsLockedOut = "USER_IS_LOCKED_OUT";
}