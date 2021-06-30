using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses;
using Fanword.Business.ViewModels.Events;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using System.Globalization;

namespace Fanword.Business.Builders.Events
{
    public class EventBuilder
    {
        private ApplicationRepository _repo { get; set; }

        public EventBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        private MapperConfiguration MapGridQueries()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Event, EventRecord>()
                    .ForMember(m => m.Teams, m => m.Ignore())
                    .ForMember(m => m.TeamNames, m => m.MapFrom(mm => mm.EventTeams.Where(i => i.Team.DateDeletedUtc == null).OrderBy(i => i.DateCreatedUtc).Select(i => i.Team.Nickname)))
                    .ForMember(m => m.TeamIds, m => m.MapFrom(mm => mm.EventTeams.Where(i => i.Team.IsActive && i.Team.DateDeletedUtc == null).Select(i => i.TeamId)))
                    .ForMember(m => m.Sport, m => m.MapFrom(mm => mm.Sport.Name));
            });
        }

        private MapperConfiguration MapQueries()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<Event, EventViewModel>()
                    .ForMember(m => m.SportDisplay, m => m.MapFrom(mm => mm.Sport.Name))
                    .ForMember(m => m.ShowEventTeams, m => m.MapFrom(mm => false))
                    .ForMember(m => m.EditEvent, m => m.MapFrom(mm => false))
                    .ForMember(m => m.IsDeleted, m => m.MapFrom(mm => false));
                config.CreateMap<EventTeam, EventTeamViewModel>();
                config.CreateMap<EventManagementPin, EventManagementPinViewModel>();
            });
        }


        private MapperConfiguration MapAdd(string eventId = null)
        {
            return new MapperConfiguration(config =>
            {
                var id = String.IsNullOrEmpty(eventId) ? Guid.NewGuid().ToString() : eventId;
                config.CreateMap<EventViewModel, Event>()
                    .ForMember(m => m.Id, m => m.UseValue(id));

                config.CreateMap<EventTeamViewModel, EventTeam>()
                    //.ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()))
                    .ForMember(m => m.EventId, m => m.MapFrom(mm => id));
            });
        }

        private MapperConfiguration MapAddPin()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<EventManagementPinViewModel, EventManagementPin>()
                    .ForMember(m => m.Id, m => m.MapFrom(mm => Guid.NewGuid().ToString()))
                    .ForMember(m => m.DateCreatedUtc, m => m.MapFrom(mm => DateTime.UtcNow))
                    .ForMember(m => m.IsActive, m => m.UseValue(true));
            });
        }

        private MapperConfiguration MapUpdatePin()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<EventManagementPinViewModel, EventManagementPin>();
            });
        }

        private MapperConfiguration MapUpdateEventTeam()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<EventTeamViewModel, EventTeam>()
                    .ForMember(m => m.Id, m => m.UseDestinationValue())
                    .ForMember(m => m.EventId, m => m.UseDestinationValue())
                    .ForMember(m => m.Team, m => m.Ignore())
                    .ForMember(m => m.Event, m => m.Ignore());
            });
        }

        private MapperConfiguration MapUpdateEvent()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<EventViewModel, Event>()
                    .ForMember(m => m.Id, m => m.UseDestinationValue())
                    .ForMember(m => m.DateCreatedUtc, m => m.UseDestinationValue())
                    .ForMember(m => m.Sport, m => m.Ignore())
                    .ForMember(m => m.EventTeams, m => m.UseDestinationValue());
            });
        }


        public async Task<List<EventRecord>> BuildGridAsync()
        {

            var events = await _repo.Events.AsQueryable().ProjectTo<EventRecord>(MapGridQueries()).OrderByDescending(m => m.DateOfEventUtc).ToListAsync();

            //return event dates in the time zone saved. 
            foreach (var item in events)
            {
                if (item != null)
                {
                    item.DateOfEventInTimezone = TimeZoneInfo.ConvertTimeFromUtc(item.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(item.TimezoneId));
                    item.TimeOfEventInTimezone = item.DateOfEventInTimezone.ToShortTimeString();
                }
            }
            return events;
            //return (await _repo.Events.AsQueryable().ProjectTo<EventRecord>(MapGridQueries()).ToListAsync()).SpecifyDateTimeKind();
        }

        //public async Task<EventManagementViewModel> BuildManagement1GridAsync(EventFilter filters)
        //{
        //    var model = new EventManagementViewModel();
        //    var query = _repo.Events.AsQueryable().Include(m => m.EventTeams);

        //    if (!string.IsNullOrEmpty(filters.SportId))
        //    {
        //        query = query.Where(m => m.SportId == filters.SportId);
        //    }

        //    if (!string.IsNullOrEmpty(filters.TeamId))
        //    {
        //        query = query.Where(m => m.EventTeams.Select(i => i.TeamId).Contains(filters.TeamId));
        //    }

        //    if (!string.IsNullOrEmpty(filters.StartDate))
        //    {
                
        //        filters.StartDateUtc = DateTime.Parse(filters.StartDate, null, DateTimeStyles.RoundtripKind);

        //        query = query.Where(m => m.DateOfEventUtc >= filters.StartDateUtc);
        //     }

        //    if (!string.IsNullOrEmpty(filters.EndDate))
        //    {
        //        filters.EndDateUtc = DateTime.Parse(filters.EndDate, null, DateTimeStyles.RoundtripKind);
        //        //filters.EndDateUtc = TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Parse(filters.EndDate));
        //        query = query.Where(m => m.DateOfEventUtc <= filters.EndDateUtc);
        //    }
        //    model.Events = await query.OrderByDescending(i => i.DateOfEventUtc).Take(100).ProjectTo<EventViewModel>(MapQueries()).ToListAsync();

        //    foreach (var item in model.Events)
        //    {

        //        if (item != null)
        //        {
        //            item.DateOfEventInTimezone = TimeZoneInfo.ConvertTimeFromUtc(item.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(item.TimezoneId));
        //            item.EventTeams = item.EventTeams.OrderBy(m => m.DateCreatedUtc).ToList();
        //        }
        //    }

        //    return model;
        //}


        public async Task<EventManagementViewModel> BuildManagementGridAsync(EventFilter filters)
        {
            try
            {
                var model = new EventManagementViewModel();
                var query = _repo.Events.AsQueryable().Include(m => m.EventTeams).Include(m=>m.Sport);
                var nQuery = await query.ToListAsync();
                

                if (!string.IsNullOrEmpty(filters.SportId))
                {
                    nQuery = nQuery.Where(m => m.SportId == filters.SportId).ToList();
                }

                if (!string.IsNullOrEmpty(filters.TeamId))
                {
                    nQuery = nQuery.Where(m => m.EventTeams.Select(i => i.TeamId).Contains(filters.TeamId)).ToList();
                }

                if (!string.IsNullOrEmpty(filters.StartDate))
                {

                    filters.StartDateUtc = DateTime.ParseExact(filters.StartDate.Substring(0, 24),
                                      "ddd MMM dd yyyy HH:mm:ss",
                                      CultureInfo.InvariantCulture);

                    nQuery = (from q in nQuery
                              where TimeZoneInfo.ConvertTimeFromUtc(q.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(q.TimezoneId)) >= filters.StartDateUtc
                              select q).ToList();
                }

                if (!string.IsNullOrEmpty(filters.EndDate))
                {
                    filters.EndDateUtc = DateTime.ParseExact(filters.EndDate.Substring(0, 24),
                                    "ddd MMM dd yyyy HH:mm:ss",
                                    CultureInfo.InvariantCulture);
                    nQuery = (from q in nQuery
                              where TimeZoneInfo.ConvertTimeFromUtc(q.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(q.TimezoneId)) <= filters.EndDateUtc
                              select q).ToList();
                }
                
                var mapper = MapQueries().CreateMapper();
                model.Events =   mapper.Map<IEnumerable<Event>, List<EventViewModel>>(nQuery.OrderByDescending(i => i.DateOfEventUtc).Take(100).ToList());

                foreach (var item in model.Events)
                {

                    if (item != null)
                    {
                        item.DateOfEventInTimezone = TimeZoneInfo.ConvertTimeFromUtc(item.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(item.TimezoneId));
                        item.EventTeams = item.EventTeams.OrderBy(m => m.DateCreatedUtc).ToList();
                    }
                }

                


                return model;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool ConvertUTC(DateTime date, string TimezoneId, DateTime? SearchDate)
        {
            return (TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(TimezoneId)) >= SearchDate);
        }

        public async Task<EventViewModel> BuildSingleAsync(string id)
        {
            var item = (await _repo.Events.GetByIdQueryable(id).ProjectTo<EventViewModel>(MapQueries()).FirstOrDefaultAsync())?.SpecifyDateTimeKind();
            item.EventTeams = item.EventTeams.OrderBy(m => m.DateCreatedUtc).ToList();
            if (item != null)
            {
                item.DateOfEventInTimezone = TimeZoneInfo.ConvertTimeFromUtc(item.DateOfEventUtc, TimeZoneInfo.FindSystemTimeZoneById(item.TimezoneId));
            }
            return item;
        }

        public async Task<EventManagementPinViewModel> BuildEventMangementPinAsync()
        {
            var model = new EventManagementPinViewModel { IsActive = true };
            var query = _repo.EventManagementPins.Where(m => m.IsActive);
            if (query.FirstOrDefault() != null)
            {
                model = (await query.ProjectTo<EventManagementPinViewModel>(MapQueries()).FirstOrDefaultAsync());
            }
            return model;
        }


        public async Task AddAsync(EventViewModel model)
        {
            if (model.IsDeleted)
            {
                return;
            }

            //remove deleted teams
            model.EventTeams = model.EventTeams.Where(i => !i.IsDeleted).ToList();

            var add = MapAdd().CreateMapper().Map<Event>(model);
            if ((add.Name == null || add.Name == "") && add.EventTeams != null)
            {
                List<string> nameList = new List<string>();
                List<string> teamList = add.EventTeams.Select(i => i.TeamId).ToList();
                foreach (var team in teamList)
                {
                    var id = await _repo.Teams.Where(m => m.Id == team).FirstOrDefaultAsync();
                    nameList.Add(id.Nickname);
                }

                add.Name = String.Join(" - ", nameList);
            }
            await _repo.Events.AddOrUpdateAndSaveAsync(add);
        }

        public async Task AddOrUpdateAsync(List<EventViewModel> model)
        {

            foreach (var eventModel in model)
            {

                try
                {
                    var dbEvent = await _repo.Events.GetByIdAsync(eventModel.Id);
                    if (dbEvent == null)
                    {
                        await AddAsync(eventModel);
                    }
                    else
                    {
                        await UpdateAsync(eventModel);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public async Task<EventManagementPinViewModel> AddOrUpdatePinAsync(EventManagementPinViewModel model)
        {
            var dbPin = await _repo.EventManagementPins.GetByIdAsync(model.Id);
            var addPin = new EventManagementPin();
            if (dbPin == null)
            {
                addPin = MapAddPin().CreateMapper().Map<EventManagementPin>(model);
            }
            else
            {
                addPin = MapUpdatePin().CreateMapper().Map<EventManagementPin>(model);
            }
            await _repo.EventManagementPins.AddOrUpdateAndSaveAsync(addPin);

            return MapQueries().CreateMapper().Map<EventManagementPinViewModel>(addPin);
        }

        public async Task UpdateAsync(EventViewModel model)
        {
            if (model.IsDeleted)
            {
                await _repo.Events.DeleteAndSaveAsync(model.Id);
                var eventTeams = _repo.EventTeams.Where(m => m.EventId == model.Id).ToList();
                await _repo.EventTeams.DeleteAndSaveAsync(eventTeams);
                return;
            }

            var dbEvent = await _repo.Events.GetByIdAsync(model.Id, query => query.Include(i => i.EventTeams));
            if (dbEvent == null) return;

            if ((model.Name == null || model.Name == "") && model.EventTeams != null)
            {
                List<string> nameList = new List<string>();
                List<string> teamList = model.EventTeams.Select(i => i.TeamId).ToList();
                foreach (var team in teamList)
                {
                    var id = await _repo.Teams.Where(m => m.Id == team).FirstOrDefaultAsync();
                    nameList.Add(id.Nickname);
                }

                model.Name = String.Join(" - ", nameList);
            }

            //assign manually because the mapper removes child object tracking
            dbEvent.Name = model.Name;
            dbEvent.SportId = model.SportId;
            dbEvent.TimezoneId = model.TimezoneId;
            dbEvent.DateOfEventUtc = model.DateOfEventUtc;
            dbEvent.Location = model.Location;
            dbEvent.PurchaseTicketsUrl = model.PurchaseTicketsUrl;

            //END OF MANUAL ASSIGNMENT//

            var deleted = model.EventTeams.Where(i => i.IsDeleted).ToList();

            foreach (var item in deleted)
            {
                _repo.EventTeams.Delete(item.Id);
            }

            //add or update teams
            foreach (var entry in model.EventTeams.Where(i => !i.IsDeleted))
            {
                var dbEntry = dbEvent.EventTeams.FirstOrDefault(i => i.Id == entry.Id);
                if (dbEntry != null)
                {
                    MapUpdateEventTeam().CreateMapper().Map(entry, dbEntry);
                }
                else
                {
                    dbEvent.EventTeams.Add(MapAdd().CreateMapper().Map<EventTeam>(entry));
                }
            }
            await _repo.SaveAsync();

        }


    }
}
