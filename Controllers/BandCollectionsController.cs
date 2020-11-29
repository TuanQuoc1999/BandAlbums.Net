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
    [Route("api/bandcollections")]
    public class BandCollectionsController :ControllerBase
    {
        private readonly IBandAlbumResponsitory _bandAlbumResponsitory;
        private readonly IMapper _mapper;

        public BandCollectionsController(IBandAlbumResponsitory bandAlbumResponsitory, IMapper mapper)
        {
            _bandAlbumResponsitory = bandAlbumResponsitory ??
                throw new ArgumentNullException(nameof(bandAlbumResponsitory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public ActionResult <IEnumerable<BandDto>> CreateBandCollection([FromBody] IEnumerable<BandForCreatingDto> bandCollection)
        {
            var bandEntities = _mapper.Map<IEnumerable<entities.Band>>(bandCollection);

            foreach(var band in bandEntities)
            {
                _bandAlbumResponsitory.AddBand(band);
            }

            _bandAlbumResponsitory.Save();

            return Ok();
        }
    }
}
