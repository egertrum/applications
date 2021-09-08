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
    public class PersonService: BaseEntityService<IAppUnitOfWork, IPersonRepository, BLLAppDTO.Person, DALAppDTO.Person>, IPersonService
    {
        public PersonService(IAppUnitOfWork serviceUow, IPersonRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new PersonMapper(mapper) )
        {
        }
        
        public async Task<BLLAppDTO.Person?> FindByIdentificationCode(string idCode, Guid userId,
            bool noTracking = true)
        {
            var res = Mapper.Map(await ServiceRepository.FindByIdentificationCode(idCode, userId, noTracking));
            return res;
        }

        public async Task<bool> ExistsByIdentificationCode(string idCode, Guid userId, bool noTracking = true)
        {
            return await ServiceRepository.ExistsByIdentificationCode(idCode, userId);
        }

        public async Task<BLLAppDTO.Person?> FirstOrDefaultAsyncWithTeams(Guid id, bool noTracking = true)
        {
            var person = Mapper.Map(await ServiceRepository.FirstOrDefaultAsyncWithTeams(id));
            if (person!.TeamPersons == null) return person;
            foreach (var teamPerson in person!.TeamPersons!)
            {
                if (person!.PersonTeams!.All(x => x.Key != teamPerson.Team!.Name))
                {
                    person!.PersonTeams!.Add(teamPerson.Team!.Name, 0);
                }
                //var gamesCount = ServiceUow.GamePersons.GetGamesCountForPlayer(teamPerson.PersonId);
                // 0 because not needed in app right now!
            }
            return person;
        }
    }

}