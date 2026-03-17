using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class Favorite
{
    [Key]
    public int Id { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.Now;

    public string ?UserId { get; set; }
    [ForeignKey("UserId")]
    public User ?User { get; set; }

    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie ?Movie { get; set; }
}