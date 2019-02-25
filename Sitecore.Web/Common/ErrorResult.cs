using System.Collections.Generic;

namespace Sitecore.Web.Common
{
    /// <summary>
    /// Common error interface
    /// </summary>
    public interface IErrorResult
    {
    }

    /// <summary>
    /// UI Error model
    /// </summary>
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

    /// <summary>
    /// Bad request error
    /// </summary>
    public class BadRequestResult : IErrorResult
    {
        public string Message { get; }

        public BadRequestResult(string msg)
        {
            Message = msg;
        }
    }
}
