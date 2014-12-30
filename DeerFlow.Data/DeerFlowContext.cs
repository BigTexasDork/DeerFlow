using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DeerFlow.Entities.Models;

namespace DeerFlow.Data
{
    public class DeerFlowContext : DbContext, IDbContext
    {
        static DeerFlowContext()
        {
            Database.SetInitializer<DeerFlowContext>(null);
        }

        public DeerFlowContext()
            : base("name=DeerFlowContext")
        {
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            this.ApplyStateChanges();
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ImageInfo>().ToTable("ImageInfo");
            modelBuilder.Entity<Image>().ToTable("Image");

            Database.SetInitializer<DeerFlowContext>(null);
        }

        //public virtual DbSet<Image> Image { get; set; }
        //public virtual DbSet<ImageInfo> ImageInfo { get; set; }
    }
}
