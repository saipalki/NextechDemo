using CouncilVoting.Shared.Models;
using MediatR;

namespace CouncilVoting.Application.Queries.GetVotingResult
{
    /// <summary>
    ///     Get voting result query
    /// </summary>
    public class GetVotingResultQuery : IRequest<VotingResultResponseModel>
    {
        /// <summary>
        ///     Measure id
        /// </summary>
        public int MeasureId { get; set; }
    }
}
