

namespace UnitTestInPractice.Domain.Entities
{
    public class Survey : Entity
    {
        protected Survey()
        {
            
        }
        public Guid SurveyGUID { get; private set; }
        public string Title { get; private set; }
        public List<Question> Questions { get; private set; }

        public Survey(string title, List<Question> questions)
        {
            SurveyGUID = Guid.NewGuid();
            Guard.Against.NullOrEmpty(title, nameof(title));
            Guard.Against.Null(questions, nameof(questions));
            Title = title;
            Questions = questions;
        }
    }
}
