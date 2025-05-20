using Models.Models;

namespace Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly _2019sbdContext _context;
    public DeviceRepository(_2019sbdContext context) { _context = context; }

    public List<Device> GetAll() => _context.Devices.ToList();
    public Device GetById(int id) => _context.Devices.Find(id);
    public Device Create(Device device)
    {
        _context.Devices.Add(device);
        _context.SaveChanges();
        return device;
    }
    public Device Update(Device device)
    {
        _context.Devices.Update(device);
        _context.SaveChanges();
        return device;
    }
    public bool Delete(int id)
    {
        var device = _context.Devices.Find(id);
        if (device == null) return false;
        _context.Devices.Remove(device);
        _context.SaveChanges();
        return true;
    }
}
