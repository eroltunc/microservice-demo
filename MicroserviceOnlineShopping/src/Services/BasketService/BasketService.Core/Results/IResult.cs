namespace BasketService.Core.Results
{
    public interface IResult // ana result 
    {
       public bool Success { get; }
       public string Message { get; }
    }
}
 