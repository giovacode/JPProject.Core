using FluentValidation.Results;
using IdentityServer4.Models;
using JPProject.Admin.Domain.Validations.Client;

namespace JPProject.Admin.Domain.Commands.Clients
{
    public class UpdateClientCommand : ClientCommand
    {
        public UpdateClientCommand(Client client, string oldClientId)
        {
            OldClientId = oldClientId;
            this.Client = client;
        }


        public override bool IsValid()
        {
            ValidationResult = new UpdateClientCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}