
namespace MiTube.BLL.DTO
{
    public class InteractionDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid VideoId { get; set; }
        public int Actionstate { get; set; }
        public DateTime? Date { get; set; }
    }
}
