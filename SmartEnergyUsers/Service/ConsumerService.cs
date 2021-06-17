using AutoMapper;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Interfaces;
using SmartEnergy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SmartEnergy.Users.Service
{
    public class ConsumerService : IConsumerService
    {

        private readonly UsersDbContext _dbContext;
        private readonly IMapper _mapper;

        public ConsumerService(UsersDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ConsumerDto> GetAsync(int id)
        {
            return _mapper.Map<ConsumerDto>(_dbContext.Consumers.Find(id));
        }

        public async Task<List<ConsumerDto>> GetAllAsync()
        {
            return  _mapper.Map<List<ConsumerDto>>(_dbContext.Consumers.ToList());

        }

        public async Task< ConsumerDto> InsertAsync(ConsumerDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ConsumerDto> UpdateAsync(ConsumerDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
