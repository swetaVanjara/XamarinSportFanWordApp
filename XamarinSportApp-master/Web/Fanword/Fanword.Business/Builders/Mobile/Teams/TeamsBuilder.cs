using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Fanword.Data.Entities;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.Teams
{
    public class TeamsBuilder
    {
        private ApplicationRepository _repo { get; set; }

        public TeamsBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<AthleteTeamSearch> TeamsForAthleteSearchAsync(string text)
        {
            var terms = text.ToLower();
            /*
            var entityParent = new EntityParent();
            entityParent.EntityChild.Test = "YOLO";
            var poco = entityParent.Map<Poco>();

            int i = 3;*/
            var teams = await _repo.Teams.Where(m => m.School.Name.ToLower().Contains(terms) || m.Sport.Name.ToLower().Contains(terms) || m.School.Nickname.ToLower().Contains(terms)).Select(m =>
                new AthleteTeam()
                {
                    Id = m.Id,
                    ProfilePublicUrl = m.ProfilePublicUrl,
                    SchoolName = m.School.Name,
                    SportName = m.Sport.Name,
                }
            ).ToListAsync();

            return new AthleteTeamSearch() { AthleteTeams = teams, SearchText = text };
        }

        public class EntityParent
        {
            public string TestTest;
            public EntityChild EntityChild { get; set; }

            public EntityParent()
            {
                EntityChild = new EntityChild();
            }
        }

        public class EntityChild
        {
            public string Test { get; set; }
        }

        public class Poco
        {
            public string EntityChildTest { get; set; }
            public string TestTest { get; set; }

        }
    }

    public static class KringsExtensions
    {
        /*
        public static TDestination Map<TDestination>(this object source)
        {
            var sourceType = source.GetType();
            var properties = sourceType.GetProperties();
            var destinationType = typeof(TDestination);
            var destinationProperties = destinationType.GetProperties();
            var includedProperties = properties.Except(destinationProperties).ToList();

            MapperConfigurationExpression cfg = new MapperConfigurationExpression();
            var something = cfg.CreateMap(source.GetType(), typeof(TDestination));
            foreach (var property in includedProperties)
            {
                var info = source.GetType().GetProperty(property.Name);
                var lamdaArg = Expression.Parameter(source.GetType());
                var propertyAccess = Expression.MakeMemberAccess(lamdaArg, info);
                
                var info2 = property.PropertyType.GetProperty("Test");
                var propertyAccess2 = Expression.MakeMemberAccess(propertyAccess, info2);

                var parameterType = Expression.Parameter(typeof(object));

                var final = Expression.Lambda<Func<object, object>>(propertyAccess2, parameterType);
                something.ForMember("EntityChildTest", m => m.MapFrom(final));
            }

            var config = new MapperConfiguration(cfg);
            return config.CreateMapper().Map<TDestination>(source);
        }*/
    }
   
}
