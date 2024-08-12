using CouncilVoting.Shared.Models;
using MediatR;

namespace CouncilVoting.Application.Commands.CreateMeasure
{
    /// <summary>
    ///     Create mesaure command
    /// </summary>
    public class CreateMeasureCommand : IRequest<MeasureResponseModel>
    {
        /// <summary>
        ///     Subject of measure
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        ///     Description of measures
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///     Min no of votes required to pass the poll
        /// </summary>
        public int MinimumNoOfVotesRequired { get; set; }
        /// <summary>
        ///     List of options on a measure
        /// </summary>
        public List<string> Options { get; set; }
    }
}
