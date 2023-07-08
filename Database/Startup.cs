using Database.Data;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
	public static class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Catan; Trusted_Connection=True;MultipleActiveResultSets=true"));
			services.AddIdentityCore<User>(o =>
			{
				o.Password.RequireNonAlphanumeric = false;
				o.User.RequireUniqueEmail = true;
			})
				.AddEntityFrameworkStores<AppDbContext>();
		}
	}
}
