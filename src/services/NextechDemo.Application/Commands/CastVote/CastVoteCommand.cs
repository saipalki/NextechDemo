using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouncilVoting.Application.Commands.SaveVote
{
    /// <summary>
    ///     Cast vote command class
    /// </summary>
    public class CastVoteCommand : IRequest<string>
    {
        /// <summary>
        ///     User who is casting the vote
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        ///     Measure id for voting
        /// </summary>
        public int MeasureId { get; set; }
        /// <summary>
        ///    This tells what option a used has selected
        /// </summary>
        public int OptionId { get; set; }
    }
}
