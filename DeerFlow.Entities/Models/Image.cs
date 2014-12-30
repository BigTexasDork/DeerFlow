using System.ComponentModel.DataAnnotations.Schema;

namespace DeerFlow.Entities.Models
{
    public class Image : IObjectState
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public byte[] Data { get; set; }

        [NotMapped]
        public ObjectState State { get; set; }
    }
}