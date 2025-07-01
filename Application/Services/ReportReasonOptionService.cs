using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Domain;
using Infrastructure.Repository;

namespace Application.Services {
    public class ReportReasonOptionService {
        private readonly ReportReasonOptionRepository _repo;

        public ReportReasonOptionService(ReportReasonOptionRepository repo) {
            _repo = repo;
        }

        public async Task<List<ReportReasonOption>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task AddAsync(string name) {
            await _repo.AddAsync(name);
        }

        public async Task DeleteAsync(int id)
            => await _repo.DeleteAsync(id);


    }
}
