using System.ComponentModel.DataAnnotations.Schema;

namespace lunch_roulette.models;

public class GroupMembers
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public int PersonId { get; set; }
    
    // Navigation properties
    [ForeignKey("GroupId")]
    public Group Group { get; set; }
    [ForeignKey("PersonId")]
    public Person Person { get; set; }
}