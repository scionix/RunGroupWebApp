using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository repository, IPhotoService photoService)
        {
            _clubRepository = repository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetClubByIdWithAddress(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address 
                    { 
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State
                    }
                };

                _clubRepository.Add(club);
                _clubRepository.Save();
                return RedirectToAction("Index");
            }
            else 
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetClubByIdWithAddress(id);

            if (club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                ImageURL = club.Image,
                ClubCategory = club.ClubCategory,
                AddressId = club.AddressId,
                Address = club.Address
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club.");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetClubByIdWithAddress(id);

            if (userClub != null)
            {
                try
                {
                    var file = new FileInfo(userClub.Image);
                    var publicId = Path.GetFileNameWithoutExtension(file.Name);
                    await _photoService.DeletePhotoAsync(publicId);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

                userClub.Id = clubVM.Id;
                userClub.Title = clubVM.Title;
                userClub.Description = clubVM.Description;
                userClub.Image = photoResult.Url.ToString();
                userClub.AddressId = clubVM.AddressId;
                userClub.Address = clubVM.Address;

                _clubRepository.Update(userClub);
                _clubRepository.Save();

                return RedirectToAction("Index");
            }

            return View("Index");
        }
    }
}
