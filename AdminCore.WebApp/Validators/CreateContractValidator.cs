using AdminCore.Common.Exceptions;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs;
using AdminCore.WebApi.Models.Base;
using AdminCore.WebApi.Models.Contract;
using AdminCore.WebApi.Validators.Base;
using AutoMapper;

namespace AdminCore.WebApi.Validators
{
  public class CreateContractValidator : ViewModelValidator
  {
    private readonly IContractService _contractService;
    private readonly IMapper _mapper;

    public CreateContractValidator(IContractService contractService, IMapper mapper) : base(typeof(CreateContractViewModel))
    {
      _contractService = contractService;
      _mapper = mapper;
    }

    protected override void RunValidation(ViewModel viewModel)
    {
      var contractDto = _mapper.Map<ContractDto>(viewModel);
      if (_contractService.ContractAlreadyExists(contractDto))
      {
        throw new ValidationException("The user already has a contract for this team during the given period.");
      }
    }
  }
}