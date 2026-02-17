using System.ComponentModel.DataAnnotations;

namespace Ip_Api.Models
{
    public class IpRecord
    {

       
            public int Id { get; set; }

            [Required]
            public string? Company { get; set; }
            public string? Ip { get; set; }
            public string? Country { get; set; }
            public string? City { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string? Lenguaje { get; set; }
            public string? TimeZone { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
