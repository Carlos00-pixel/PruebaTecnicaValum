using FluentValidation;
using MediatR;

namespace ProjectManagement.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        // Ejecuta las validaciones de la petición antes de que llegue al handler.
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = new List<FluentValidation.Results.ValidationFailure>();

            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken);

                failures.AddRange(result.Errors);
            }

            // Si hay errores de validación, se detiene la ejecución y se lanza una excepción.
            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
