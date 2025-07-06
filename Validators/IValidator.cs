using System.ComponentModel.DataAnnotations;

namespace TaskManager.Validators
{
    public interface IValidator<T> 
        where T : class
    {
        /// <summary>
        /// Validates the model and returns a collection of validation results.
        /// </summary>
        /// <returns>An enumerable collection of validation results.</returns>
        IEnumerable<ValidationResult> Validate(T model);
    }
}
