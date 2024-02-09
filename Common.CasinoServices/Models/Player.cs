using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.CasinoServices.Models;

public class Player
{
    [Key]
    public int Id { get; set; }

    public string Username { get; set; } = null!;
    
    [Column(TypeName = "int")]
    public int Score { get; set; }
}