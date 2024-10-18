using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestInPractice.Domain.Root
{
    public class Assessment : Entity, IAggregateRoot
    {
        protected Assessment()
        {

        }


        public Guid AssessmentGUID { get; private set; }
        public string Name { get; private set; }
        public Survey Survey { get; private set; }
        public DateTime DateCreated { get; private set; }
        public AssessmentStatus Status { get; private set; }
        public Score? FinalScore { get; private set; }
        public Feedback? AssessmentFeedback { get; private set; }

        private List<Response> _responses;
        public IReadOnlyCollection<Response> Responses => _responses.AsReadOnly();

        public Assessment(string name, Survey survey)
        {
            Guard.Against.Null(survey, nameof(survey));
            Guard.Against.NullOrEmpty(name, nameof(name));
            AssessmentGUID = Guid.NewGuid();
            Name = name;
            Survey = survey;
            DateCreated = DateTime.UtcNow;
            Status = AssessmentStatus.Pending;
            _responses = new List<Response>();
        }


        public void AddResponse(Guid questionId, string answer)
        {
            var question = Survey.Questions.FirstOrDefault(q => q.QuestionGUID == questionId);
            if (question == null)
                throw new ArgumentException("Invalid question ID.");

            _responses.Add(new Response(questionId, answer));
            Status = AssessmentStatus.InProgress;
        }


        public void CompleteAssessment()
        {
            if (_responses.Count != Survey.Questions.Count)
                throw new InvalidOperationException("All questions must be answered before completing the assessment.");

            Status = AssessmentStatus.Completed;
            FinalScore = CalculateScore();
            AssessmentFeedback = GenerateFeedback();
        }


        private Score CalculateScore()
        {        
            int scoreValue = (_responses.Count * 100) / Survey.Questions.Count;
            return new Score(scoreValue);
        }


        private Feedback GenerateFeedback()
        {
            SeverityLevel severity = FinalScore.Value switch
            {
                <= 25 => SeverityLevel.Low,
                > 25 and <= 50 => SeverityLevel.Moderate,
                > 50 and <= 75 => SeverityLevel.High,
                _ => SeverityLevel.Critical,
            };

            var message = severity.Name switch
            {
                nameof(SeverityLevel.Low) => new FeedbackMessage("You seem to be doing well! Keep up the good work."),
                nameof(SeverityLevel.Moderate) => new FeedbackMessage("You're facing some challenges. Consider reviewing your mental health practices."),
                nameof(SeverityLevel.High) => new FeedbackMessage("You might want to seek professional help soon."),
                nameof(SeverityLevel.Critical) => new FeedbackMessage("Immediate help is recommended. Please contact a professional."),
                _ => new FeedbackMessage("Unknown severity.")
            };

            return new Feedback(AssessmentGUID, severity, message);
        }

    }
}
