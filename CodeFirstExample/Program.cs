
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
                        
                        var newPerson = new Person(Guid.NewGuid(), namePerson, surnamePerson);

                        var namePet = GetInputLine("Введите кличку животного");
                        var typeOfPet = GetInputLine("Введите тип животного");

                        var newPet = new Pet(Guid.NewGuid(), typeOfPet, namePet);
                        
                        newPerson.Pets.Add(newPet);
                        
                        db.Persons.Add(newPerson);
                        db.SaveChanges();
                        
                        return true;
                    case "2": //редакт
                        Console.Clear();
                        var inputPersonId = Guid.Parse(GetInputLine("Введите id человека, которого хотите редактировать"));
                        
                        var editingPerson = db.Persons.FirstOrDefault(p => p.Id == inputPersonId);
                        
                        if (editingPerson == null)
                        {
                            Console.WriteLine("Данного id не существует в базе данных");
                            return true;
                        }
                        
                        var newNamePerson = GetInputLine("Введите новое имя человека");
                        var newSurnamePerson = GetInputLine("Введите новую фамилию человека");
                        
                        editingPerson.Name = newNamePerson;
                        editingPerson.Surname = newSurnamePerson;
                        
                        db.SaveChanges();
                        
                        return true;
                    case "3": //удаление
                        Console.Clear();
                        
                        if (Guid.TryParse(GetInputLine("Введите id человека, которого хотите удалить из базы данных"), out var idForDelete))
                        {
                            var deletedCount = db.Persons.Where(p=>p.Id == idForDelete).ExecuteDelete();
                            if (deletedCount == 0)
                            {
                                Console.WriteLine("Такого человека с id не существует");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат id");
                        }
                            
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
                            count++;
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

