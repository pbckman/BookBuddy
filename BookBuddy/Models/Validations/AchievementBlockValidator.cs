using BookBuddy.Models.Blocks;
using EPiServer.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.Models.Validations
{
    public class AchievementBlockValidator : IValidate<AchievementBlock>
    {
        public IEnumerable<ValidationError> Validate(AchievementBlock instance)
        {
            var errors = new List<ValidationError>();

            if (instance.NumberOfBooks <= 0 && instance.NumberOfChapters <= 0)
            {
                errors.Add(new ValidationError
                {
                    ErrorMessage = "Either Number of Books or Number of Chapters must be greater than 0.",
                    PropertyName = nameof(instance.NumberOfBooks),
                    Severity = ValidationErrorSeverity.Error
                });
            }

            return errors;
        }
    }
}
