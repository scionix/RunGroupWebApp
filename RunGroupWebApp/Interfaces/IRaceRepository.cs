using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IRaceRepository : IGenericRepository<Race>
    {
        Task<Race> GetRaceByIdWithAddress(int id);
    }
}
