﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if(region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            // Request(DTO) to Domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area= addRegionRequest.Area,
                Lat= addRegionRequest.Lat,
                Long= addRegionRequest.Long,
                Name= addRegionRequest.Name,
                Population  = addRegionRequest.Population
            };

            region = await regionRepository.AddAsync(region);
            var regionDTO = new Models.DTO.Region
            {
                Id= region.Id, 
                Code  = region.Code,
                Area= region.Area,
                Lat= region.Lat,
                Long= region.Long,
                Name = region.Name,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new {id = region.Id}, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);
            
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            // return ok response
            return Ok(regionDTO);
        }
       
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            // convert DTO to Domain model
            var region = new Models.Domain.Region
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };

            region = await regionRepository.UpdateAsync(id, region);

            if(region == null) { return NotFound(); }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }
    }
}
