using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; //Provides attributes like [ApiController], [Route], [HttpGet], etc.
using Microsoft.EntityFrameworkCore;
using NZWalk.Api.CQRS.Commands.RegionsCommands;
using NZWalk.Api.CQRS.Commands.WalksCommands;
using NZWalk.Api.CQRS.Queries.RegionsQueries;
using NZWalk.Api.CQRS.Queries.WalkQueries;
using NZWalk.Api.CustomActionfilters;
using NZWalk.Api.Data;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;
using NZWalk.Api.Services;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;  //Used for logging by serializing objects into JSON format.

namespace NZWalk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;
        private readonly IMediator mediator;

        public IRegionRepository regionRepository { get; }

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger,IMediator mediator)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.mediator = mediator;
        }
        [HttpGet]
        [Authorize(Roles ="Reader,Writer")]
        public async Task< IActionResult >GetAll()
        {

            //var regions = new List<Region> 
            //{
            //    new Region
            //    { 
            //        id = Guid.NewGuid(),
            //        Name = "Auckland Region",
            //        Code = "AUK",                    
            //        RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/regions/auckland/auckland-landscape-2.jpg"
            //    },
            //    new Region
            //    {
            //        id = Guid.NewGuid(),
            //        Name = "Bay of Plenty Region",
            //        Code = "BOP",
            //        RegionImageUrl = "https://www.doc.govt.nz/globalassets/images/regions/bay-of-plenty/bay-of-plenty-landscape-2.jpg"
            //    }

            //};

            //get data from database

            //  var regionsDomain = await dbContext.Regions.ToListAsync();//without using repositories

            //old var regionsDomain = await regionRepository.GetAllAsync();// importing interface and concret class using repositories


            //map domani model to dto model
            // var regionDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionDto.Add(new RegionDto()
            //    {
            //        id = regionDomain.id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });

            //}

            //map domain model to dtos
            //var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);
            //return Ok(regionDto);
            //or

            //old return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            logger.LogInformation("Received request to get all regions.");
            try
            {

                var query = new GetAllRegionQuery();

                //Logs the data for debugging.
                logger.LogInformation($"Finishe get all region request with data:{JsonSerializer.Serialize(query)}");


                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while getting all regions.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching regions.");
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            //  var regionDomain =await dbContext.Regions.FirstOrDefaultAsync(x => x.id == id);//without using repositories

            //old var regionDomain = await regionRepository.GetByIdAsync(id);// importing interface and concret class using repositories

            //if (regionDomain == null)
            //{
            //    return NotFound();
            //}
            //var regionDto = new RegionDto()
            //{
            //    id = regionDomain.id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};
            //old return Ok(mapper.Map<RegionDto>(regionDomain));
            logger.LogInformation($"Received request to get region with ID: {id}");
            try
            {

                var query = new GetAllRegionsByIdQuery(id);
              //  logger.LogInformation($"Finishe get By ID region request with data:{JsonSerializer.Serialize(query)}");
               logger.LogInformation($"Successfully fetched region with ID: {id}");

                var result = await mediator.Send(query);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"Error occurred while getting region with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the region.");
            }
        }
        //post to create new region
        //post:https://localhost:5001/api/regions
        [HttpPost]
        // [ValidateModel]  //custom action filter validate model attribute
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map or convert dto to domain model
            //var regionDomainModel = new Region()
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name =  addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            //old
            // var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //use domain mode to to create region

            //await dbContext.Regions.AddAsync(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            //old
            //regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //map domain model back to dto model

            //var regionDto = new RegionDto()
            //{
            //    id = regionDomainModel.id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            //old
            //var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            //return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.id }, regionDto);
            logger.LogInformation($"Received request to create a region with data: {JsonSerializer.Serialize(addRegionRequestDto)}");
            try
            {

                var command = new CreateRegionsCommand(addRegionRequestDto);

                var result = await mediator.Send(command);
                logger.LogInformation($"Successfully created region with ID: {result.id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating a new region.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the region.");
            }


        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        { // map Dto to domain model
          //var regionDomain = new Region()
          //{
          //    Code = updateRegionRequestDto.Code,
          //    Name = updateRegionRequestDto.Name,
          //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
          //};

            //old   var regionDomain = mapper.Map<Region>(updateRegionRequestDto);


            //find check region by id
            //  var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.id == id);

            //old   regionDomain = await regionRepository.UpdateAsync(id, regionDomain);


            //if (regionDomain == null)
            //{
            //    return NotFound();
            //}
            //update region
            // regionDomain.Code = updateRegionRequestDto.Code;
            // regionDomain.Name = updateRegionRequestDto.Name;
            // regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            // //save changes
            //await dbContext.SaveChangesAsync();

            //map domain model back to dto model
            //var regionDto = new RegionDto()
            //{
            //    id = regionDomain.id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};

            //old //var regionDto = mapper.Map<RegionDto>(regionDomain);
            //return Ok(regionDto);
            logger.LogInformation($"Received request to update region with ID: {id} and data: {JsonSerializer.Serialize(updateRegionRequestDto)}");
            try
            {
                var updatedRegion = await mediator.Send(new UpdateRegionsCommand(id, updateRegionRequestDto));

                if (updatedRegion == null)
                {
                    logger.LogWarning($"Region with ID {id} not found for update.");
                    return NotFound();
                }
                logger.LogInformation($"Successfully updated region with ID: {id}");
                return Ok(updatedRegion);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"Error occurred while updating region with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the region.");
            }

        }
       

        [HttpDelete]
       [Route("{id:guid}")]
       [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //find region by id
            // var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.id == id);

            //old var regionDomain = await regionRepository.DeleteAsync(id);
            //  if (regionDomain == null)
            //  {
            //      return NotFound();
            //  }
            //  //delete region
            //  dbContext.Regions.Remove(regionDomain);
            //  //save changes
            //await dbContext.SaveChangesAsync();

            //return deleted region back
            //map domain model to dto

            //var regionDto = new RegionDto()
            //{
            //    id = regionDomain.id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};
            // var regionDto = mapper.Map<RegionDto>(regionDomain);
            // return Ok(mapper.Map<RegionDto>(regionDomain));
            logger.LogInformation($"Received request to delete region with ID: {id}");

            try
            {

                var deletedRegion = await mediator.Send(new DeleteReagionsCommand(id));

                if (deletedRegion == null)
                {
                    logger.LogWarning($"Region with ID {id} not found for deletion.");

                    return NotFound();
                }
                logger.LogInformation($"Successfully deleted region with ID: {id}");

                return Ok(deletedRegion);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while deleting region with ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the region.");
            }
        }

      


    }
}
