using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DeerFlow.Entities.Models;

//namespace DeerFlow.Models
//{
//    public class DeerFlow2Context
//    {
//    }
//}
namespace DeerFlow.Models
{
    public class DeerFlowContext : DbContext
    {
        public DeerFlowContext()
            : base("name=DeerFlowContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<DeerFlowContext>(null);
        }

        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<ImageInfo> ImageInfo { get; set; }
    }
}
