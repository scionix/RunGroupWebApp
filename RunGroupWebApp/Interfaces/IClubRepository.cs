using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        Task<IEnumerable<Club>> GetClubByCity(string city);

        Task<Club> GetClubByIdWithAddress(int id);
    }
}
