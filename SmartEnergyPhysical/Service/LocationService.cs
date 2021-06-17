using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;
using SmartEnergy.Physical.Infrastructure;
using System.Threading.Tasks;

namespace SmartEnergy.Physical.Service
{
    public class LocationService : ILocationService
    {
        private readonly PhysicalDbContext _dbContext;
        private readonly IMapper _mapper;

        public LocationService(PhysicalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<LocationDto> GetAllLocations()
        {
            return _mapper.Map<List<LocationDto>>(_dbContext.Location.ToList());
        }

        public LocationDto GetByIdAsync(int id)
        {
            return _mapper.Map<LocationDto>(_dbContext.Location.Find(id));
        }
    }
}
