using System;
using System.Collections.Generic;

interface IFeedable
{
    void Feed(); 
}

abstract class Animal : IFeedable
{
    public string Name { get; private set; }
    public int Age { get; private set; }
    public double Weight { get; private set; }

    public Animal(string name, int age, double weight)
    {
        Name = name;
        Age = age;
        Weight = weight;
    }

    public abstract void MakeSound();

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Имя: {Name}, Возраст: {Age} лет, Вес: {Weight} кг");
    }

    public virtual void Feed()
    {
        Console.WriteLine($"{Name} покормлено.");
    }

    public static bool operator >(Animal a, Animal b)
    {
        return a.Age > b.Age;
    }

    public static bool operator <(Animal a, Animal b)
    {
        return a.Age < b.Age;
    }
}

class Mammal : Animal
{
    public Mammal(string name, int age, double weight) : base(name, age, weight) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} — Млекопитающее издает звук: Мууу");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine("Это млекопитающее.");
    }
}

class Bird : Animal
{
    public Bird(string name, int age, double weight) : base(name, age, weight) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} — Птица издает звук: Чирик-чирик");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine("Это птица.");
    }
}

class Reptile : Animal
{
    public Reptile(string name, int age, double weight) : base(name, age, weight) { }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} — Рептилия издает звук: оооооо");
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine("Это рептилия.");
    }
}
class Zoo
{

    private List<Animal> animals = new List<Animal>();

    public IReadOnlyList<Animal> Animals => animals.AsReadOnly();

    public void AddAnimal(Animal animal)
    {
        animals.Add(animal);
        Console.WriteLine($"{animal.Name} добавлено в зоопарк.");
    }

    public void RemoveAnimal(string name)
    {
        var animal = animals.Find(a => a.Name == name);
        if (animal != null)
        {
            animals.Remove(animal);
            Console.WriteLine($"{name} удалено из зоопарка.");
        }
        else
        {
            Console.WriteLine($"Животное с именем {name} не найдено.");
        }
    }

    public void DisplayAllAnimals()
    {
        Console.WriteLine("Информация о животных в зоопарке:");
        foreach (var animal in animals)
        {
            animal.DisplayInfo();
            Console.WriteLine(); 
        }
    }

    public void AllAnimalsMakeSound()
    {
        foreach (var animal in animals)
        {
            animal.MakeSound();
        }
    }

    public void FeedAllAnimals()
    {
        foreach (var animal in animals)
        {
            animal.Feed();
        }
    }
}

class ZooKeeper
{
    public void TakeCare(Animal animal)
    {
        Console.WriteLine($"Смотритель ухаживает за {animal.Name}.");

    }

    public void CompareAnimals(Animal a, Animal b)
    {
        if (a > b)
            Console.WriteLine($"{a.Name} старше {b.Name}");
        else if (a < b)
            Console.WriteLine($"{a.Name} моложе {b.Name}");
        else
            Console.WriteLine($"{a.Name} и {b.Name} одного возраста");
    }
}


static class ZooStatistics
{
    private static int totalAnimals = 0;

    public static void AddAnimal()
    {
        totalAnimals++;
    }

    public static void ShowStatistics()
    {
        Console.WriteLine($"Всего животных в зоопарке: {totalAnimals}");
    }
}

class Program
{
    static void Main()
    {
        Zoo zoo = new Zoo();

        Animal lion = new Mammal("Лев", 5, 190);
        Animal eagle = new Bird("Орёл", 3, 6);
        Animal snake = new Reptile("К snake", 2, 2);

        zoo.AddAnimal(lion);
        zoo.AddAnimal(eagle);
        zoo.AddAnimal(snake);

        ZooStatistics.AddAnimal();
        ZooStatistics.AddAnimal();
        ZooStatistics.AddAnimal();

        zoo.DisplayAllAnimals();

        Console.WriteLine("Звуки животных:");
        zoo.AllAnimalsMakeSound();

        Console.WriteLine("Кормим животных:");
        zoo.FeedAllAnimals();

        ZooKeeper keeper = new ZooKeeper();
        keeper.TakeCare(lion);
        keeper.TakeCare(eagle);

        keeper.CompareAnimals(lion, eagle);
        keeper.CompareAnimals(eagle, snake);

        ZooStatistics.ShowStatistics();

        Console.WriteLine($"Лев старше орла? {(lion > eagle ? "Да" : "Нет")}");
        Console.WriteLine($"Код завершён.");
    }
}
