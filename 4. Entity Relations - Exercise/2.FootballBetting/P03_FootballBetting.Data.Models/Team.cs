﻿namespace P03_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

public class Team
{
    [Key]
    public int TeamId { get; set; }

    [MaxLength(ValidationConstants.MaxTeamNameLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.MaxLogoUrlLength)]
    public string? LogoUrl { get; set; }

    [MaxLength (ValidationConstants.MaxInitialsLength)]
    public string Initials { get; set; } = null!;

    public decimal Budget { get; set; }

    [ForeignKey(nameof(PrimaryKitColor))]
    public int PrimaryKitColorId { get; set; }

    public Color PrimaryKitColor { get; set; } = null!;

    [ForeignKey(nameof(SecondaryKitColor))]
    public int SecondaryKitColorId { get; set; }

    public Color SecondaryKitColor { get; set; } = null!;

    public int TownId { get; set; }

    //TODO : Add relations
}
