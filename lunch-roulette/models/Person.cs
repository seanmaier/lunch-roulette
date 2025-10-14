using System.ComponentModel.DataAnnotations;

namespace lunch_roulette.models;

public class Person
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;
    
    // Navigation properties
    public List<Lunch>? Lunches { get; set; }
}