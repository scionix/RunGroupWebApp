using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class RaceRepository : GenericRepository<Race>, IRaceRepository
    {
        public RaceRepository(RunGroupDbContext context) : base(context) { }

        public async Task<Race> GetRaceByIdWithAddress(int id)
        {
            return await _context.Races.Include(r => r.Address).Where(r => r.Id == id).FirstAsync();
        }
    }
}
