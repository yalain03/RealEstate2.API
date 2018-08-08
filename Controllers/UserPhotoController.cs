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
    [Route("api/users/{userId}/[controller]/")]
    [ApiController]
    public class UserPhotoController : ControllerBase
    {
        private readonly IRealEstateRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public UserPhotoController(IRealEstateRepository repo, IMapper mapper,        
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name="GetUserPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, 
            [FromForm]UserPhotoForCreationDto photoDto)
        {
            var userFromRepo = await _repo.GetUser(userId);

            var file = photoDto.File;

            photoDto.UserId = userId;

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

            var photo = _mapper.Map<UserPhoto>(photoDto);
            photo.User = userFromRepo;
            photo.UserId  = userFromRepo.Id;

            userFromRepo.UserPhoto = photo;

            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<UserPhotoForDetailedDto>(photo);
                return CreatedAtRoute("GetUserPhoto", new {id = photo.Id}, photoToReturn);
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
    }
}