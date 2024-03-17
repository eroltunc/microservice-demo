namespace BasketService.Core.GeneralModels
{
    public class ApiReturnModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string ItemId { get; set; }
        public HttpStatusCodeEnum StatusCode { get; set; }
    }
   
}
