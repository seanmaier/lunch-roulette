using lunch_roulette.models;
using Microsoft.EntityFrameworkCore;

namespace lunch_roulette;

public class LunchService(AppDbContext context): ILunchService
{
    public List<Lunch> GetLunches()
    {
        return context.Lunches
            .Include(l => l.Groups)
            .ThenInclude(g => g.Persons)
            .OrderBy(l => l.Date)
            .ToList();
    }

    public Lunch CreateLunch(DateTime date)
    {
        // Create lunch
        // Add Group to lunch by diving people into groups of 3-4
        // Check if persons were already in a lunch
        // Check if Group is full
        // Add person to group
        var lunch = new Lunch
        {
            Date = date,
        };
        
        var persons = context.Persons.ToList();
        var groupCount = persons.Count / 3; // Create only groups of 3 people. If modulo is not 0 add to existing group
        
        // Create the number of groups
        var groups = new List<Group>();
        for (var i = 0; i < groupCount; i++)
        {
            var group = new Group();
            
            // Add 3 persons to the group
            // SIMPLIFIED: Needs to be updated with conditions later on
            group.Persons.AddRange(persons.Take(3));
            persons.RemoveRange(0, Math.Min(3, persons.Count));
            
            groups.Add(group);
        }
        
        for (var i = 0; i < persons.Count; i++)
        {
            groups[i].Persons.Add(persons[i]);
            persons.RemoveAt(i);
        }
        
        // Add the remaining persons to the last group

        
        /*------------------ Just an Idea------------------*/
        /*while (persons.Count > 0)
        {
            
            
        }*/
        /*------------------ Just an Idea------------------*/
        
        lunch.Groups = groups;
        context.Lunches.Add(lunch);
        context.SaveChanges();
        
        return lunch;
    }
    
    public bool ResetLunches()
    {
        context.Lunches.RemoveRange(context.Lunches);
        context.SaveChanges();
        return true;
    }
}