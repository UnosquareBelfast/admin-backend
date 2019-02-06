using AdminCore.Common.Exceptions;
using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Validators.Base
{
    public abstract class ViewModelValidator
    {
        private readonly Type _typeBeingValidated;

        protected ViewModelValidator(Type typeBeingValidated)
        {
            _typeBeingValidated = typeBeingValidated;
        }

        public void Validate(ViewModel viewModel)
        {
            CheckType(viewModel);
            RunValidation(viewModel);
        }

        private void CheckType(ViewModel viewModel)
        {
            if(!_typeBeingValidated.IsInstanceOfType(viewModel))
            {
                throw new ValidationException($"Type {viewModel.GetType()} cannot be used by validator of type {GetType()}");
            }
        }

        protected abstract void RunValidation(ViewModel viewModel);
    }
}
