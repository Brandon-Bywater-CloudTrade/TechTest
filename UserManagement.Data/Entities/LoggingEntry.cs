using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace UserManagement.Data.Entities;
public class LoggingEntry
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long? UserId { get; set; } // Not hard foreign key, as user may be deleted
    public string Action { get; set; } = default!;
    public string Changes { get; set; } = default!;
    public DateTime TimeOfChange { get; set; } = DateTime.UtcNow;

}
