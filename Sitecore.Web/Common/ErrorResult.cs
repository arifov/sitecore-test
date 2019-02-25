using System.Collections.Generic;

namespace Sitecore.Web.Common
{
    public interface IErrorResult
    {
    }

    public class ErrorResult : IErrorResult
    {
        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }

        public ErrorResult(IEnumerable<string> errors, bool succeeded)
        {
            Errors = errors;
            Succeeded = succeeded;
        }
    }

    public class BadRequestResult : IErrorResult
    {
        public string Message { get; }

        public BadRequestResult(string msg)
        {
            Message = msg;
        }
    }
}
