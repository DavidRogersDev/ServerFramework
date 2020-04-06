using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;

namespace KesselRunFramework.Core.Infrastructure.Mapping
{
    public class MapperProvider
    {
        private readonly Container _container;

        public MapperProvider(Container container)
        {
            _container = container;
        }

        public IMapper GetMapper(IEnumerable<Profile> profiles)
        {
            var mapperConfigurationExpression = new MapperConfigurationExpression();

            mapperConfigurationExpression.ConstructServicesUsing(_container.GetInstance);

            mapperConfigurationExpression.AddProfiles(profiles);

            var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression);

            //mc.AssertConfigurationIsValid();

            IMapper mapper = new Mapper(mapperConfiguration, t => _container.GetInstance(t));

            return mapper;
        }
    }
}
