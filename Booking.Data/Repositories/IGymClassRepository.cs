using Booking.Core.Entities;

namespace Booking.Data.Repositories
{
    public interface IGymClassRepository
    {
        Task<List<GymClass>> GetAsync();
        Task<IEnumerable<GymClass>> GetWithAttendinAsync();
        Task<GymClass?> GetAsync(int id);
        void Add(GymClass gymclass);
      
    }
}