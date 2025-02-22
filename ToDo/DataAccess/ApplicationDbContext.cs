using Microsoft.EntityFrameworkCore;
using ToDo.Models;
using ToDo.DataAccess;

namespace ToDo.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer("Data Source=MOUSSAN\\MSSQLSERVERS;Initial Catalog=ToDoList;Integrated Security=True;TrustServerCertificate=True");

    }
}
