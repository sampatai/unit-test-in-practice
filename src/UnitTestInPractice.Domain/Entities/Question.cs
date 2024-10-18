using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestInPractice.Domain.Entities
{
    public class Question:Entity
    {
        protected Question()
        {
            
        }
        public Guid QuestionGUID { get; private set; }
        public string Text { get; private set; }
        public List<string> Options { get; private set; }

        public Question(string text, List<string> options)
        {
            QuestionGUID = Guid.NewGuid();
            Guard.Against.NullOrEmpty(text, nameof(text));
            Guard.Against.Null(options, nameof(options));
            Text = text;
            Options = options;
        }
    }
}
