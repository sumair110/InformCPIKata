using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InformCPIKata.Models
{
	public class ContactDbContext : DbContext
	{
		public ContactDbContext(DbContextOptions<ContactDbContext> options)
		: base(options)
		{
		}

		public DbSet<Contact> Contacts { get; set; }
	}
}
