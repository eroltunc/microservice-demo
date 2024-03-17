namespace BasketService.Core.GeneralModels
{
    public enum HttpStatusCodeEnum
    {
        Success = 200,
        BadRequest = 400,
        UnAuthorized = 401,
        NotFound = 404,
        MethodNotAllowed = 405,
        InternalServerError = 500
    }

    public enum SortType
    {
        Ascending,
        Descending
    }
}