using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyXemPhim.Models.Entities;

public class Episode
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ?Title { get; set; } // Tập 1, Tập 2...

    [Required]
    public int EpisodeNumber { get; set; } // Số tập (để sắp xếp)

    [Required]
    public string ?VideoUrl { get; set; } // Link phim

    public int Duration { get; set; }

    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie ?Movie { get; set; }
}