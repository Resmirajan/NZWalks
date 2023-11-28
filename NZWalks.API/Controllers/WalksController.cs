using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;
using System.Data;
using System.Net;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            Mapper = mapper;
            WalkRepository = walkRepository;
        }

        public IMapper Mapper { get; }
        public IWalkRepository WalkRepository { get; }

        //CREATE Walks post method
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            
                //map dto to domain model
                var walkDomainModel = Mapper.Map<Walk>(addWalksRequestDto);
                await WalkRepository.CreateAsync(walkDomainModel);
                //map domain to dto

                return Ok(Mapper.Map<WalkDto>(walkDomainModel));
          

        }

        //GetALL
        //GET:/api/walks?filterOn=name&filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterOn, [FromQuery]string? filterQuery, 
            [FromQuery] string? sortby, [FromQuery] bool? isAscending,
            [FromQuery]int pageNumber = 1, [FromQuery]int pageSize=1000)
        {
           
                var WalkDomainModel = await WalkRepository.GetAllAsync(filterOn, filterQuery, sortby, isAscending ?? true, pageNumber, pageSize);


            //Create exception 
            throw new Exception("This is a new exception");

                return Ok(Mapper.Map<List<WalkDto>>(WalkDomainModel));
            
        }

        //Get Single walk
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
          var WalkDomainModel= await WalkRepository.GetByIdAsync(id);
            if(WalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<WalkDto>(WalkDomainModel));
        }

        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update( [FromRoute]Guid id, [FromBody]UpdateWalkRequestDto updateWalkRequestDto)
        {
            
                var walkDomainModel = Mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await WalkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(Mapper.Map<WalkDto>(walkDomainModel));
           

        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult>Delete([FromRoute]Guid id)
        {
            var deletedWalkDoaminModel= await WalkRepository.DeleteAsync(id);
            if (deletedWalkDoaminModel == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<WalkDto>(deletedWalkDoaminModel));
        }
    }
}
