using System.Text.Json;
using System.Text.RegularExpressions;
using Application;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Repositories;

public class DeviceService : IDeviceService
{
    private readonly _2019sbdContext _context;
    private readonly IDeviceRepository _repository;
    public DeviceService(IDeviceRepository repository, _2019sbdContext context)
    {
        _repository = repository;
        _context = context;
    }

    public List<DeviceDto> GetAll() => _repository.GetAll().Select(d => new DeviceDto { Id = d.Id, Name = d.Name }).ToList();

    public DeviceDetailsDto GetById(int id)
    {
        
        var d = _repository.GetById(id);
        if (d == null) return null;

        var deviceType = _context.DeviceTypes.Find(d.DeviceTypeId);
        var currentAssignment = _context.DeviceEmployees
            .Include(de => de.Employee)
            .ThenInclude(e => e.Person)
            .FirstOrDefault(de => de.DeviceId == d.Id && de.ReturnDate == null);


        var fixedJson = FixJson(d.AdditionalProperties ?? "{}");
        var additionalProperties = JsonSerializer.Deserialize<Dictionary<string, string>>(fixedJson) ?? new Dictionary<string, string>();


        return new DeviceDetailsDto()
        {
            Id = d.Id,
            Name = d.Name,
            IsEnabled = d.IsEnabled,
            DeviceTypeName = deviceType?.Name,
            AdditionalProperties = additionalProperties,
            CurrentEmployee = currentAssignment != null ? new CurrentEmployeeDto
            {
                Id = currentAssignment.Employee.Id,
                FullName = $"{currentAssignment.Employee.Person.FirstName} {currentAssignment.Employee.Person.LastName}"
            } : null
        };
    }


    public DeviceDetailsDto Create(DeviceCreateRequest request)
    {
        var d = new Device
        {
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            AdditionalProperties = JsonSerializer.Serialize(request.AdditionalProperties),
            DeviceTypeId = request.DeviceTypeId
        };
        var created = _repository.Create(d);
        return new DeviceDetailsDto { Id = created.Id, Name = created.Name };
    }

    public DeviceDetailsDto Update(int id, DeviceCreateRequest request)
    {
        var existing = _repository.GetById(id);
        if (existing == null) return null;
        existing.Name = request.Name;
        existing.IsEnabled = request.IsEnabled;
        existing.AdditionalProperties = JsonSerializer.Serialize(request.AdditionalProperties);
        existing.DeviceTypeId = request.DeviceTypeId;
        var updated = _repository.Update(existing);
        return new DeviceDetailsDto { Id = updated.Id, Name = updated.Name };
    }

    public bool Delete(int id) => _repository.Delete(id);

    string FixJson(string json)//I added this because i was getting a formatting issue where the special properties was null because it didn't have "", I didn't want to change the database so I manually add the ""
    {
        return Regex.Replace(json, @"(?<=\{|,)\s*(\w+)\s*:", "\"$1\":");
    }

}
