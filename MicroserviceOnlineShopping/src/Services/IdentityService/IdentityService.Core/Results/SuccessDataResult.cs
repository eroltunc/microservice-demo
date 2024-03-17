namespace IdentityService.Core.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        
        // sadece data
        public SuccessDataResult(T data) : base(data, true) { }

        // data ve message
        public SuccessDataResult(T data, string message) : base(data, true, message) { }

        // default data ve message
        public SuccessDataResult(string message) : base(default!, true, message) { }

        // sadece default data
        public SuccessDataResult() : base(default!, true) { }
    }
}
