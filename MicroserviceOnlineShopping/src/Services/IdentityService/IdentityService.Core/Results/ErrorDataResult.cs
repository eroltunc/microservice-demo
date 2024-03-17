namespace IdentityService.Core.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        // sadece data
        public ErrorDataResult(T data) : base(data, false) { }

        // data ve message
        public ErrorDataResult(T data, string message) : base(data, false, message) { }
        // default data ve message
        public ErrorDataResult(string message) : base(default!, false, message) { }

        // sadece default data
        public ErrorDataResult() : base(default!, false) { }
    }
}
