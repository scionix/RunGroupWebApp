using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        private IRaceRepository _raceRepository;
        public RaceController(IRaceRepository reposoitory)
        {
            _raceRepository = reposoitory;
        }

        public IActionResult Index()
        {
            var races = _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult>  Detail(int id)
        {
            Race race = await _raceRepository.GetRaceByIdWithAddress(id);
            return View(race);
        }
    }
}
