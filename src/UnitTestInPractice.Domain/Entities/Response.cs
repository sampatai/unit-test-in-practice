

namespace UnitTestInPractice.Domain.Entities
{
    public class Response : Entity
    {
        protected Response()
        {

        }
        public Guid QuestionGUID { get; private set; }
        public string Answer { get; private set; }

        public Response(Guid questionId, string answer)
        {
            Guard.Against.Default(questionId, nameof(questionId));
            Guard.Against.NullOrEmpty(answer, nameof(answer));
            QuestionGUID = questionId;
            Answer = answer;

        }

    }
}
