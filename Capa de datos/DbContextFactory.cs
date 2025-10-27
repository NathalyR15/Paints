using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Capa_de_datos
{
    public static class DbContextFactory
    {
        private static DbContextOptions<AppDbContext>? _options;

        public static void Init(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString);
            _options = builder.Options;
        }

        public static AppDbContext Create()
        {
            if (_options == null) throw new InvalidOperationException("DbContextFactory no está inicializada.");
            return new AppDbContext(_options);
        }
    }
}
