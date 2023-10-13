using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repositories;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private IClubRepository _clubRepository;

        public ClubController(IClubRepository repository)
        {
            _clubRepository = repository;
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
    }
}
