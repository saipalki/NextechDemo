using System.ComponentModel.DataAnnotations;

 namespace NextechDemo.Shared.Exceptions
{
    public enum ExceptionType
    {
        [Display(Name = "Fatal Error")]
        FatalError,
        [Display(Name = "Validation Error")]
        ValidationError,
        [Display(Name = "SQL Error")]
        SQLError,
        [Display(Name = "Syntax Error")]
        SyntaxError,
        [Display(Name = "Application Error")]
        ApplicationError,
        [Display(Name = "External Api Error")]
        ExternalApiError,
        [Display(Name = "Permission Denied")]
        PermissionDenied
    }
}
