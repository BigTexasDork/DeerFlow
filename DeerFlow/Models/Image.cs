using System.ComponentModel.DataAnnotations.Schema;

namespace DeerFlow.Models
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public byte[] Data { get; set; }
    }
}