using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Campaigns;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity;

namespace Fanword.Business.Builders.Campaigns {
    public class CampaignBuilder {
        private ApplicationRepository _repo { get; set; }

        public CampaignBuilder(ApplicationRepository repo) {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Campaign, CampaignRecord>()
                    .ForMember(m => m.TeamList, m => m.MapFrom(mm => mm.Teams.Select(i => i.School.Name + "-" + i.Sport.Name)))
                    .ForMember(m => m.SportList, m => m.MapFrom(mm => mm.Sports.Select(i => i.Name)))
                    .ForMember(m => m.Profiles, m => m.Ignore())
                    .ForMember(m => m.SchoolList, m => m.MapFrom(mm => mm.Schools.Select(i => i.Name)));
            });
        }

        private MapperConfiguration MapQueries() {
            return new MapperConfiguration(config => {
                config.CreateMap<Campaign, CampaignViewModel>()
                    .ForMember(m => m.TeamIds, m => m.MapFrom(mm => mm.Teams.Select(i => i.Id)))
                    .ForMember(m => m.SchoolIds, m => m.MapFrom(mm => mm.Schools.Select(i => i.Id)))
                    .ForMember(m => m.SportIds, m => m.MapFrom(mm => mm.Sports.Select(i => i.Id)));
            });
        }

        private IMapper MapAdd() {
            return new MapperConfiguration(config => {
                config.CreateMap<CampaignViewModel, Campaign>()
                    .ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()))
                    .ForMember(m => m.StartUtc, m => m.MapFrom(mm => mm.StartUtc.Value.ToUniversalTime()))
                    .ForMember(m => m.EndUtc, m => m.MapFrom(mm => mm.EndUtc.Value.ToUniversalTime()));
            }).CreateMapper();
        }

        private IMapper MapUpdate() {
            return new MapperConfiguration(config => {
                config.CreateMap<CampaignViewModel, Campaign>()
                    .ForMember(m => m.Sports, m => m.UseDestinationValue())
                    .ForMember(m => m.Teams, m => m.UseDestinationValue())
                    .ForMember(m => m.Schools, m => m.UseDestinationValue())
                    .ForMember(m => m.StartUtc, m => m.MapFrom(mm => mm.StartUtc.Value.ToUniversalTime()))
                    .ForMember(m => m.EndUtc, m => m.MapFrom(mm => mm.EndUtc.Value.ToUniversalTime()));
            }).CreateMapper();
        }

        public async Task<List<CampaignRecord>> BuildGridAsync(string advertiserId = null) {
            if (String.IsNullOrEmpty(advertiserId) && ClaimsPrincipal.Current.IsInRole(AppRoles.SystemAdmin)) {
                //get all
                return (await _repo.Campaigns.AsQueryable().ProjectTo<CampaignRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
            if (String.IsNullOrEmpty(advertiserId) && ClaimsPrincipal.Current.IsInRole("Advertiser")) {
                //return mine
                var myId = ClaimsPrincipal.Current.Identity.GetUserId();
                var advertiser = await _repo.UserManager.Users.Where(i => i.Id == myId).Select(m => m.AdvertiserId).FirstOrDefaultAsync();
                return (await _repo.Campaigns.Where(m => m.AdvertiserId == advertiser).ProjectTo<CampaignRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
            }
            return (await _repo.Campaigns.Where(m => m.AdvertiserId == advertiserId).ProjectTo<CampaignRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
        }


        public async Task<CampaignViewModel> BuildSingleAsync(string id) {
            return (await _repo.Campaigns.GetByIdQueryable(id).ProjectTo<CampaignViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
        }

        public async Task AddAsync(CampaignViewModel model) {
            var newItem = MapAdd().Map<Campaign>(model);
            if (ClaimsPrincipal.Current.IsInRole("Advertiser")) {
                //don't let them change this
                newItem.AdvertiserId = ClaimsPrincipal.Current.GetClaimValue(AppClaimTypes.AdvertiserId);
            }

            newItem.ImageAspectRatio = await GetImageAspectRatio(newItem.ImageUrl);
            newItem.Teams = await _repo.Teams.Where(i => model.TeamIds.Contains(i.Id)).ToListAsync();
            newItem.Schools = await _repo.Schools.Where(m => model.SchoolIds.Contains(m.Id)).ToListAsync();
            newItem.Sports = await _repo.Sports.Where(m => model.SportIds.Contains(m.Id)).ToListAsync();
            await _repo.Campaigns.AddOrUpdateAndSaveAsync(newItem);
        }

        public async Task UpdateAsync(CampaignViewModel model) {
            var dbItem = await _repo.Campaigns.GetByIdAsync(model.Id, query => query.Include(m => m.Schools).Include(m => m.Teams).Include(m => m.Sports));
            if (dbItem == null) return;

            dbItem = MapUpdate().Map(model, dbItem);
            if (ClaimsPrincipal.Current.IsInRole("Advertiser") && !ClaimsPrincipal.Current.IsInRole(AppRoles.SystemAdmin)) {
                dbItem.AdvertiserId = ClaimsPrincipal.Current.GetClaimValue(AppClaimTypes.AdvertiserId);
            }

            dbItem.Sports?.Clear();
            dbItem.Schools?.Clear();
            dbItem.Teams?.Clear();

            dbItem.ImageAspectRatio = await GetImageAspectRatio(dbItem.ImageUrl);


            dbItem.Teams = await _repo.Teams.Where(i => model.TeamIds.Contains(i.Id)).ToListAsync();
            dbItem.Schools = await _repo.Schools.Where(m => model.SchoolIds.Contains(m.Id)).ToListAsync();
            dbItem.Sports = await _repo.Sports.Where(m => model.SportIds.Contains(m.Id)).ToListAsync();

            await _repo.SaveAsync();
        }

        async Task<float> GetImageAspectRatio(string url)
        {
            HttpClient client = new HttpClient();
            var stream = await client.GetStreamAsync(url);
            var image = System.Drawing.Image.FromStream(stream);
            var ratio = (float)image.Height / (float)image.Width;
            ratio = ratio <= 0 ? 0.5625f : ratio;
            return ratio;
        }
    }
}
