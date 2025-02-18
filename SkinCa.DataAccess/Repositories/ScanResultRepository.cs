using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess;
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
            try
            {
                var scanResult = await _context.ScanResults.FindAsync(id);
                if (scanResult == null)
                    throw new NotFoundException($"Scan result with ID {id} not found");

                return scanResult;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error retrieving scan result {ScanResultId}", id);
                throw new RepositoryException("Error retrieving scan result", ex);
            }
        }

        public async Task<List<ScanResult>> GetAllAsync()
        {
            try
            {
                return await _context.ScanResults.ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error retrieving all scan results");
                throw new RepositoryException("Error retrieving scan results", ex);
            }
        }

        public async Task CreateAsync(ScanResult scanResult)
        {
            try
            {
                await _context.ScanResults.AddAsync(scanResult);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while creating the scan result");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating scan result");
                throw new RepositoryException("Error creating scan result", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var scanResult = await _context.ScanResults.FindAsync(id);
                if (scanResult == null)
                    throw new NotFoundException($"Scan result with ID {id} not found");

                _context.ScanResults.Remove(scanResult);
                if (await _context.SaveChangesAsync() == 0)
                    throw new RepositoryException("No changes were saved to the database while deleting the scan result");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting scan result {ScanResultId}", id);
                throw new RepositoryException("Error deleting scan result", ex);
            }
        }
    }
}
