using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MovieLibrary.Api.Data;

public class MovieLibrayDbContext(DbContextOptions<MovieLibrayDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{

}