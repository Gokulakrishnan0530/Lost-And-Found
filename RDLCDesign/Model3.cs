using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RDLCDesign
{
    public partial class Model3 : DbContext
    {
        public Model3()
            : base("name=Model3")
        {
        }

        public virtual DbSet<Finded> Findeds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
