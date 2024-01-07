using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Api.Todos
{
    [Table("todo")]
    public class Todo
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("task")]
        public string Task { get; set; } = string.Empty;
        [Column("description")]
        public string Description { get; set; } = string.Empty;
        [Column("is_complete")]
        public bool IsComplete { get; set; }
    }
}
