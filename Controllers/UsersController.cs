using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Data;
using RealEstate.API.Dtos;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRealEstateRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IRealEstateRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _repo.GetUser(id);

            if (userFromRepo == null)
                return BadRequest("User not found");

            var userToReturn = _mapper.Map<UserForDetailedDto>(userFromRepo);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForRegisterDto userDto)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userDto, userFromRepo);

            if(await _repo.SaveAll())
                return NoContent();

            return BadRequest("Error during personal info update");
        }
    }
}