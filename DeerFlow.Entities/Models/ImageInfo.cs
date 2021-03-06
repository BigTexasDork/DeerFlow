﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeerFlow.Entities.Models
{
    public class ImageInfo : IObjectState
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public DateTime ExifDate { get; set; }
        public string ExifLattitude { get; set; }
        public string ExifLongitude { get; set; }
        public string StorageType { get; set; }
        public int? ImageId { get; set; }
        public DateTime InsertDate { get; set; }

        [NotMapped]
        public ObjectState State { get; set; }

        public Image Image { get; set; }
    }
}