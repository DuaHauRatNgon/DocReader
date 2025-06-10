using Core.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Repository;
using Core.Models.Domain.Core.Models.Domain;

namespace Application.Services {
    public class TagService {
        private TagRepository _tagRepository;



        public TagService(TagRepository tagRepository) {
            _tagRepository = tagRepository;
        }




        public async Task<List<Tag>> GetAllAsync() {
            var res = await _tagRepository.GetAllAsync();
            return res;
        }







        public async Task<Tag> GetByIdAsync(Guid id) => await _tagRepository.GetByIdAsync(id);






        public async Task CreateAsync(string name) => await _tagRepository.CreateAsync(name);






        public async Task UpdateAsync(Guid id, string name) => await _tagRepository.UpdateAsync(id, name);




        public async Task<bool> DeleteAsync(Guid id) {
            return await _tagRepository.DeleteAsync(id);
        }
    }

}

