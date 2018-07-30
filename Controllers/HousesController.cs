using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Data;
using RealEstate.API.Dtos;
using RealEstate.API.Helpers;
using RealEstate.API.Models;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRealEstateRepository _repo;
        public HousesController(IRealEstateRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetHouses([FromQuery]UserParams userParams)
        {
            var houses = await _repo.GetHouses(userParams);

            var housesToReturn = _mapper.Map<IEnumerable<HousesForListDto>>(houses);

            Response.AddPagination(houses.CurrentPage, houses.PageSize, 
                houses.TotalCount, houses.TotalPages);

            return Ok(housesToReturn);
        }

        [HttpGet("{houseId}")]
        public async Task<IActionResult> GetHouse(int houseId)
        {
            var houseFromRepo = await _repo.GetHouse(houseId);

            var houseToReturn = _mapper.Map<HouseForDetailedDto>(houseFromRepo);

            return Ok(houseToReturn);
        }

        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetHousesForUser(int userId, [FromQuery]UserParams userParams)
        {
            var housesFromRepo = await _repo.GetHousesForUser(userId, userParams);

            var housesToReturn = _mapper.Map<IEnumerable<HousesForListDto>>(housesFromRepo);

            return Ok(housesToReturn);
        }

        [Authorize]
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> CreateHouse(int userId, [FromBody]HouseForCreationDto houseDto)
        {
            // var cred = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // if(userId != cred)
            //     return Unauthorized();

            houseDto.UserId = userId;

            var house = _mapper.Map<House>(houseDto);

            _repo.Add(house);

            if(await _repo.SaveAll())
                return StatusCode(201);

            return BadRequest("Error during house creation");
        }
    }
}