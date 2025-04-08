using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.CQRS.Commands.WalksCommands;
using NZWalk.Api.CQRS.Queries.WalkQueries;
using NZWalk.Api.CustomActionfilters;
using NZWalk.Api.Models.Domain;
using NZWalk.Api.Models.DTO;
using NZWalk.Api.Repositories;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace NZWalk.Api.Controllers
{
    //api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
      //  private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<WalksController> logger;

        //  private readonly IWalkRepository walkRepository;

        //IMapper mapper, IWalkRepository walkRepository, 
        public WalksController(IMediator mediator,ILogger<WalksController> logger)
        {
            //this.mapper = mapper;
         //  this.WalkRepository = walkRepository;
            this.mediator = mediator;
            this.logger = logger;
        }

        public IWalkRepository WalkRepository { get; }

        //create Walk
        //post:/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)//
        {
             {
                //map dto to domain model using automapper

                //var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                //await WalkRepository.CreateAsync(walkDomainModel);

                ////map domain model to dto

                //return Ok(mapper.Map<WalkDto>(walkDomainModel));

                //new code usig cqrs

                logger.LogInformation("[WalksController] Received request to create a new walk.");

                try
                {

                    var command = new CreateWalkCommand(addWalkRequestDto);
                    logger.LogInformation($"[WalksController] Creating walk with data: {JsonSerializer.Serialize(command)}");
                    var result = await mediator.Send(command);
                    logger.LogInformation($"[WalksController] Walk created successfully with ID: {result.id}");
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "[WalksController] Error occurred while creating walk.");
                    return StatusCode(500, "An error occurred while processing your request.");

                }

            }
        }


        //        These parameters allow the client to send filtering, sorting, and pagination options via the query string.

        //✅ [FromQuery]
        //        string? filterOn
        //Specifies which field to filter on(e.g., "name" or "region").
        //? → Makes it nullable.
        //✅ [FromQuery]
        //        string? filterQuery
        //Specifies what value to filter for (e.g., "mountain" for filtering by name).
        //✅ [FromQuery]
        //        string? sortBy
        //Specifies which field to sort by(e.g., "name", "length", etc.).
        //✅ [FromQuery]
        //        bool? isAscending
        //Determines whether sorting should be in ascending(true) or descending(false) order.
        //?? true → If isAscending is null, it defaults to true (ascending order).
        //✅ [FromQuery]
        //        int pageNumber = 1
        //Specifies the page number for pagination.Defaults to 1.
        //✅ int pageSize = 1000
        //Specifies the number of items per page.Defaults to 1000.


        //get walk
        //get:/api/walks?filterOn=Name&filterQuery=Trak&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10

        [HttpGet] 
         public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                [FromQuery] int pageNumber = 1, int pageSize = 1000 )
            {
            //var walks = await mediator.Send(filterOn, filterQuery, sortBy, isAscending ?? true,
            //       pageNumber, pageSize);

            //// Creat an new  excepion

            //throw new Exception("This is new exception ");
            //    //mape domain model to dto

            //    return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));

            //New code by cqrs
            try
            {
                logger.LogInformation("[WalksController] Received request to get all walks.");
                var query = new GetAllWalksQuery
                      (filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

                logger.LogInformation($"[WalksController] Fetching walks with filters: {JsonSerializer.Serialize(query)}");


                var result = await mediator.Send(query);

                logger.LogInformation("[WalksController] Successfully retrieved walks.");


                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[WalksController] Error fetching all walks.");
                return StatusCode(500, "An error occurred while processing your request.");

            }

        }
        

            [HttpGet]   
            [Route("{id:Guid}")]
            public async Task<IActionResult> GetById([FromRoute] Guid id)
            {
            //var walkDomainModel = await WalkRepository.GetByIdAsync(id);
            //if (walkDomainModel == null)
            //{
            //    return NotFound();
            //}
            ////map domain model to dto
            //return Ok(mapper.Map<WalkDto>(walkDomainModel));

            //NEw Code
            logger.LogInformation($"[WalksController] Received request to get walk by ID: {id}");

            try
            {
                var query = new GetWalkByIdQuery(id);

              

                var result = await mediator.Send(query);
                if (result == null)
                {
                    logger.LogWarning($"[WalksController] Walk with ID {id} not found.");
                    return NotFound();
                }

                logger.LogInformation($"[WalksController] Successfully retrieved walk with ID: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"[WalksController] Error fetching walk with ID: {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


            //update walk by id
            //put:/api/walks/{id}
            [HttpPut]
            [Route("{id:Guid}")]
            [ValidateModel]
            public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
            {


            //map dto to domain model 

            //var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            //walkDomainModel = await WalkRepository.UpdateAsync(id, walkDomainModel);
            //if (walkDomainModel == null)
            //{
            //    return NotFound();
            //}
            ////map domain model to dto
            //return Ok(mapper.Map<WalkDto>(walkDomainModel));

            //new code using cqrs
            logger.LogInformation($"[WalksController] Received request to update walk with ID: {id}");

            try
            {
                var updatedWalk = await mediator.Send(new UpdateWalkCommand(id, updateWalkRequestDto));

                logger.LogInformation($"[WalksController] Updating walk with data: {JsonSerializer.Serialize(updatedWalk)}");


                if (updatedWalk == null) { 
                logger.LogWarning($"[WalksController] Walk with ID {id} not found for update.");
                return NotFound();
            }
                logger.LogInformation($"[WalksController] Successfully updated walk with ID: {id}");
                return Ok(updatedWalk);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"[WalksController] Error updating walk with ID: {id}");
                return StatusCode(500, "An error occurred while processing your request.");

            }

        }
        //delete walk by id
        //delete:/api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var deletedWalkDomainModel = await WalkRepository.DeleteAsync(id);
            //if (deletedWalkDomainModel == null)
            //{
            //    return NotFound();
            //}
            ////map domain model to dto
            //return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
            logger.LogInformation($"[WalksController] Received request to delete walk with ID: {id}");

            try
            {
                var deletedWalk = await mediator.Send(new DeleteWalkCommand(id));

                if (deletedWalk == null)
                {
                    logger.LogWarning($"[WalksController] Walk with ID {id} not found for deletion.");
                    return NotFound();
                }


                logger.LogInformation($"[WalksController] Successfully deleted walk with ID: {id}");
                return Ok(deletedWalk);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"[WalksController] Error deleting walk with ID: {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



    }
}
