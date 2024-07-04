using System.ComponentModel.DataAnnotations;

namespace Lost_And_Found.Admin
{
    public class Admin
    {
        public int ID { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
