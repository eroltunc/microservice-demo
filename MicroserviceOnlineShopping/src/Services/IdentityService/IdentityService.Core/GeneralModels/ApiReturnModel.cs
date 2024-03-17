namespace IdentityService.Core.GeneralModels
{
    public class ApiReturnModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string ItemId { get; set; }
        public HttpStatusCodeEnum StatusCode { get; set; }
    }
    public enum HttpStatusCodeEnum
    {
        Success = 200,
        BadRequest = 400,
        UnAuthorized = 401,
        NotFound = 404,
        MethodNotAllowed = 405,
        InternalServerError = 500
    }
}
