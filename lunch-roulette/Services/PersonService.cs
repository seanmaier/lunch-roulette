using lunch_roulette.models;

namespace lunch_roulette;

public class PersonService(AppDbContext context): IPersonService
{
    public Person CreatePerson(Person person)
    {
        context.Persons.Add(person);
        context.SaveChanges();
        
        return person;
    }

    public Person UpdatePerson(Person person)
    {
        context.Persons.Update(person);
        context.SaveChanges();
        
        return person;
    }

    public bool DeletePerson(int id)
    {
        var person = context.Persons.Find(id);
        if (person == null) 
            return false;
        
        context.Persons.Remove(person);
        context.SaveChanges();
        return true;
    }

    public List<Person> GetPersons()
    {
        return context.Persons.ToList();
    }
}