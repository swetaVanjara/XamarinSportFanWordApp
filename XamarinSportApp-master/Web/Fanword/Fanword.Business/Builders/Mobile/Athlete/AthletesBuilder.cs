using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionClasses.AutoMapper.Mappers;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile
{
    public class AthletesBuilder : BaseBuilder
    {
        public AthletesBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<Athlete> SaveAthleteAync(Athlete athlete, string userId)
        {
            athlete.ApplicationUserId = userId;
            var dbAthlete = athlete.Map<Data.Entities.Athelete>();
            await _repo.Atheletes.DeleteAndSaveAsync(_repo.Atheletes.Where(m => m.ApplicationUserId == userId).ToList());
            await _repo.Atheletes.AddOrUpdateAndSaveAsync(dbAthlete);
            return dbAthlete.Map<Athlete>();
        }

        public async Task DeleteAthleteAsync(string userId)
        {
            await _repo.Atheletes.DeleteAndSaveAsync(_repo.Atheletes.Where(m => m.ApplicationUserId == userId).ToList());
        }
    }
}
