using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        public ClubRepository(RunGroupDbContext context) : base(context) { }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetClubByIdWithAddress(int id)
        {
            return await _context.Clubs.Include(c => c.Address).Where(c => c.Id == id).FirstAsync();
        }
    }
}
