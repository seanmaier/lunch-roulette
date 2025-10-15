using lunch_roulette.models;

namespace lunch_roulette;

public static class AppDbSeeder
{
    public static void SeedFromCsv(AppDbContext context, string csvFilePath)
    {
        if (!File.Exists(csvFilePath))
            throw new FileNotFoundException("File not found", csvFilePath);
        
        var personsFromCsv = new List<Person>();
        
        using var reader = new StreamReader(csvFilePath);
        reader.ReadLine(); // Skip header or first line
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null) continue;
            var values = line.Split(';');

            string name = values[0];
            string department = values[1];
            string jobTitle = values[2];

            var newPerson = new Person { Name = name, Department = department, JobTitle = jobTitle };
            personsFromCsv.Add(newPerson);
        }
        
        // Check if the person already exists in the database via name
        var existingNames = context.Persons.Select(p => p.Name).ToHashSet();
        var newPersons = personsFromCsv.Where(p => !existingNames.Contains(p.Name)).ToList();
        context.Persons.AddRange(newPersons);
        context.SaveChanges();
    }
}