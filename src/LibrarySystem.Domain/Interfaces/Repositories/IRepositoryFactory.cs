namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryFactory
{
    IUserRepository CreateUserRepository();
    IEmployeeRepository CreateEmployeeRepository();
    IAuthorRepository CreateAuthorRepository();
}
