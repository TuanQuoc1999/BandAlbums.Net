using AutoMapper;
using BandAPI.Helpers;
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
    [Route("api/bands")]
    public class BandsController : ControllerBase
    {
        private readonly IBandAlbumResponsitory _bandAlbumResponsitory;
        private readonly IMapper _mapper;

        public BandsController(IBandAlbumResponsitory bandAlbumResponsitory, IMapper mapper)
        {
            _bandAlbumResponsitory = bandAlbumResponsitory ??
                throw new ArgumentNullException(nameof(bandAlbumResponsitory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<BandDto>> GetBands([FromQuery] BandResourceParameters bandResourceParameters)
        {
            var bandsFromRepo = _bandAlbumResponsitory.GetBands(bandResourceParameters);
            return Ok(_mapper.Map<IEnumerable<BandDto>>(bandsFromRepo));
        }

        [HttpGet("{bandId}", Name ="GetBand")]
        public IActionResult GetBand(Guid bandId)
        {
            var bandFromRepo = _bandAlbumResponsitory.GetBand(bandId);
            if (bandFromRepo == null)
                return NotFound();

            return Ok(bandFromRepo);
        }

        [HttpPost]
        public ActionResult<BandDto> CreateBand([FromBody] BandForCreatingDto band)
        {
            var bandEntity = _mapper.Map<entities.Band>(band);
            _bandAlbumResponsitory.AddBand(bandEntity);
            _bandAlbumResponsitory.Save();

            var bandToReturn = _mapper.Map<BandDto>(bandEntity);

            return CreatedAtRoute("GetBand", new { bandId = bandToReturn.Id },bandToReturn);
        }

        [HttpOptions]
        public IActionResult GetBandsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,DELETE,HEAD,OPTIONS");
            return Ok();
        }
    }
}
