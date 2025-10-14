namespace lunch_roulette.models;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    
    // Navigation properties
    public List<Lunch>? Lunches { get; set; }
}