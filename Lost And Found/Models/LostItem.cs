using System.ComponentModel.DataAnnotations.Schema;

namespace Lost_And_Found.Models
{
    public class LostItem
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public string Owner_Name { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string Item_Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string Additional_Note { get; set; }
    }
}
