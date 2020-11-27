using AutoMapper;
using BandAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Profiles
{
    public class BandProfile :Profile
    {
        public BandProfile()
        {
            CreateMap<entities.Band, Models.BandDto>()
                .ForMember(
                    dest => dest.FoundedYearsAgo,
                    opt => opt.MapFrom(src => $"{src.Founded.ToString("yyyy")} ({src.Founded.GetYearsAgo()}) years ago"));

            CreateMap<Models.BandForCreatingDto, entities.Band>();
        }
    }
}
