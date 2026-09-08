using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Managementsystem.Helper
{
    public class ModelValidator : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ModelValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context,ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null)
                    continue;

                // Find validator for current model
                var validatorType = typeof(IValidator<>)
                    .MakeGenericType(argument.GetType());

                var validator = _serviceProvider.GetService(validatorType)
                    as IValidator;

                if (validator == null)
                    continue;

                // Validate model
                var result = await validator.ValidateAsync(
                    new ValidationContext<object>(argument),
                    context.HttpContext.RequestAborted);

                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(e => e.ErrorMessage).ToArray()
                        );

                    context.Result = new BadRequestObjectResult(new
                    {
                        statusCode = 400,
                        message = "Validation failed",
                        errors = errors
                    });

                    return;
                }
            }

            await next();
        }
    }
}