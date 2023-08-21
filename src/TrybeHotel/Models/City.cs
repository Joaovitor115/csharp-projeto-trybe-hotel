namespace TrybeHotel.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // 1. Implemente as models da aplicação
    public class City {
[Key]
        public int CityId { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        [InverseProperty("City")]
        public ICollection<Hotel>? Hotels { get; set; }
    }
}