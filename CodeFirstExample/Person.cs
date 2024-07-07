using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstExample;

public class Person
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public List<Pet> Pets { get; set; } = new List<Pet>();

    public Person(Guid id, string name, string surname)
    {
        Id = id;
        Name = name;
        Surname = surname;
    }
}