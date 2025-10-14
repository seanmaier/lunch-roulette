
using lunch_roulette.models;

public class Lunch
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    
    // Navigation properties
    public List<Group> Groups { get; set; } = [];
}