using System.ComponentModel.DataAnnotations;

namespace lunch_roulette.models;

public class Group
{
    public int Id { get; set; }
    public int LunchId { get; set; }
    
    // Navigation properties
    public Lunch Lunch { get; set; }
    public List<GroupMembers> Members { get; set; } = [];
}