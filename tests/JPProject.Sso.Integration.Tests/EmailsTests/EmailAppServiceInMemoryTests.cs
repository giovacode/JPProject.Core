﻿using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Fakers.Test.Email;
using JPProject.Sso.Integration.Tests.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.EmailsTests
{
    public class EmailAppServiceInMemoryTests : IClassFixture<WarmupUnifiedContext>
    {
        private readonly ITestOutputHelper _output;
        private readonly UnifiedContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private readonly IEmailAppService _emailAppService;
        public WarmupUnifiedContext UnifiedContextData { get; }

        public EmailAppServiceInMemoryTests(WarmupUnifiedContext unifiedContext, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            UnifiedContextData = unifiedContext;
            _emailAppService = UnifiedContextData.Services.GetRequiredService<IEmailAppService>();
            _database = UnifiedContextData.Services.GetRequiredService<UnifiedContext>();
            _notifications = (DomainNotificationHandler)UnifiedContextData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }

        [Fact]
        public async Task Should_Save_Email()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Emails.FirstOrDefault(f => f.Type == command.Type).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Save_Email_With_Many_Bccs()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            var emailBcc = _faker.Person.Email;
            command.Bcc = $"{emailBcc};{_faker.Internet.Email()};{_faker.Internet.Email()};{_faker.Internet.ExampleEmail()}";

            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
        }

        [Fact]
        public async Task Should_Save_Email_With_Null_Bcc()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();

            command.Bcc = null;

            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            var email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);
            email.Should().NotBeNull();
        }


        [Fact]
        public async Task Should_Update_Email_With_Null_Bcc()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();

            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            var email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);
            email.Should().NotBeNull();

            command.Bcc = new BlindCarbonCopy();
            command.Content = _faker.Lorem.Paragraphs();

            await _emailAppService.SaveEmail(command);
            email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);
            email.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Find_Email_By_Type()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            await _emailAppService.SaveEmail(command);

            var email = await _emailAppService.FindByType(command.Type);
            _database.Emails.FirstOrDefault(f => f.Type == command.Type).Should().NotBeNull();
            email.Should().NotBeNull();
            email.Sender.Should().NotBeNull();
            email.Sender.Name.Should().NotBeNull();
            email.Sender.Address.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Update_Email_When_Type_Already_Exists()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue();
            _database.Emails.FirstOrDefault(f => f.Type == command.Type).Should().NotBeNull();

            var newCommand = EmailFaker.GenerateEmailViewModel(command.Type).Generate();
            result = await _emailAppService.SaveEmail(newCommand);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Emails.Count(f => f.Type == newCommand.Type).Should().Be(1);
        }

        [Fact]
        public async Task Should_Save_Template()
        {
            var command = EmailFaker.GenerateTemplateViewModel().Generate();
            var result = await _emailAppService.SaveTemplate(command);

            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Templates.FirstOrDefault(f => f.Name == command.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Remove_Template()
        {
            var command = EmailFaker.GenerateTemplateViewModel().Generate();
            var result = await _emailAppService.SaveTemplate(command);

            result.Should().BeTrue();

            result = await _emailAppService.RemoveTemplate(command.Name);

            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Templates.FirstOrDefault(f => f.Name == command.Name).Should().BeNull();
        }
    }
}
