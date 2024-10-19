using AutoFixture;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestInPractice.Application.Behaviors;
using UnitTestInPractice.Application.Command;
using UnitTestInPractice.Application.Query;
using UnitTestInPractice.Application.Repository;
using UnitTestInPractice.Application.Test.Helper;
using UnitTestInPractice.Domain.Enum;
using UnitTestInPractice.Domain.Root;
using UnitTestInPractice.Shared.SeedWork;
using static UnitTestInPractice.Application.Query.GetAssessments;

namespace UnitTestInPractice.Application.Test
{

    internal class GetAssessmentsTest
    {
        private readonly int _resultCount = 10;
        GetAssessments.Query Query;

        private AutoMocker _mocker = new AutoMocker();
        private Fixture _fixture = new Fixture();
        private IMediator _mediator;
        private IServiceCollection _services;
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private ServiceProvider _provider;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private CreateCommandFixture _commandFixture = new();
        IEnumerable<Assessment> _assessments;
        [SetUp]

        public virtual void Setup()
        {
            // Reset AutoMocker for each test
            _mocker = new AutoMocker();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Initialize the service collection
            _services = new ServiceCollection();

            // Add the pipeline behavior (ValidatorBehavior) and memory cache
            _services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            _services.AddMemoryCache();

            // Add logging for the ValidatorBehavior and command handler using AutoMocker
            _services.AddTransient<ILogger<ValidatorBehavior<GetAssessments.Query, GetAssessments.ListAssessment>>>(provider =>
                _mocker.GetMock<ILogger<ValidatorBehavior<GetAssessments.Query, GetAssessments.ListAssessment>>>().Object);

            _services.AddTransient<ILogger<GetAssessments.Handler>>(provider =>
                _mocker.GetMock<ILogger<GetAssessments.Handler>>().Object);

            // Add validators and MediatR for the command handler
            var assembly = typeof(GetAssessments.Handler).Assembly;
            _services.AddValidatorsFromAssemblyContaining<GetAssessments.Query>(ServiceLifetime.Scoped);
            _services.AddMediatR(x => x.RegisterServicesFromAssemblies(assembly));

            // Add the repository mock
            _services.AddScoped<IReadOnlyAssessmentRepository>(_ => _mocker.GetMock<IReadOnlyAssessmentRepository>().Object);
            _assessments = new AssessmentFixture().WithAssessments(_resultCount).ListBuild();
            _mocker.GetMock<IReadOnlyAssessmentRepository>()
             .Setup(x => x.GetAssessments(It.IsAny<FilterModel>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(() => (_assessments, _resultCount));
            // Build the service provider and resolve IMediator
            _provider = _services.BuildServiceProvider();
            _mediator = _provider.GetRequiredService<IMediator>();
            Query = CreateQuery();
        }
        [Test]
        public async Task Handle_Should_Return_ListAssessment()
        {
            // Arrange


            // Act
            var result = await _mediator.Send(Query);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ListAssessment>();
        }

        [Test]
        public void Should_Have_Error_When_PageNumber_Is_Less_Than_One()
        {
            // Arrange
            Query.PageNumber = 0;

            // Act
            AsyncTestDelegate sut = async () => await _mediator.Send(Query);
            // Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(sut);

            assertion.Errors.Should().Contain(x => x.PropertyName == nameof( GetAssessments.Query.PageNumber) && x.ErrorMessage == "Page number must be greater than 0.");
        }

        [Test]
        public void Should_Have_Error_When_PageSize_Is_Out_Of_Range()
        {
            // Arrange
            Query.PageSize = 101;

            // Act
            AsyncTestDelegate sut = () => _mediator.Send(Query);

            // Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(sut);

            assertion.Errors.Should().Contain(x => x.PropertyName == nameof( GetAssessments.Query.PageSize) && x.ErrorMessage == "Page size must be between 1 and 100.");
        }

       

        protected GetAssessments.Query CreateQuery() => Query = _fixture.Build<GetAssessments.Query>()
           .With(q => q.PageNumber, 1)
           .With(q => q.PageSize, 10)
           .With(q => q.Status, (AssessmentStatus)null)
            .With(q => q.DateCreated, (DateTime?)null)

           .Create();
    }

}

