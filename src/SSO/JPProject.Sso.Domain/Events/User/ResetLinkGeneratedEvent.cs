﻿using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class ResetLinkGeneratedEvent : Event
    {
        public string Email { get; }
        public string Username { get; }

        public ResetLinkGeneratedEvent(string email, string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Email = email;
            Username = username;
        }
    }
}