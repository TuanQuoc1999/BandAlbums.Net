using AutoMapper;
using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bands/{bandId}/albums")]
    public class AlbumsController :ControllerBase
    {
        private readonly IBandAlbumResponsitory _bandAlbumResponsitory;
        private readonly IMapper _mapper;

        public AlbumsController(IBandAlbumResponsitory bandAlbumResponsitory, IMapper mapper)
        {
            _bandAlbumResponsitory = bandAlbumResponsitory ??
                throw new ArgumentNullException(nameof(bandAlbumResponsitory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlbumDto>> GetAlbumsForBand(Guid bandId)
        {
            if (!_bandAlbumResponsitory.BandExists(bandId))
                return NotFound();

            var albumsFromRepo = _bandAlbumResponsitory.GetAlbums(bandId);
            return Ok(_mapper.Map<IEnumerable<AlbumDto>>(albumsFromRepo));
        }

        [HttpGet("{albumId}", Name = "GetAlbumForBand")]
        public ActionResult<AlbumDto> GetAlbumForBand(Guid bandId, Guid albumId)
        {
            if (!_bandAlbumResponsitory.BandExists(bandId))
                return NotFound();

            var albumFromRepo = _bandAlbumResponsitory.GetAlbum(bandId, albumId);
            if (albumFromRepo == null)
                return NotFound();
            return Ok(_mapper.Map<AlbumDto>(albumFromRepo));
        }

        [HttpPost]
        public ActionResult<AlbumDto> CreateAlbumForBand(Guid bandId, [FromBody] AlbumCreatingDto album)
        {
            if (!_bandAlbumResponsitory.BandExists(bandId))
                return NotFound();

            var albumEntity = _mapper.Map<entities.Album>(album);
            _bandAlbumResponsitory.AddAlbum(bandId, albumEntity);
            _bandAlbumResponsitory.Save();

            var albumToReturn = _mapper.Map<AlbumDto>(albumEntity);
            return CreatedAtRoute("GetAlbumForBand", new { bandId = bandId, albumId = albumToReturn.Id }, albumToReturn);
        }
    }
}
