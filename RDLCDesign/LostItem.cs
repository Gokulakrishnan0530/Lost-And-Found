namespace RDLCDesign
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LostItem")]
    public partial class LostItem
    {
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string Owner_Name { get; set; }

        //[Required]
        //public string UserId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string Item_Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        //[Required]
        //public string Additional_Note { get; set; }
    }
}
