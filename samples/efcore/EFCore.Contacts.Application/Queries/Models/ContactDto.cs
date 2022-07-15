using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Contacts.Application.Queries.Models
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description  { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
