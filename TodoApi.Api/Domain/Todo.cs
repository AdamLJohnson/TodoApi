using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Api.Domain
{
    [Table("todo")]
    public class Todo
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("task")]
        public string Task { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_complete")]
        public bool IsComplete { get; set; }
    }
}
