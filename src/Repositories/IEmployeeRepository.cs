using Models.Models;

namespace Repositories;

public interface IEmployeeRepository
{
    List<Employee> GetAll();
    Employee GetById(int id);
}
