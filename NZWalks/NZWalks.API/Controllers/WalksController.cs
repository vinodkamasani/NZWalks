using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            if(walkDTO == null)
            {
                return NotFound();
            }
            return Ok(walkDTO);
        }

        [HttpPost]
        [Route("[controller]")]
        public async Task<IActionResult> AddAsync(AddWalkRequest addWalkRequest)
        {
            var walk = new Models.Domain.Walk
            {
            Name= addWalkRequest.Name,
            Length=addWalkRequest.Length,
            WalkDifficultyId=addWalkRequest.WalkDifficultyId,
            RegionId=addWalkRequest.RegionId
            };

            walk = await walkRepository.AddAsync(walk);

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody] Models.DTO.UpdateWalkReqest updateWalkReqest)
        {
            var walk = new Models.Domain.Walk
            {
                Name = updateWalkReqest.Name,
                Length = updateWalkReqest.Length,
                RegionId = updateWalkReqest.RegionId,
                WalkDifficultyId = updateWalkReqest.WalkDifficultyId,
            };
            
            walk = await walkRepository.UpdateAsync(id, walk);

            if(walk == null)
            {
                return NotFound("Walk with this ID not found");
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
           return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);

            if(walk == null)
            {
                return NotFound("Record with this ID not found");
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);

        }
    }
}
