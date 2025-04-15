using FluentValidation;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping
{
    public class ValidationMappingMiddlware
    { 
        private readonly RequestDelegate _next;

        public ValidationMappingMiddlware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next (context);
            }
            catch(ValidationException ex)
            {
                context.Response.StatusCode = 400;

                var validationFailureResponse = new ValidationFailureResponse
                {

                    Errors = ex.Errors.Select( x=> new ValidationResponse
                    {
                        PropertyName = x.PropertyName , Message=x.ErrorMessage
                    })
                };

                await context.Response.WriteAsJsonAsync (validationFailureResponse);
            }
        }
    }
}
