using lunch_roulette.models;
using Microsoft.EntityFrameworkCore;

namespace lunch_roulette;

public class LunchService(AppDbContext context) : ILunchService
{
    public List<Lunch> GetLunches()
    {
        return context.Lunches
            .Include(l => l.Groups)
            .ThenInclude(g => g.Members)
            .ThenInclude(m => m.Person)
            .OrderBy(l => l.Date)
            .ToList();
    }

    public Lunch CreateLunch(DateTime date)
    {
        var persons = context.Persons.ToList();

        var unassigned = new List<Person>(persons); // Used to keep track of unassigned persons

        var pairCounts = GetKeyPairCount(); // Dictionary for pairs

        var groups = new List<List<Person>>(); // Final list of groups
        while (unassigned.Count > 0)
        {
            if (unassigned.Count == 3)
            {
                var lastGroup = new List<Person>(unassigned);
                groups.Add(lastGroup);
                unassigned.Clear();
                break;
            }

            var personA = unassigned.First();
            unassigned.Remove(personA);

            var possiblePairings = unassigned // Get all unassigned persons that are different from personA
                .OrderBy(p => pairCounts.GetValueOrDefault(GetPairKey(personA.Id, p.Id), 0)) // Order by the number of times they have been paired
                .ToList();

            var partner = possiblePairings.First();
            unassigned.Remove(partner);

            var group = new List<Person> { personA, partner };
            groups.Add(group);
        }

        var lunch = new Lunch { Date = date }; // Setup lunch

        foreach (var group in groups) // Fill lunch with groups
        {
            var lunchGroup = new Group { Lunch = lunch };
            foreach (var person in group) // Fill groups with persons
            {
                lunchGroup.Members.Add(new GroupMembers { Person = person });
            }

            lunch.Groups.Add(lunchGroup);
        }

        context.Lunches.Add(lunch);
        context.SaveChanges();

        return lunch;
    }

    private Dictionary<string, int> GetKeyPairCount()
    {
        var pairs = new Dictionary<string, int>(); // Dictionary for pairs as keys and count as values

        var allGroups = context.Groups
            .Include(g => g.Members)
            .ThenInclude(m => m.Person)
            .ToList();

        foreach (var group in allGroups)
        {
            var person = group.Members.Select(m => m.Person.Id).ToList(); // Collect just the ids to create key pairs

            for (int i = 0; i < person.Count; i++)
            {
                for (int j = i + 1; j < person.Count; j++)
                {
                    var key = GetPairKey(person[i], person[j]); // Creates a key like a-b or b-a
                    pairs[key] =
                        pairs.GetValueOrDefault(key, 0) +
                        1; // If the key already exists, add 1 to the count otherwise set it to 0
                }
            }
        }

        return pairs;
    }

    private string GetPairKey(int a, int b) =>
        a > b ? $"{a}-{b}" : $"{b}-{a}";

    public bool ResetLunches()
    {
        context.Lunches.RemoveRange(context.Lunches);
        context.SaveChanges();
        return true;
    }
}