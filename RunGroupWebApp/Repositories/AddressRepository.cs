using RunGroupWebApp.Data.DBContext;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(RunGroupDbContext context) : base(context) { }
    }
}
