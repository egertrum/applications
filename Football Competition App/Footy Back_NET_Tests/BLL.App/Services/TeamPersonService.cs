using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;


namespace BLL.App.Services
{
    public class TeamPersonService: BaseEntityService<IAppUnitOfWork, ITeamPersonRepository, BLLAppDTO.TeamPerson, DALAppDTO.TeamPerson>, ITeamPersonService
    {
        public TeamPersonService(IAppUnitOfWork serviceUow, ITeamPersonRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TeamPersonMapper(mapper))
        {
        }
        
        public async Task<IEnumerable<BLLAppDTO.TeamPerson>> GetAllWithTeamId(Guid teamId)
        {
            return (await ServiceRepository.GetAllWithTeamId(teamId)).Select(x => Mapper.Map(x))!;
        }

        public async Task<bool> IsPossibleToAdd(DALAppDTO.TeamPerson teamPerson)
        {
            return await ServiceRepository.IsPossibleToAdd(teamPerson);
        }

        public async Task<BLLAppDTO.TeamPerson?> AddIfPossible(BLLAppDTO.TeamPerson teamPerson)
        {
            var dalTeamPerson = Mapper.Map(teamPerson);
            if (!await IsPossibleToAdd(dalTeamPerson!)) return null;

            var addedTeamPerson = ServiceRepository.Add(Mapper.Map(teamPerson)!);
            await ServiceUow.SaveChangesAsync();

            return Mapper.Map(addedTeamPerson);
        }

        public async Task<bool> TeamAndPersonBelongToUser(BLLAppDTO.TeamPerson teamPerson, Guid userId)
        {
            var team = await ServiceUow.Teams.FirstOrDefaultAsync(teamPerson.TeamId, userId);
            var person = await ServiceUow.Persons.FirstOrDefaultAsync(teamPerson.PersonId, userId);

            return team != null && person != null;
        }
    }

}