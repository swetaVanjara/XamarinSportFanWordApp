using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Advertisers;
using Fanword.Business.ViewModels.Registration;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.Repository;

namespace Fanword.Business.Builders.Advertisers {
    public class AdvertiserBuilder {
        private ApplicationRepository _repo { get; set; }

        public AdvertiserBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Advertiser, AdvertiserRecord>()
                    .ForMember(m => m.NumberPendingCampaigns, m => m.MapFrom(mm => mm.Campaigns.Count(i => i.CampaignStatus == CampaignStatus.Pending && i.DateDeletedUtc == null)))
                    .ForMember(m => m.Name, m => m.MapFrom(mm => mm.CompanyName));
            });
        }

        private MapperConfiguration MapAdd() {
            return new MapperConfiguration(config => {
                config.CreateMap<AdvertiserRegistrationViewModel, Advertiser>()
                    .ForMember(m => m.ContactName, m => m.MapFrom(mm => mm.FirstName + " " + mm.LastName));
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Advertiser, AdvertiserViewModel>();
            });
        }

        private MapperConfiguration MapUpdate() {
            return new MapperConfiguration(config => {
                config.CreateMap<AdvertiserViewModel, Advertiser>();
            });
        }


        public async Task<List<AdvertiserRecord>> BuildGridAsync() {
            //return (await _repo.Advertisers.AsQueryable().Map<AdvertiserRecord>(MapGridQueries).ToListAsync()).SpecifyDateTimeKind();
            return (await _repo.Advertisers.AsQueryable().ProjectTo<AdvertiserRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
        }



        public async Task<AdvertiserViewModel> BuildSingleAsync(string id) {
            return (await _repo.Advertisers.GetByIdQueryable(id).ProjectTo<AdvertiserViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }

        public async Task<string> AddAsync(AdvertiserRegistrationViewModel model) {
            var dbAdvertiser = MapAdd().CreateMapper().Map<Advertiser>(model);			
				await _repo.Advertisers.AddOrUpdateAndSaveAsync(dbAdvertiser);		
			return dbAdvertiser.Id;
        }

        public async Task UpdateAsync(AdvertiserViewModel model) {
            var dbAdvertiser = await _repo.Advertisers.GetByIdAsync(model.Id);
            if (dbAdvertiser == null) return;
            dbAdvertiser = MapUpdate().CreateMapper().Map(model, dbAdvertiser);
            await _repo.SaveAsync();
        }
    }
}
