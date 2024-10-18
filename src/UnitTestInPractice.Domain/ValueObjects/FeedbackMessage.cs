
namespace UnitTestInPractice.Domain.ValueObjects
{
    public class FeedbackMessage : ValueObject
    {
        protected FeedbackMessage()
        {
            
        }
        public string Message { get; }

        public FeedbackMessage(string message)
        {
            Guard.Against.NullOrEmpty(message, nameof(message), "Feedback message cannot be empty.");
            Message = message;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Message;
        }
    }
}
