using JPProject.Admin.Domain.Commands.ApiResource;

namespace JPProject.Admin.Domain.Validations.ApiResource
{
    public class UpdateApiResourceCommandValidation : ApiResourceValidation<UpdateApiResourceCommand>
    {
        public UpdateApiResourceCommandValidation()
        {
            ValidateName();
        }
    }
}