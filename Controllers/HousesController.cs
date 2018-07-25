using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Data;
using RealEstate.API.Dtos;
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
        public async Task<IActionResult> GetHouses()
        {
            var housesFromRepo = await _repo.GetHouses();

            var housesToReturn = _mapper.Map<IEnumerable<HousesForListDto>>(housesFromRepo);

            return Ok(housesToReturn);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateHouse(int userId, [FromBody]HouseForCreationDto houseDto)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            houseDto.UserId = userId;

            var house = _mapper.Map<House>(houseDto);

            _repo.Add(house);

            if(await _repo.SaveAll())
                return StatusCode(201);

            return BadRequest("Error during house creation");
        }
    }
}