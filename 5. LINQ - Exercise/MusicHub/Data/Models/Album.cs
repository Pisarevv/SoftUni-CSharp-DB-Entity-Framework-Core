﻿namespace MusicHub.Data.Models;

using Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Album
{
    public Album()
    {
       this.Songs = new HashSet<Song>(); 
    }

    [Key]
    public int Id { get; set; }

    [MaxLength(ValidationConstants.MaxAlbumNameLength)]
    public string Name { get; set; } = null!;

    public DateTime ReleaseDate { get; set; }

    [NotMapped]
    public decimal Price
        => this.Songs.Sum(s => s.Price);

    [ForeignKey(nameof(Producer))]
    public int? ProducerId { get; set; }

    public virtual Producer? Producer { get; set; }

    public ICollection<Song> Songs { get; set; }
}
