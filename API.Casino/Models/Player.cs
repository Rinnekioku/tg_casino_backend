using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Casino.Models;

public class Player
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(256)]
    public string TelegramUsername { get; init; } = null!;

    [Column(TypeName = "VARCHAR")]
    [StringLength(36)]
    public string ReferralCode { get; init; } = null!;

    [Column(TypeName = "int")]
    public int Score { get; set; }

    [Column(TypeName = "int")]
    public int Spins { get; set; }
}