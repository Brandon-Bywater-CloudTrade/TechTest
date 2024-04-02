using System;
using Bogus;
using UserManagement.Data.Entities;

namespace UserManagement.Data.Tests.Models;
public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(x => x.IsActive, f => f.Random.Bool());
        RuleFor(x => x.Forename, f => f.Person.FirstName);
        RuleFor(x => x.Surname, f => f.Person.LastName);
        RuleFor(x => x.Email, f => f.Person.Email);
        RuleFor(x => x.DateOfBirth, f => DateOnly.FromDateTime(f.Person.DateOfBirth));
    }
}
