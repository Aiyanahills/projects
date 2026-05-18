namespace EXAM.Models
{
    public class ContactForm
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Topic { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}