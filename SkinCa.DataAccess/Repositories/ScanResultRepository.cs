using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories
{
    public class ScanResultRepository : IScanResultRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ScanResultRepository> _logger;

        public ScanResultRepository(AppDbContext context, ILogger<ScanResultRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ScanResult> GetByIdAsync(int id)
        {
            var scanResult = await _context.ScanResults.FindAsync(id);
            if (scanResult == null)
                throw new NotFoundException($"Scan result with ID {id} not found");

            return scanResult;
        }

        public async Task<List<ScanResult>> GetAllAsync()
        {
            return await _context.ScanResults.ToListAsync();
        }

        public async Task CreateAsync(ScanResult scanResult)
        {
            await _context.ScanResults.AddAsync(scanResult);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while creating the scan result");
        }

        public async Task DeleteAsync(int id)
        {
            var scanResult = await _context.ScanResults.FindAsync(id);
            if (scanResult == null)
                throw new NotFoundException($"Scan result with ID {id} not found");

            _context.ScanResults.Remove(scanResult);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting the scan result");
        }
    }
}