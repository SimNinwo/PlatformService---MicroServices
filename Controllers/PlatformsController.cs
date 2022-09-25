using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platRepo;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo platRepo, IMapper mapper)
        {
            _platRepo = platRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("---> Getting Platfoms...");

            var platformItems = _platRepo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }


        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine($"---> Getting a platform with Id: {id}");

            var platformItem = _platRepo.GetPlatformById(id);
            if(platformItem != null){
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            
            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            System.Console.WriteLine("---> Creating a platform.");

            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            
            _platRepo.CreatePlatform(platformModel);
            _platRepo.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id}, platformReadDto);
        }
    }
}