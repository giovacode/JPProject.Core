using JPProject.Admin.Domain.Commands.Clients;

namespace JPProject.Admin.Domain.Validations.Client
{
    public class SaveClientPropertyCommandValidation : ClientPropertyValidation<SaveClientPropertyCommand>
    {
        public SaveClientPropertyCommandValidation()
        {
            ValidateClientId();
            ValidateKey();
            ValidateValue();
        }
    }
}