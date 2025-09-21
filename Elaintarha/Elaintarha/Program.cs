using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooDemo
{
  
    public interface IFlyable
    {
        string Fly();
    }

    
    public abstract class Animal
    {
        
        private string _name;
        private int _age;

        protected Animal(string name, int age)
        {
            Name = name;  
            Age = age;
        }

        
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value.Trim();
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Age), "Age cannot be negative.");
                _age = value;
            }
        }

        
        public abstract string MakeSound();
    }

    
    public sealed class Lion : Animal
    {
        private readonly bool _isAlpha;
        public bool IsAlpha => _isAlpha;

        public Lion(string name, int age, bool isAlpha) : base(name, age)
        {
            _isAlpha = isAlpha;
        }

        public override string MakeSound() => "Roar";
    }

    public sealed class Parrot : Animal, IFlyable
    {
     
        private readonly List<string> _vocabulary = new();
        public IReadOnlyCollection<string> Vocabulary => _vocabulary.AsReadOnly();

        public Parrot(string name, int age, IEnumerable<string>? initialWords = null) : base(name, age)
        {
            if (initialWords != null) _vocabulary.AddRange(initialWords.Where(w => !string.IsNullOrWhiteSpace(w)));
        }

        public void Teach(string word)
        {
            if (!string.IsNullOrWhiteSpace(word)) _vocabulary.Add(word.Trim());
        }

        public override string MakeSound() => "Squawk";

        public string Fly() => $"{Name} flaps its wings and takes off.";
    }

    public sealed class Snake : Animal
    {
        private readonly bool _isVenomous;
        public bool IsVenomous => _isVenomous;

        public Snake(string name, int age, bool isVenomous) : base(name, age)
        {
            _isVenomous = isVenomous;
        }

        public override string MakeSound() => "Hiss";
    }

    
    public interface IAnimalRepository
    {
        void Add(Animal animal);
        IReadOnlyCollection<Animal> GetAll();
    }

    public sealed class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly List<Animal> _animals = new();

        public void Add(Animal animal)
        {
            if (animal is null) throw new ArgumentNullException(nameof(animal));
            _animals.Add(animal);
        }

        public IReadOnlyCollection<Animal> GetAll() => _animals.AsReadOnly();
    }


    public sealed class ZooService
    {
        private readonly IAnimalRepository _repo;

        public ZooService(IAnimalRepository repo) // DI konstruktorissa
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void AddAnimal(Animal a) => _repo.Add(a);

        public void AnnounceAll()
        {
            foreach (var a in _repo.GetAll())
            {
                Console.WriteLine($"{a.Name} ({a.GetType().Name}, {a.Age} v) says: {a.MakeSound()}");
            }
        }

        public void ShowFliers()
        {
            foreach (var a in _repo.GetAll())
            {
                if (a is IFlyable f)
                {
                    Console.WriteLine(f.Fly());
                }
            }
        }
    }


    internal static class Program
    {
        private static void Main()
        {
            IAnimalRepository repo = new InMemoryAnimalRepository();
            var zoo = new ZooService(repo);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n== Zoo Menu ==");
                Console.WriteLine("1. Lisää leijona");
                Console.WriteLine("2. Lisää papukaija");
                Console.WriteLine("3. Lisää käärme");
                Console.WriteLine("4. Näytä kaikki eläimet");
                Console.WriteLine("5. Lopeta");
                Console.Write("Valinta: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Anna nimi: ");
                        string lionName = Console.ReadLine();
                        Console.Write("Anna ikä: ");
                        int lionAge = int.Parse(Console.ReadLine());
                        Console.Write("Onko alfayksilö? (k/e): ");
                        bool isAlpha = Console.ReadLine().ToLower() == "k";
                        zoo.AddAnimal(new Lion(lionName, lionAge, isAlpha));
                        break;

                    case "2":
                        Console.Write("Anna nimi: ");
                        string parrotName = Console.ReadLine();
                        Console.Write("Anna ikä: ");
                        int parrotAge = int.Parse(Console.ReadLine());
                        zoo.AddAnimal(new Parrot(parrotName, parrotAge));
                        break;

                    case "3":
                        Console.Write("Anna nimi: ");
                        string snakeName = Console.ReadLine();
                        Console.Write("Anna ikä: ");
                        int snakeAge = int.Parse(Console.ReadLine());
                        Console.Write("Onko myrkyllinen? (k/e): ");
                        bool isVenomous = Console.ReadLine().ToLower() == "k";
                        zoo.AddAnimal(new Snake(snakeName, snakeAge, isVenomous));
                        break;

                    case "4":
                        zoo.AnnounceAll();
                        zoo.ShowFliers();
                        break;

                    case "5":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Virheellinen valinta!");
                        break;
                }
            }

            Console.WriteLine("Ohjelma päättyi.");
        }

    }
}
