%% HAKUSANA: UML Class diagram (Mermaid)
classDiagram
    direction TB

    class Animal {
        <<abstract>>
        - string _name
        - int _age
        + string Name
        + int Age
        + MakeSound() string*
    }

    class Lion {
        - bool _isAlpha
        + bool IsAlpha
        + Lion(name: string, age: int, isAlpha: bool)
        + MakeSound() string
    }

    class Parrot {
        - List<string> _vocabulary
        + IReadOnlyCollection<string> Vocabulary
        + Parrot(name: string, age: int, initialWords: IEnumerable<string>?)
        + Teach(word: string) void
        + MakeSound() string
        + Fly() string
    }

    class Snake {
        - bool _isVenomous
        + bool IsVenomous
        + Snake(name: string, age: int, isVenomous: bool)
        + MakeSound() string
    }

    class IFlyable {
        <<interface>>
        + Fly() string
    }

    class IAnimalRepository {
        <<interface>>
        + Add(animal: Animal) void
        + GetAll() IReadOnlyCollection<Animal>
    }

    class InMemoryAnimalRepository {
        - List<Animal> _animals
        + Add(animal: Animal) void
        + GetAll() IReadOnlyCollection<Animal>
    }

    class ZooService {
        - IAnimalRepository _repo
        + ZooService(repo: IAnimalRepository)
        + AddAnimal(a: Animal) void
        + AnnounceAll() void
        + ShowFliers() void
    }

    %% Perintä
    Animal <|-- Lion
    Animal <|-- Parrot
    Animal <|-- Snake

    %% Rajapinnan toteutus
    IFlyable <|.. Parrot
    IAnimalRepository <|.. InMemoryAnimalRepository

    %% Riippuvuudet / käyttö
    ZooService --> IAnimalRepository : uses

    %% (Valinnainen) ohjelman entry point ei ole luokka kaaviossa
