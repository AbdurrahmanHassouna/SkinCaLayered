using SkinCa.DataAccess.RepoContracts;

namespace SkinCa.DataAccess.Repositories;

public class DoctorRepository:IDoctorRepository
{
    public Task<List<DoctorInfo>> GetDoctorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<DoctorInfo>> GetNearbyDoctorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<DoctorInfo>> GetWorkingDoctorsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DoctorInfo> CraeteDoctorInfo(DoctorInfo doctor)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDoctorAsync(DoctorInfo doctor)
    {
        throw new NotImplementedException();
    }
}