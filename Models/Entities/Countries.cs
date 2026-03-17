using QuanLyXemPhim.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace QuanLyXemPhim.Models.Entities;

public class Countries
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ?Name { get; set; }

    [Required]
    [MaxLength(20)]
    public string ?Code { get; set; } // VN, US, KR...

    public ICollection<Movie> ?Movies { get; set; }
}