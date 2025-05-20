using Models.DTOs;
using Repositories;

namespace Application;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    public EmployeeService(IEmployeeRepository repository) { _repository = repository; }

    public List<EmployeeListDto> GetAll() => _repository.GetAll()
        .Select(e => new EmployeeListDto
        {
            Id = e.Id,
            FullName = e.Person.FirstName + " " + e.Person.LastName
        }).ToList();

    public EmployeeDetailDto GetById(int id)
    {
        var e = _repository.GetById(id);
        if (e == null) return null;
        return new EmployeeDetailDto
        {
            FirstName = e.Person.FirstName,
            MiddleName = e.Person.MiddleName,
            LastName = e.Person.LastName,
            Email = e.Person.Email,
            PhoneNumber = e.Person.PhoneNumber,
            Salary = e.Salary,
            HireDate = e.HireDate,
            Position = new PositionDto { Id = e.Position.Id, Name = e.Position.Name }
        };
    }
}