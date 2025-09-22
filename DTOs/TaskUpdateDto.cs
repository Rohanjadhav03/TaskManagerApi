namespace TaskManagerApi.DTOs
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; } = 3;
    }
}