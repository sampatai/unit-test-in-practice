using AutoFixture;
using Bogus;
using Bogus.DataSets;
using Moq;
using System.Collections.Generic;
using UnitTestInPractice.Domain.Enum;
using UnitTestInPractice.Domain.Root;
using UnitTestInPractice.Domain.ValueObjects;

namespace UnitTestInPractice.Application.Test.Helper
{
    internal class AssessmentFixture
    {
        private Assessment _assessment = null!;
        private IEnumerable<Assessment> _assessments = null!;
        private Fixture _commandFixture = new Fixture();
        private Faker _faker = new Faker();

        public Assessment Build() => _assessment;
        public IEnumerable<Assessment> ListBuild() => _assessments;
        public AssessmentFixture WithAssessment()
        {
            _commandFixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.SetupGet(x => x.Id).Returns(Random.Shared.Next(1, 100));
                moq.CallBase = true;
                moq.Object.AddResponse(_faker.Random.Guid(), _faker.Random.String2(4));
                moq.Object.CompleteAssessment(_faker.Random.Number(1, 50), SeverityLevel.Low);

                return moq.Object;
            });
            return this;
        }
        public AssessmentFixture WithAssessments(int count)
        {
            // Create and register multiple assessment objects using a loop based on the count
            _commandFixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.SetupGet(x => x.Id).Returns(Random.Shared.Next(1, 100));
                moq.CallBase = true;
                moq.Object.AddResponse(_faker.Random.Guid(), _faker.Random.String2(4));
                moq.Object.CompleteAssessment(_faker.Random.Number(1, 50), SeverityLevel.Low);

                return moq.Object;
            });
            _assessments = _commandFixture.CreateMany<Assessment>(count);

            return this;
        }


        public AssessmentFixture WithAddResponse()
        {
            _commandFixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.CallBase = true;
                moq.Object.AddResponse(_faker.Random.Guid(), _faker.Random.String2(5));
                return moq.Object;
            });
            return this;
        }

    }
}
