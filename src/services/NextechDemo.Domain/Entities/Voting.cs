namespace CouncilVoting.Domain.Entities
{
    public class Voting : BaseEntity
    {
        public int UserId { get; set; }
        public int MeasureId { get; set; }
        public int OptionId { get; set; }
    }
}
