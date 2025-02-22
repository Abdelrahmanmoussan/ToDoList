namespace ToDo.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Img { get; set; }

        public string? Pdf { get; set; }

    }
}
