using System;
using System.ComponentModel.DataAnnotations.Schema;
using VerticalToDo.Core.Features.Accounts;

namespace VerticalToDo.Core.Features.ToDos
{
    public class ToDo : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
