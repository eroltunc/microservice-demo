namespace BasketService.Core.Results
{
    public class Result : IResult
    {
        public bool Success { get; }

        public string Message { get; }


        public Result(bool Success)
        {
            this.Success = Success;
        }

        public Result(bool Success, string Message) : this(Success)
        {
            this.Message = Message;
        }
    }
}
