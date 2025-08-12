using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCa.Common.Exceptions;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories
{
    public class DoctorInfoRepository : IDoctorInfoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DoctorInfoRepository> _logger;

        public DoctorInfoRepository(AppDbContext context, ILogger<DoctorInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DoctorInfo> GetDoctorInfoAsync(string doctorId)
        {
            var doctorInfo = await _context.DoctorInfos
                .Include(d => d.User)
                .Include(d => d.WorkingDays)
                .Where(d => d.UserId == doctorId)
                .SingleOrDefaultAsync();

            if (doctorInfo == null)
                throw new NotFoundException($"Doctor with ID {doctorId} not found");

            return doctorInfo;
        }

        public async Task<List<DoctorInfo>> GetAllAsync()
        {
            return await _context.DoctorInfos.AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.WorkingDays)
                .ToListAsync();
        }

        public async Task<List<DoctorInfo>> GetWorkingDoctorsAsync()
        {
            return await _context.DoctorInfos.AsNoTracking()
                .Include(d => d.WorkingDays)
                .Include(d => d.User)
                .Where(d => d.WorkingDays.Any(IsWorkingNow))
                .ToListAsync();
        }

        public async Task CreateAsync(DoctorInfo doctor)
        {
            await _context.DoctorInfos.AddAsync(doctor);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while creating doctor info");
        }

        public async Task DeleteAsync(string userId)
        {
            var doctorInfo = await _context.DoctorInfos.FindAsync(userId);
            if (doctorInfo == null)
                throw new NotFoundException($"Doctor with ID {userId} not found");

            _context.DoctorInfos.Remove(doctorInfo);
            if (await _context.SaveChangesAsync() == 0)
                throw new RepositoryException("No changes were saved to the database while deleting doctor info");
        }

        public async Task UpdateAsync(DoctorInfo doctor)
        {
            var existing = await _context.DoctorInfos.FindAsync(doctor.UserId);
            if (existing == null)
                throw new NotFoundException($"Doctor with ID {doctor.UserId} not found");

            existing.WorkingDays = doctor.WorkingDays;
            existing.Description = doctor.Description;
            existing.Experience = doctor.Experience;
            existing.Services = doctor.Services;
            existing.ClinicFees = doctor.ClinicFees;
            existing.Specialization = doctor.Specialization;

            _context.DoctorInfos.Update(existing);
            if (await _context.SaveChangesAsync() == 0)
            {
                throw new RepositoryException("No changes were saved to the database while updating doctor info");
            }
        }

        public bool IsWorkingNow(DoctorWorkingDay doctorWorkingDay)
        {
            return doctorWorkingDay.Day == DateTime.Now.DayOfWeek
                   && doctorWorkingDay.CloseAt.CompareTo(DateTime.Now.TimeOfDay) <= 0
                   && doctorWorkingDay.OpenAt.CompareTo(DateTime.Now.TimeOfDay) > 0;
        }
    }
}