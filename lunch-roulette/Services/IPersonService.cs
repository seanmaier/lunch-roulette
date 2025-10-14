using lunch_roulette.models;

namespace lunch_roulette;

public interface IPersonService
{
    Person CreatePerson(Person person);
    Person UpdatePerson(Person person);
    bool DeletePerson(int id);
    List<Person> GetPersons();
}