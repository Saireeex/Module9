using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Класс Student представляет студента с его ID, именем, фамилией, возрастом и группой
public class Student
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Group { get; set; }

    public override string ToString()
    {
        return $"ID: {ID}, Имя: {FirstName}, Фамилия: {LastName}, Возраст: {Age}, Группа: {Group}";
    }
}

// Класс StudentManager управляет списком студентов, предоставляет методы для добавления, удаления,редактирования, поиска, сортировки студентов, а также для сохранения и загрузки данных из файла
public class StudentManager
{
    private List<Student> students = new List<Student>(); // список студентов
    private const string FilePath = "students.txt"; 

    // Метод для добавления нового студента в список
    public void AddStudent(Student student)
    {
        student.ID = students.Count > 0 ? students.Max(s => s.ID) + 1 : 1; // автоприсвоение ID
        students.Add(student);
    }

    // Метод для удаления студента по ID
    public void RemoveStudent(int id)
    {
        var student = students.FirstOrDefault(s => s.ID == id);
        if (student != null)
        {
            students.Remove(student);
            Console.WriteLine("Студент удален.");
        }
        else
        {
            Console.WriteLine("Студент с таким ID не найден.");
        }
    }

    // Метод для редактирования информации о студенте
    public void EditStudent(int id, string firstName, string lastName, int age, string group)
    {
        var student = students.FirstOrDefault(s => s.ID == id);
        if (student != null)
        {
            student.FirstName = firstName;
            student.LastName = lastName;
            student.Age = age;
            student.Group = group;
            Console.WriteLine("Информация о студенте обновлена.");
        }
        else
        {
            Console.WriteLine("Студент с таким ID не найден.");
        }
    }

    // Метод для поиска студентов по имени или фамилии
    public List<Student> SearchStudents(string term)
    {
        return students.Where(s =>
            s.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            s.LastName.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Метод для сортировки студентов по выбранному критерию
    public List<Student> SortStudents(string criteria)
    {
        return criteria.ToLower() switch
        {
            "имя" => students.OrderBy(s => s.FirstName).ToList(),
            "фамилия" => students.OrderBy(s => s.LastName).ToList(),
            "возраст" => students.OrderBy(s => s.Age).ToList(),
            "группа" => students.OrderBy(s => s.Group).ToList(),
            _ => students
        };
    }

    // Метод для сохранения списка студентов в файл
    public void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter(FilePath))
        {
            foreach (var student in students)
            {
                writer.WriteLine($"{student.ID},{student.FirstName},{student.LastName},{student.Age},{student.Group}");
            }
        }
        Console.WriteLine("Данные успешно сохранены в файл.");
    }

    // Метод для загрузки списка студентов из файла
    public void LoadFromFile()
    {
        if (File.Exists(FilePath))
        {
            students.Clear();
            var lines = File.ReadAllLines(FilePath);

            foreach (var line in lines)
            {
                var data = line.Split(',');
                if (data.Length == 5 &&
                    int.TryParse(data[0], out int id) &&
                    int.TryParse(data[3], out int age))
                {
                    students.Add(new Student
                    {
                        ID = id,
                        FirstName = data[1],
                        LastName = data[2],
                        Age = age,
                        Group = data[4]
                    });
                }
            }
            Console.WriteLine("Данные успешно загружены из файла.");
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }

    // Метод для вывода всех студентов в консоль
    public void PrintAllStudents()
    {
        foreach (var student in students)
        {
            Console.WriteLine(student);
        }
    }
}

// Основной класс Program, содержащий меню и методы для взаимодействия с пользователем
public class Program
{
    public static void Main()
    {
        var manager = new StudentManager();
        manager.LoadFromFile(); // Загрузка данных из файла при запуске программы

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Удалить студента");
            Console.WriteLine("3. Редактировать студента");
            Console.WriteLine("4. Найти студента");
            Console.WriteLine("5. Сортировать студентов");
            Console.WriteLine("6. Показать всех студентов");
            Console.WriteLine("7. Сохранить в файл");
            Console.WriteLine("8. Выйти");

            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent(manager);
                    break;
                case "2":
                    RemoveStudent(manager);
                    break;
                case "3":
                    EditStudent(manager);
                    break;
                case "4":
                    SearchStudent(manager);
                    break;
                case "5":
                    SortStudents(manager);
                    break;
                case "6":
                    manager.PrintAllStudents();
                    break;
                case "7":
                    manager.SaveToFile();
                    break;
                case "8":
                    return; // Завершение программы
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }

    // Метод для добавления студента через консоль
    static void AddStudent(StudentManager manager)
    {
        Console.Write("Введите имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        var lastName = Console.ReadLine();
        Console.Write("Введите возраст: ");
        var age = int.Parse(Console.ReadLine());
        Console.Write("Введите группу: ");
        var group = Console.ReadLine();

        var student = new Student { FirstName = firstName, LastName = lastName, Age = age, Group = group };
        manager.AddStudent(student);
    }

    // Метод для удаления студента через консоль
    static void RemoveStudent(StudentManager manager)
    {
        Console.Write("Введите ID студента: ");
        var id = int.Parse(Console.ReadLine());
        manager.RemoveStudent(id);
    }

    // Метод для редактирования информации о студенте через консоль
    static void EditStudent(StudentManager manager)
    {
        Console.Write("Введите ID студента: ");
        var id = int.Parse(Console.ReadLine());
        Console.Write("Введите новое имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Введите новую фамилию: ");
        var lastName = Console.ReadLine();
        Console.Write("Введите новый возраст: ");
        var age = int.Parse(Console.ReadLine());
        Console.Write("Введите новую группу: ");
        var group = Console.ReadLine();

        manager.EditStudent(id, firstName, lastName, age, group);
    }

    // Метод для поиска студента через консоль
    static void SearchStudent(StudentManager manager)
    {
        Console.Write("Введите имя или фамилию для поиска: ");
        var term = Console.ReadLine();
        var results = manager.SearchStudents(term);
        results.ForEach(Console.WriteLine);
    }

    // Метод для сортировки студентов через консоль
    static void SortStudents(StudentManager manager)
    {
        Console.Write("Сортировать по (имя, фамилия, возраст, группа): ");
        var criteria = Console.ReadLine();
        var sortedList = manager.SortStudents(criteria);
        sortedList.ForEach(Console.WriteLine);
    }
}
