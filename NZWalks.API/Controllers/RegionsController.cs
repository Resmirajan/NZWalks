using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,
            IMapper mapper,ILogger<RegionsController>logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //Get all region--------------
        [HttpGet]
       // [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAllRegions Action method was invoked");
            
            var regionsDomain = await regionRepository.GetAllAsync();


            logger.LogInformation($"Finished getallregion request with  data:{JsonSerializer.Serialize(regionsDomain)}");
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //Search a single region--------------
        [HttpGet]
 //       [Authorize(Roles = "Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
           
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
           
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //Create a region------------------------
        [HttpPost]
        [ValidateModel]
  //      [Authorize(Roles = "Writer")]
        public async Task< IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           
                {
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
                //Use domain model to create region
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
                //map domain model back to dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                //return dto
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
                }
           

        }

        //Update a region---------------------
        [HttpPut]
        [ValidateModel]
 //       [Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        public async Task< IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                //map dto to domain
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //convert domain model to dto

                return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
            

        //Delete a region-----------------

        [HttpDelete]
        [Route("{id:Guid}")]
 //       [Authorize(Roles = "Writer")]
        public async Task< IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel= await regionRepository.DeleteAsync(id);
            //check if this id exists
            if(regionDomainModel == null)
            {
                return NotFound();
            }
          
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
       
    }
}
