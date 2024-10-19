using AutoFixture;
using Bogus;
using System;
using System.Collections.Generic;
using UnitTestInPractice.Application.Command;

namespace UnitTestInPractice.Application.Test.Helper
{
    public class CreateCommandFixture
    {
        private readonly Fixture _commandFixture;
        private readonly Faker _faker;

        public CreateCommandFixture()
        {
            _commandFixture = new Fixture();
            _faker = new Faker();
        }

        public CreateAssessment.Command Build()
        {
            return _commandFixture.Create<CreateAssessment.Command>();
        }

        public CreateCommandFixture WithFullName(string fullName)
        {
            _commandFixture.Customize<CreateAssessment.Command>(c =>
            {
                var detailsCommand = new DetailsCommand
                (
                    FullName: fullName,
                    Occupation: _faker.Name.JobTitle(),
                    Address: _faker.Address.FullAddress()
                );

                return c.With(x => x.DetailsCommand, detailsCommand);
            });

            return this;
        }

        public CreateCommandFixture WithOccupation(string occupation)
        {
            _commandFixture.Customize<CreateAssessment.Command>(c =>
            {
                var detailsCommand = new DetailsCommand
                (
                    FullName: _faker.Name.FullName(),
                    Occupation: occupation,
                    Address: _faker.Address.FullAddress()
                );

                return c.With(x => x.DetailsCommand, detailsCommand);
            });

            return this;
        }

        public CreateCommandFixture WithAddress(string address)
        {
            _commandFixture.Customize<CreateAssessment.Command>(c =>
            {
                var detailsCommand = new DetailsCommand
                (
                    FullName: _faker.Name.FullName(),
                    Occupation: _faker.Name.JobTitle(),
                    Address: address
                );

                return c.With(x => x.DetailsCommand, detailsCommand);
            });

            return this;
        }

        public CreateCommandFixture WithAddResponse(Guid questionId, string answer)
        {
            _commandFixture.Customize<CreateAssessment.Command>(c =>
            {
                var detailsCommand = new DetailsCommand
                (
                    FullName: _faker.Name.FullName(),
                    Occupation: _faker.Name.JobTitle(),
                    Address: _faker.Address.FullAddress()
                );

                var response = new ResponseCommand(questionId, answer);

                return c.With(x => x.DetailsCommand, detailsCommand)
                        .With(x => x.Responses, new List<ResponseCommand> { response });
            });

            return this;
        }

        public CreateCommandFixture WithRandomResponses(int numberOfResponses)
        {
            _commandFixture.Customize<CreateAssessment.Command>(c =>
            {
                var detailsCommand = new DetailsCommand
                (
                    FullName: _faker.Name.FullName(),
                    Occupation: _faker.Name.JobTitle(),
                    Address: _faker.Address.FullAddress()
                );

                var responses = new List<ResponseCommand>();

                for (int i = 0; i < numberOfResponses; i++)
                {
                    responses.Add(new ResponseCommand(
                        _faker.Random.Guid(),
                        _faker.Random.String2(5)
                    ));
                }

                return c.With(x => x.DetailsCommand, detailsCommand)
                        .With(x => x.Responses, responses);
            });

            return this;
        }
    }
}
