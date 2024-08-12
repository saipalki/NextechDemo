namespace CouncilVoting.Domain.Entities
{
    public class Measure : BaseEntity
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public int MinimumNoOfVotesRequired { get; set; }

        private readonly List<Option> _optionItems;
        public IReadOnlyCollection<Option> Options => _optionItems;

        public Measure()
        {
            _optionItems = new List<Option>();
        }

        public Measure(string subject, string description, int minimumNoOfVotesRequired) : this()
        {
            Subject = subject;
            Description = description;
            MinimumNoOfVotesRequired = minimumNoOfVotesRequired;
        }
        /// <summary>
        ///     Add option for a measure
        /// </summary>
        /// <param name="measureId"></param>
        /// <param name="optionValue"></param>
        public void AddOption(int measureId, string optionValue)
        {
            var option = new Option(measureId, optionValue);
            _optionItems.Add(option);
        }
    }
}
