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
            if(!string.IsNullOrEmpty(userParams.City))
                userParams.City = userParams.City.ToLower();
            if(!string.IsNullOrEmpty(userParams.State))
                userParams.State = userParams.State.ToLower();

            var houses = await _repo.GetHouses(userParams);

            var housesToReturn = _mapper.Map<IEnumerable<HousesForListDto>>(houses);

            Response.AddPagination(houses.CurrentPage, houses.PageSize, 
                houses.TotalCount, houses.TotalPages);

            return Ok(housesToReturn);
        }

        [HttpGet("{id}", Name="GetHouse")]
        public async Task<IActionResult> GetHouse(int id)
        {
            var houseFromRepo = await _repo.GetHouse(id);

            var houseToReturn = _mapper.Map<HouseForDetailedDto>(houseFromRepo);

            return Ok(houseToReturn);
        }

        [Authorize]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetHousesForUser(int userId, [FromQuery]UserParams userParams)
        {            
            if(!string.IsNullOrEmpty(userParams.City))
                userParams.City = userParams.City.ToLower();
            if(!string.IsNullOrEmpty(userParams.State))
                userParams.State = userParams.State.ToLower();

            var houses = await _repo.GetHousesForUser(userId, userParams);

            var housesToReturn = _mapper.Map<IEnumerable<HousesForListDto>>(houses);

            Response.AddPagination(houses.CurrentPage, houses.PageSize, 
                houses.TotalCount, houses.TotalPages);

            return Ok(housesToReturn);
        }

        [Authorize]
        [HttpPost("user/{userId}")]
        public async Task<IActionResult> CreateHouse(int userId, [FromBody]HouseForCreationDto houseDto)
        {
            // var cred = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // if(userId != cred)
            //     return Unauthorized();

            var house = _mapper.Map<House>(houseDto);
            house.City = house.City.ToLower();
            house.State = house.State.ToLower();

            _repo.Add(house);

            if(await _repo.SaveAll()) {
                var houseToReturn = _mapper.Map<HouseForDetailedDto>(house);
                return CreatedAtRoute("GetHouse", new {id = house.Id}, houseToReturn);
            }                

            return BadRequest("Error during house creation");
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHouse(int id, [FromBody]HouseForCreationDto houseDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var houseFromRepo = await _repo.GetHouse(id);

            if(houseFromRepo == null)
                return BadRequest("House does not exist!");

            _mapper.Map(houseDto, houseFromRepo);

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Error on house update");
        }

        [Authorize]
        [HttpDelete("user/{userId}/delete/{id}")]
        public async Task<IActionResult> DeleteHouse(int userId, int id) 
        {
            // if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //     return Unauthorized();

            var houseFromRepo = await _repo.GetHouse(id);

            if(houseFromRepo == null)
                return NotFound("House not found!");

            _repo.Delete(houseFromRepo);

            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error during house deletion");
        }
    }
}