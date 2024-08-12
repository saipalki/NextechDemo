namespace CouncilVoting.Domain.Entities
{
    public class Option : BaseEntity
    {
        public int MeasureId { get; set; }
        public string Value { get; set; }
        public Option(int measureId, string value)
        {
            MeasureId = measureId;
            Value = value;
        }
    }
}
