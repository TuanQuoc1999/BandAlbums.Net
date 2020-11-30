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

        [HttpGet("({ids})", Name ="GetBandCollection")]
        public IActionResult GetBandCollection([FromRoute] 
               [ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();
            var bandEntities = _bandAlbumResponsitory.GetBands(ids);

            if (ids.Count() != bandEntities.Count())
                return NotFound();

            var bandToReturn = _mapper.Map<IEnumerable<BandDto>>(bandEntities);

            return Ok(bandToReturn);
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
            var bandCollectionToReturn = _mapper.Map<IEnumerable<BandDto>>(bandEntities);
            var IdsString = string.Join(",", bandCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetBandCollection", new { ids = IdsString }, bandCollectionToReturn);
        }
    }
}
