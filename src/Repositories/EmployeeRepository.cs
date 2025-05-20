using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly _2019sbdContext _context;
    public EmployeeRepository(_2019sbdContext context) { _context = context; }

    public List<Employee> GetAll() => _context.Employees.Include(e => e.Person).ToList();

    public Employee GetById(int id) => _context.Employees
        .Include(e => e.Person)
        .Include(e => e.Position)
        .FirstOrDefault(e => e.Id == id);
}
