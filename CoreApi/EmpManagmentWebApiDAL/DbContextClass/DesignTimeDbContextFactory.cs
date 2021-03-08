using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmpManagmentWebApiDAL.DbContextClass
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EmployeeDbContext>
    {
        public EmployeeDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../EmpManagmentWebApi/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<EmployeeDbContext>();
            var connectionString = configuration.GetConnectionString("EmployeeDBConnection");
            builder.UseSqlServer(connectionString);
            return new EmployeeDbContext(builder.Options);
        }
    }
}
