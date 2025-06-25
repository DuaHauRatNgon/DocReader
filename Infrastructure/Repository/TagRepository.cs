using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository {
    public class TagRepository {
        private readonly AppDbContext _context;



        public TagRepository(AppDbContext context) {
            _context = context;
        }





        public async Task<List<Tag>> GetAllAsync() => await _context.Tags.ToListAsync();





        public async Task<Tag?> GetByIdAsync(Guid id)
            => await _context.Tags.FindAsync(id);






        public async Task CreateAsync(string name) {
            var tag = new Tag {
                Id = Guid.NewGuid(),
                Name = name.Trim()
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }







        public async Task UpdateAsync(Guid id, string newName) {
            var tag = await _context.Tags.FindAsync(id);
            tag.Name = newName.Trim();
            await _context.SaveChangesAsync();
        }




        public async Task<bool> DeleteAsync(Guid id) {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
