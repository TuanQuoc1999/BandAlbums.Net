using AutoMapper;
using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bands/{bandId}/albums")]
    [ResponseCache(CacheProfileName = "90SecondsCacheProfile")]
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
        [ResponseCache(Duration =120)]
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

        [HttpPut("{albumId}")]
        public IActionResult UpdateAlbumForBand(Guid bandId, Guid albumId, 
            [FromBody] AlbumForUpdatingDto album)
        {
            if (!_bandAlbumResponsitory.BandExists(bandId))
                return NotFound();

            var albumFromRepo = _bandAlbumResponsitory.GetAlbum(bandId, albumId);
            if (albumFromRepo == null)
            {
                var albumToAdd = _mapper.Map<entities.Album>(album);
                albumToAdd.Id = albumId;
                _bandAlbumResponsitory.AddAlbum(bandId, albumToAdd);
                _bandAlbumResponsitory.Save();

                var albumToReturn = _mapper.Map<AlbumDto>(albumToAdd);

                return CreatedAtRoute("GetAlbumForBand", new { bandId = bandId, albumId = albumToReturn.Id },
                    albumToReturn);
            }

            _mapper.Map(album, albumFromRepo);
            _bandAlbumResponsitory.UpdateAlbum(albumFromRepo);
            _bandAlbumResponsitory.Save();

            return NoContent();
        }

        [HttpPatch]
        public ActionResult PartiallyUpdateAlbumForBand(Guid bandId, Guid albumId,[FromBody] JsonPatchDocument<AlbumForUpdatingDto> patchDocument)
        {
            if (!_bandAlbumResponsitory.BandExists(bandId))
                return NotFound();
            var albumFromRepo = _bandAlbumResponsitory.GetAlbum(bandId, albumId);
            if (albumFromRepo == null)
            {
                var albumDto = new AlbumForUpdatingDto();
                patchDocument.ApplyTo(albumDto);
                var albumToAdd = _mapper.Map<entities.Album>(albumDto);
                albumToAdd.Id = albumId;

                _bandAlbumResponsitory.AddAlbum(bandId, albumToAdd);
                _bandAlbumResponsitory.Save();

                var albumToReturn = _mapper.Map<AlbumDto>(albumToAdd);

                return CreatedAtRoute("GetAlbumForBand", new { bandId = bandId, albumId = albumToReturn.Id }, albumToReturn);
            }

            var albumToPatch = _mapper.Map<AlbumForUpdatingDto>(albumFromRepo);
            patchDocument.ApplyTo(albumToPatch, ModelState);

            if (!TryValidateModel(albumToPatch))
                return ValidationProblem(ModelState);
            _mapper.Map(albumToPatch, albumFromRepo);
            _bandAlbumResponsitory.UpdateAlbum(albumFromRepo);
            _bandAlbumResponsitory.Save();

            return NoContent();
        }

        [HttpDelete("{albumId}")]
        public ActionResult DeleteAlbumForBand(Guid bandId, Guid albumId)
        {
            if (!_bandAlbumResponsitory.BandExists(bandId))
                return NotFound();

            var albumFromRepo = _bandAlbumResponsitory.GetAlbum(bandId, albumId);
            if (albumFromRepo == null)
                return NotFound();

            _bandAlbumResponsitory.DeleteAlbum(albumFromRepo);
            _bandAlbumResponsitory.Save();

            return NoContent();
        }
    }
}
