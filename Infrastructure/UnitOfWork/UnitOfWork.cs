using Core.Interfaces;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork {
    public class UnitOfWork : IUnitOfWork {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context) {
            _context = context;
        }

        public async Task SaveChangesAsync() {
            await _context.SaveChangesAsync();
        }
    }

}
