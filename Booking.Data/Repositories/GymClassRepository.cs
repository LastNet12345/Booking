using Booking.Core.Entities;
using Booking.Core.Repositories;
using Booking.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Repositories
{
    public class GymClassRepository : IGymClassRepository
    {
        private readonly ApplicationDbContext db;

        public GymClassRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<List<GymClass>> GetAsync()
        {
            return await db.GymClasses.ToListAsync();
        }

        public async Task<IEnumerable<GymClass>> GetWithAttendinAsync()
        {
            return await db.GymClasses.Include(g => g.AttendingMembers).ToListAsync();
        }

        public async Task<GymClass?> GetAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            return await db.GymClasses.FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Add(GymClass gymclass)
        {
            db.Add(gymclass);
        }



    }
}
