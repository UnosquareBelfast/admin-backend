using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Client;
using AdminCore.Services.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using AdminCore.Constants.Enums;

namespace AdminCore.Services
{
  public class ClientService : BaseService, IClientService
  {
    private readonly IMapper _mapper;

    public ClientService(IDatabaseContext databaseContext, IMapper mapper)
      : base(databaseContext)
    {
      _mapper = mapper;
    }

    public IList<ClientDto> GetAll()
    {
      var clients = DatabaseContext.ClientRepository.Get();
      return _mapper.Map<IList<ClientDto>>(clients);
    }

    public ClientDto Save(ClientDto clientDto)
    {
      Client client;
      if (clientDto.ClientId == 0)
      {
        client = _mapper.Map<Client>(clientDto);

        client.SystemUser = new SystemUser {SystemUserRoleId = (int)SystemUserRoles.Client};

        DatabaseContext.ClientRepository.Insert(client);
      }
      else
      {
        client = GetById(clientDto.ClientId);
        client.ClientName = clientDto.ClientName;
      }

      DatabaseContext.SaveChanges();
      return _mapper.Map<ClientDto>(client);
    }

    public void Delete(int id)
    {
      var clientToDelete = DatabaseContext.ClientRepository.GetSingle(x => x.ClientId == id);
      if (clientToDelete != null)
      {
        DatabaseContext.ClientRepository.Delete(clientToDelete);
        DatabaseContext.SaveChanges();
      }
    }

    public ClientDto GetByClientId(int id)
    {
      var client = GetById(id);
      return _mapper.Map<ClientDto>(client);
    }

    public IList<ClientDto> GetByClientName(string clientName)
    {
      var client = DatabaseContext.ClientRepository.Get(x =>
        x.ClientName.Equals(clientName, StringComparison.CurrentCultureIgnoreCase));
      return _mapper.Map<IList<ClientDto>>(client);
    }

    private Client GetById(int id)
    {
      return DatabaseContext.ClientRepository.GetSingle(x => x.ClientId == id);
    }
  }
}
