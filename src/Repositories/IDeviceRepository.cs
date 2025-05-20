using Models.Models;

namespace Repositories;

public interface IDeviceRepository
{
    List<Device> GetAll();
    Device GetById(int id);
    Device Create(Device device);
    Device Update(Device device);
    bool Delete(int id);
}

