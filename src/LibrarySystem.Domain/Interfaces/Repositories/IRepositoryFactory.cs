namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IRepositoryFactory
{
    IUserRepository CreateUserRepository();
}
