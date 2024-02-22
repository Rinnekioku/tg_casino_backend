using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Casino.Models;

public class Player
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(256)]
    public required string TelegramUsername { get; init; } = null!;

    [Column(TypeName = "VARCHAR")]
    [StringLength(36)]
    public required string ReferralCode { get; init; } = null!;

    [Column(TypeName = "int")]
    public required int Score { get; set; }

    [Column(TypeName = "int")]
    public required int Spins { get; set; }

    public ICollection<Player> Referrals { get; set; }
    
    public int? ReferrerId { get; set; }
    public Player? Referrer { get; set; }

    public required bool IsSetupComplete { get; set; }
    
    [Column(TypeName = "VARCHAR")]
    [StringLength(36)]
    public required string ReferralStatus { get; set; } = null!;
}