using AutoFixture;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using UnitTestInPractice.Application.Behaviors;
using UnitTestInPractice.Application.Command;
using UnitTestInPractice.Application.Repository;
using UnitTestInPractice.Application.Test.Helper;
using UnitTestInPractice.Domain.Root;
using UnitTestInPractice.Shared.SeedWork;


namespace UnitTestInPractice.Application.Test
{
    public class CreateAssessmentTest
    {
        private AutoMocker _mocker = new AutoMocker();
        private Fixture _fixture = new Fixture();
        private IMediator _mediator;
        private IServiceCollection _services;
        private ServiceProvider _provider;
        private CreateCommandFixture _commandFixture = new();
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
            _services.AddTransient<ILogger<ValidatorBehavior<CreateAssessment.Command, Unit>>>(provider =>
                _mocker.GetMock<ILogger<ValidatorBehavior<CreateAssessment.Command, Unit>>>().Object);

            _services.AddTransient<ILogger<CreateAssessment.Handler>>(provider =>
                _mocker.GetMock<ILogger<CreateAssessment.Handler>>().Object);

            // Add validators and MediatR for the command handler
            var assembly = typeof(CreateAssessment.Handler).Assembly;
            _services.AddValidatorsFromAssemblyContaining<CreateAssessment.Command>(ServiceLifetime.Scoped);
            _services.AddMediatR(x => x.RegisterServicesFromAssemblies(assembly));

            // Add the repository mock
            _services.AddScoped<IAssessmentRepository>(_ => _mocker.GetMock<IAssessmentRepository>().Object);
            _mocker.GetMock<IAssessmentRepository>()
                .SetupGet(x => x.UnitOfWork)
                .Returns(_mocker.GetMock<IUnitOfWork>().Object);
            // Build the service provider and resolve IMediator
            _provider = _services.BuildServiceProvider();
            _mediator = _provider.GetRequiredService<IMediator>();
        }

       
        [Test]
        public async Task Should_Have_Error_When_FullName_Is_Empty()
        {
            // Arrange
            var command = _commandFixture
                .WithFullName(string.Empty)
                .Build();

            // Act & Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(async () =>await _mediator.Send(command))!;

            assertion.Errors.Should().Contain(x => x.PropertyName == "DetailsCommand.FullName"
            && x.ErrorMessage == "Name is required.");

          
        }

        [Test]
        public async Task Should_Have_Error_When_Occupation_Is_Empty()
        {
            // Arrange
            var command = _commandFixture
                .WithOccupation(string.Empty)
                .Build();

            // Act & Assert
           
            var assertion = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command))!;

            assertion.Errors.Should().Contain(x => x.PropertyName == "DetailsCommand.Occupation"
           && x.ErrorMessage == "Occupation is required.");
          
        }

        [Test]
        public async Task Should_Have_Error_When_Address_Is_Empty()
        {
            // Arrange
            var command = _commandFixture
                .WithAddress(string.Empty)
                .Build();

            // Act & Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(async () => await _mediator.Send(command))!;

            assertion.Errors.Should().Contain(x => x.PropertyName == "DetailsCommand.Address"
          && x.ErrorMessage == "Address is required.");

        }

        [Test]
        public async Task Should_Have_Error_When_Response_Answer_Is_Empty()
        {
            // Arrange
            var command = _commandFixture
                .WithAddResponse(Guid.NewGuid(), string.Empty)
                .Build();

            // Act & Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command))!;


            assertion.Errors.Should().Contain(x => x.PropertyName == "Responses[0].Answer"
                            && x.ErrorMessage == "Answer cannot be empty.");
           
        }
        [Test]
        public async Task Should_Have_Error_When_Response_Question_Is_Empty()
        {
            // Arrange
            var command = _commandFixture
                .WithAddResponse(Guid.Empty, _fixture.Create<string>())
                .Build();

            // Act & Assert
            var assertion = Assert.ThrowsAsync<ValidationException>(() => _mediator.Send(command))!;


            assertion.Errors.Should().Contain(x => x.PropertyName == "Responses[0].QuestionId"
        && x.ErrorMessage == "QuestionId cannot be empty.");

        }

        [Test]
        public async Task Should_Add_Valid_Assessment_Command()
        {
            // Arrange
            var command = _commandFixture
                .WithFullName(_fixture.Create<string>())
                .WithOccupation(_fixture.Create<string>())
                .WithAddress(_fixture.Create<string>())
                .WithAddResponse(Guid.NewGuid(), "Yes")
                .Build();

            // Act
            await _mediator.Send(command);

            // Assert
            _mocker.GetMock<IAssessmentRepository>().Verify(x => x.AddAsync(It.IsAny<Assessment>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
