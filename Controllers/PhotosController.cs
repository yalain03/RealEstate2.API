using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RealEstate.API.Data;
using RealEstate.API.Dtos;
using RealEstate.API.Helpers;
using RealEstate.API.Models;

namespace RealEstate.API.Controllers
{
    [Route("api/houses/{houseId}/[controller]/")]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRealEstateRepository _repo;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IRealEstateRepository repo, IMapper mapper, 
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _repo = repo;
            _mapper = mapper;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPhotoForHouse(int houseId, 
            [FromForm]PhotosForCreationDto photoDto)
        {
            var houseFromRepo = await _repo.GetHouse(houseId);

            var file = photoDto.File;

            photoDto.HouseId = houseId;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                };
            }

            photoDto.Url = uploadResult.Uri.ToString();
            photoDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoDto);

            if(!houseFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            houseFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add photo");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo == null)
                return NotFound("Photo not found");

            if(photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if(result.Result == "ok")
                    _repo.Delete(photoFromRepo);
            }

            if(photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if(await _repo.SaveAll())
                return Ok();

            return BadRequest("Error during photo deletion");
        }

        // [Authorize]
        // [HttpPost("{id}/main")]
        // public async Task<IActionResult> setMain(int houseId, int id) 
        // {
        //     var house = await _repo.GetHouse(houseId);

        //     var photoFromRepo = house.Photos.Where(p => p.IsMain == true);

            
        //     return Ok("Worked");
        // }
    }
}