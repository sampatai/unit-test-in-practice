using AutoFixture;
using Bogus;
using Bogus.DataSets;
using Moq;
using UnitTestInPractice.Domain.Enum;
using UnitTestInPractice.Domain.Root;
using UnitTestInPractice.Domain.ValueObjects;

namespace UnitTestInPractice.Application.Test.Helper
{
    internal class AssessmentFixture
    {
        private Assessment _assessment = null!;
        private Fixture _fixture = new Fixture();
        private Faker _faker = new Faker();

        public Assessment Build() => _assessment;

        public AssessmentFixture WithAssessment()
        {
            _fixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.SetupGet(x => x.Id).Returns(Random.Shared.Next(1, 100));
                moq.CallBase = true;
                moq.Object.AddResponse(_faker.PickRandom<Guid>(), _faker.PickRandom<string>());
                moq.Object.CompleteAssessment(_faker.Random.Number(1, 50), _faker.PickRandom<SeverityLevel>());

                return moq.Object;
            });
            return this;
        }

        public AssessmentFixture WithFullName(string fullName)
        {
            _fixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(fullName, x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.CallBase = true;

                return moq.Object;
            });
            return this;
        }
        public AssessmentFixture WithOccupation(string occumation)
        {
            _fixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), occumation, x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.CallBase = true;

                return moq.Object;
            });
            return this;
        }

        public AssessmentFixture WithAddress(string address)
        {
            _fixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), address))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.CallBase = true;

                return moq.Object;
            });
            return this;
        }
        public AssessmentFixture WithAddResponseAndQuestion(Guid questionId)
        {
            _fixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.CallBase = true;
                moq.Object.AddResponse(questionId, _faker.Random.String2(5));
                return moq.Object;
            });
            return this;
        }

        public AssessmentFixture WithAddResponseAndAnswer(string answer)
        {
            _fixture.Register<Assessment>(() =>
            {
                var assessment = new Faker<AssessmentTakerDetails>()
                .CustomInstantiator(x => new AssessmentTakerDetails(x.Name.FullName(), x.Name.JobTitle(), x.Address.FullAddress()))
                .Generate();
                var moq = new Mock<Assessment>(assessment);
                moq.CallBase = true;
                moq.Object.AddResponse(_faker.Random.Guid(), answer);
                return moq.Object;
            });
            return this;
        }
    }
}
