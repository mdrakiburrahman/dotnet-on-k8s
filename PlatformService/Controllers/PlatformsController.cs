using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")] // Uses the name - Platforms (minus the Controller) as the route; e.g. https://localhost:5001/api/platforms
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet] // We will return an enumeration of our Dtos here
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting all platforms");

            // Pull Platforms from the repository
            var platformItem = _repository.GetAllPlatforms();

            // Return a 200 and the result by automapping the enumerable by automapper
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        }

        [HttpGet("{id}", Name = "GetPlatformById")] // Specify the route so that if there's an id, it gets to this versus the above (no id)
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("--> Getting platform by id");

            // Pull Platforms from the repository by Id
            var platformItem = _repository.GetPlatformById(id);

            if (platformItem != null) 
            {   
                // If the platform exists, return the single object (not an enumerable)
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            // 404
            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Console.WriteLine("--> Creating platform");

            // Map the Dto to a temporary model object that we pass into the repo
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            // Best Practice - return:
            // 1. Http 201
            // 2. THe resource that was created
            // 3. URI to the resource location
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            
            // Sends an HTTP 201 with a Route back to the resource location as part of the return header
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto); // Returns the resource created in Body
        }
    }
}