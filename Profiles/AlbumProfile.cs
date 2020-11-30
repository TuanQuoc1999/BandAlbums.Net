using AutoMapper;
using BandAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Profiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<entities.Album, Models.AlbumDto>().ReverseMap();
            CreateMap<AlbumCreatingDto, entities.Album>();
            CreateMap<Models.AlbumForUpdatingDto, entities.Album>().ReverseMap();
        }
    }
}
