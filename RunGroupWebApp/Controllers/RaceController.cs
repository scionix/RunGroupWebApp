using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository reposoitory, IPhotoService photoService)
        {
            _raceRepository = reposoitory;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var races = await _raceRepository.GetAll();
            return View(races);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State
                    }
                };

                _raceRepository.Add(race);
                _raceRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }

        public async Task<IActionResult>  Detail(int id)
        {
            Race race = await _raceRepository.GetRaceByIdWithAddress(id);
            return View(race);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetRaceByIdWithAddress(id);

            if (race == null)
            {
                return View("Error");
            }

            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                ImageURL = race.Image,
                RaceCategory = race.RaceCategory,
                AddressId = race.AddressId,
                Address = race.Address
            };

            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race.");
                return View("Edit", raceVM);
            }

            var userRace = await _raceRepository.GetRaceByIdWithAddress(id);

            if (userRace != null)
            {
                try
                {
                    var file = new FileInfo(userRace.Image);
                    var publicId = Path.GetFileNameWithoutExtension(file.Name);
                    await _photoService.DeletePhotoAsync(publicId);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

                userRace.Id = raceVM.Id;
                userRace.Title = raceVM.Title;
                userRace.Description = raceVM.Description;
                userRace.Image = photoResult.Url.ToString();
                userRace.AddressId = raceVM.AddressId;
                userRace.Address = raceVM.Address;

                _raceRepository.Update(userRace);
                _raceRepository.Save();

                return RedirectToAction("Index");
            }

            return View("Index");
        }
    }
}
