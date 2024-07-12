
using System.Text;
using System.Text.Json.Serialization;
using CodeFirstExample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace CodeFirstExample
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        static bool MainMenu()
        {
            using (var db = new MyAppContext())
            {
                Console.WriteLine("Что Вы хотите сделать?");
                Console.WriteLine("1 - Добавить человека с собакой в базу данных");
                Console.WriteLine("2 - Редактировать человека в базе данных");
                Console.WriteLine("3 - Удалить человека из базы данных");
                Console.WriteLine("4 - Показать информацию о людях в базе данных");
                Console.WriteLine("5 - Выйти из приложения");

                switch (Console.ReadLine())
                {
                    case "1": //добавление
                        Console.Clear();
                        
                        var namePerson = GetInputLine("Введите имя человека");
                        var surnamePerson = GetInputLine("Введите фамилию человека");
                        
                        var person1 = new Person(Guid.NewGuid(), namePerson, surnamePerson);

                        var namePet = GetInputLine("Введите кничку животного");
                        var typeOfPet = GetInputLine("Введите тип животного");

                        var pet1 = new Pet(Guid.NewGuid(), typeOfPet, namePet);
                        
                        person1.Pets.Add(pet1);
                        
                        db.Persons.Add(person1);
                        db.SaveChanges();
                        
                        return true;
                    case "2": //редакт
                        Console.Clear();
                        var inputPersonId = Guid.Parse(GetInputLine("Введите id человека, которого хотите редактировать"));
                        
                        var editingPersonId = db.Persons.FirstOrDefault(p => p.Id == inputPersonId);
                        if (editingPersonId.Id != inputPersonId)
                        {
                            Console.WriteLine("Данного id не существует в базе данных");
                        }
                        
                        var newNamePerson = GetInputLine("Введите новое имя человека");
                        var newSurnamePerson = GetInputLine("Введите новую фамилию человека");

                        var person2 = db.Persons.Find(inputPersonId);
                        
                        person2.Name = newNamePerson;
                        person2.Surname = newSurnamePerson;
                        
                        db.Persons.Update(person2);
                        db.SaveChanges();
                        
                        return true;
                    case "3": //удаление
                        Console.Clear();
                        
                        var idForDelete = Guid.Empty;
                        if (Guid.TryParse(GetInputLine("Введите id человека, которого хотите удалить из базы данных"), out Guid result))
                        {
                            idForDelete = result;
                        }
                        
                        db.Persons.Where(p=>p.Id == idForDelete).ExecuteDelete();
                            
                        return true;
                    case "4":
                        Console.Clear();
                        var sb = new StringBuilder();
                        var count = 0;
                        foreach (var per in db.Persons.Include(p => p.Pets))
                        {
                            sb.AppendLine($"\n##### BEGIN {count+1} #####");
                            sb.AppendLine($"Идентификатор: {per.Id}");
                            sb.AppendLine($"ФИО: {per.Name} {per.Surname}");
                            sb.AppendLine("Животные:");
                            sb.AppendJoin("\n", per.Pets.Select(p=> $"\tИдентификатор: {p.Id}; Тип: {p.TypeOfPet}; Кличка: {p.PetNickname};"));
                            sb.AppendLine($"\n##### END {count+1} #####");
                            Console.WriteLine(sb.ToString());
                            sb.Clear();
                        }
                        
                        return true;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Благодарим за использование нашего приложения!");
                        return false;
                    default:
                        Console.Clear();
                        Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                        return true;
                }
            }
        }

        static string? GetInputLine(string messsage)
        {
            Console.WriteLine(messsage);
            return Console.ReadLine();
        }
    }
}

