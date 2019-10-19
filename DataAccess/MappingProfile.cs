using System;
using AutoMapper;
using ConnectOnCommuteBackend.Models;

namespace ConnectOnCommuteBackend.DataAccess
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignupRequest, Account>();
        }
    }
}


