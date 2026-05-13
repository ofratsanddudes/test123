using Spw4.Exploration;
using NSubstitute;
using Xunit;    

namespace Exploration.Tests;

public class PersonService2Tests
{
    private readonly PersonService _personService;
    private readonly IPersonRepository _personRepository;

    public PersonService2Tests()
    {
        _personRepository = Substitute.For<IPersonRepository>();
        _personService = new PersonService(_personRepository);
    }


    [Fact]

    void GetAverageAge_ShouldReturnCorrectResult()
    {
        // Arrange
        var expected = 35;

        _personRepository.ReadAllPersons().Returns(new List<Person>
            {
            new(name: "Alice", age: 30),
            new(name: "Bob", age: 40),
        });

        // Act
        var actual = _personService.GetAverageAge();
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]

    void Register_WithValidNameAndAge_Succeeds()
    {
        // Arrange
        var name = "Alice";
        var age = 20;


        // Act
        _personService.Register(name, age);

        // Assert
        _personRepository.Received(1).CreatePerson(Arg.Is<Person>(p => p.Name == name && p.Age == age));
    }

    [Fact]

    void FindPerson_ReturnsCorrectResult()
    {
        // Arrange
        var name = "Alice";
        var expected = new Person(name, 30);
        _personRepository.ReadPersonByName(name).Returns(expected);

        // Act
        var actual = _personService.FindPerson(name);

        // Assert
        //Assert.Equal(expected, actual);
        Assert.Equal(expected.Name, actual?.Name);
    }


}




