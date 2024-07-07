using System.ComponentModel.DataAnnotations;

namespace CodeFirstExample;

public class Pet
{
    public Guid Id { get; set; }
    public string TypeOfPet { get; set; }
    public string PetNickname { get; set; }

    public Guid PersonId { get; set; } // внешний ключ
    
    public Person Person { get; set; } // навигационное свойство

    public Pet(Guid id, string typeOfPet, string petNickname)
    {
        Id = id;
        TypeOfPet = typeOfPet;
        PetNickname = petNickname;
    }
}