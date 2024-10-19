namespace UnitTestInPractice.Application.Command;

public abstract class Validator<T> : AbstractValidator<T> where T : AssessmentCommand
{
    public Validator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Name is required.");

        RuleForEach(x => x.Responses).SetValidator(new ResponseValidator());

    }

}

public class ResponseValidator : AbstractValidator<ResponseCommand>
{
    public ResponseValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEqual(Guid.Empty)
            .WithMessage("QuestionId cannot be empty.");
        RuleFor(x => x.Answer)
            .Must(x => x.Length > 0)
            .NotEmpty()
            .WithMessage("Answer cannot be empty.");
    }
}