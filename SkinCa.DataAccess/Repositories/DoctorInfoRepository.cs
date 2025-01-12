using Microsoft.EntityFrameworkCore;
using SkinCa.DataAccess.RepositoriesContracts;

namespace SkinCa.DataAccess.Repositories;

public class DoctorInfoRepository:IDoctorInfoRepository
{
    private readonly AppDbContext _context;

    public DoctorInfoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DoctorInfo?> GetDoctorInfoAsync(string doctorId)
    {
        return await _context.DoctorInfos
            .Include(d => d.User)
            .Include(d => d.WorkingDays)
            .Where(d => d.UserId == doctorId)
            .SingleOrDefaultAsync();
    }
    public async Task<List<DoctorInfo>> GetAllAsync()
    {
        return await _context.DoctorInfos
            .Include(d=>d.User)
            .Include(d => d.WorkingDays)
            .ToListAsync();
    }

    public async Task<List<DoctorInfo>> GetWorkingDoctorsAsync()
    {
        return await _context.DoctorInfos
            .Include(d => d.WorkingDays)
            .Include(d=>d.User)
            .Where(d => (d.WorkingDays.Any(IsWorkingNow)))
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(DoctorInfo doctor)
    {
        await _context.DoctorInfos.AddAsync(doctor);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var doctorInfo = await _context.DoctorInfos.FindAsync(id);
        if (doctorInfo == null) return null;
        _context.DoctorInfos.Remove(doctorInfo);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool?> UpdateAsync(DoctorInfo doctor)
    {
        var existing =await _context.DoctorInfos.FindAsync(doctor.Id);
        
        if(existing==null) return null;
        
        existing.WorkingDays = doctor.WorkingDays;
        existing.Description = doctor.Description;
        existing.Experience =  doctor.Experience;
        existing.Services =    doctor.Services;
        existing.ClinicFees =  doctor.ClinicFees;
        existing.Specialization = doctor.Specialization;
        existing.User.Latitude = doctor.User.Latitude;
        existing.User.Longitude = doctor.User.Longitude;
        existing.User.Governorate = doctor.User.Governorate;
        existing.User.ProfilePicture= doctor.User.ProfilePicture;
        existing.User.FirstName = doctor.User.FirstName;
        existing.User.LastName = doctor.User.LastName;
        existing.User.BirthDate = doctor.User.BirthDate;
        existing.User.Address = doctor.User.Address;
        
        _context.DoctorInfos.Update(existing);
        
        return await _context.SaveChangesAsync() > 0;
    }

    public bool IsWorkingNow(DoctorWorkingDay doctorWorkingDay)
    {
        return (doctorWorkingDay.Day == DateTime.Now.DayOfWeek
                && doctorWorkingDay.CloseAt.CompareTo(DateTime.Now.TimeOfDay) <= 0
                && doctorWorkingDay.OpenAt.CompareTo(DateTime.Now.TimeOfDay) > 0);
    }
    
}