using eclipseworks.Business.UseCases.Validators;

namespace eclipseworks.Business.Contracts
{
    public interface IValidators
    {
        ResultValidation Validate(object entity);
    }
}
