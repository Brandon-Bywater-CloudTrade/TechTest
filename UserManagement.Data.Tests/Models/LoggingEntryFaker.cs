
using Bogus;
using UserManagement.Data.Entities;

namespace UserManagement.Data.Tests.Models;
public class LoggingEntryFaker : Faker<LoggingEntry>
{
    public LoggingEntryFaker()
    {
        RuleFor(x => x.UserId, f => f.Random.Long(1, 1000));
        RuleFor(x => x.Action, f => f.Lorem.Word());
        RuleFor(x => x.Changes, f => f.Lorem.Sentence());
        RuleFor(x => x.TimeOfChange, f => f.Date.Past());
    }
}
