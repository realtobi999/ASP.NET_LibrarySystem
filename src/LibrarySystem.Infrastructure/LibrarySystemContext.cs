using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public class LibrarySystemContext(DbContextOptions<LibrarySystemContext> options ) : DbContext(options)
{

}
