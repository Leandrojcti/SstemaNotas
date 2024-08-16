using Microsoft.EntityFrameworkCore;
using SistemaNote.Models;

namespace SistemaNote.BancoDados
{
    public class ClaseContext : DbContext
    {
        public ClaseContext(DbContextOptions<ClaseContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Notas> notas { get; set; }
    }
}
//DbSet represaenta uma tabela no banco de daedos
//DbContext clase interna do entity Framework que representa um contexto de banco de dados