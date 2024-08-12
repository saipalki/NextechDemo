using CouncilVoting.Infrastructure.Persistance;
using CouncilVoting.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CouncilVoting.Application.Queries.GetVotingResult
{
    public class GetVotingResultQueryHandler : IRequestHandler<GetVotingResultQuery, VotingResultResponseModel>
    {
        private readonly CouncilVotingContext _context;
        public GetVotingResultQueryHandler(CouncilVotingContext context)
        {
            _context = context;
        }
        /// <summary>
        ///     Get voting result query handler method implementation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<VotingResultResponseModel> Handle(GetVotingResultQuery request, CancellationToken cancellationToken)
        {
            var votings = await _context.Votings.Where(x => x.MeasureId == request.MeasureId).ToListAsync();

            var users = await (from v in _context.Votings
                        join user in _context.Users
                        on v.UserId equals user.Id
                        join option in _context.Options
                        on v.OptionId equals option.Id
                        where v.MeasureId == request.MeasureId
                        select new UserResponseModel
                        {
                            Options = new OptionResponseModel
                            {
                                Id = option.Id,
                                Option = option.Value,
                                MeasureId = request.MeasureId,
                            },
                            Email = user.Email,
                            Name = user.Name,
                            UserId = user.Id,
                        }).ToListAsync(cancellationToken: cancellationToken);

            var measures = await (from m in _context.Measures
                                  join o in _context.Options
                                  on m.Id equals o.MeasureId
                                  select new MeasureResponseModel
                                  {
                                      Id = m.Id,
                                      Subject = m.Subject,
                                      Description = m.Description,
                                      MinimumNoOfVotesRequired = m.MinimumNoOfVotesRequired,
                                      OptionResponse = m.Options.Select(x => new OptionResponseModel
                                      {
                                          Id = x.Id,
                                          MeasureId = x.MeasureId,
                                          Option = x.Value
                                      }).ToList()!
                                  }).FirstOrDefaultAsync();

            var response = new VotingResultResponseModel
            {
                TotalNoOfVotes = votings.Count,
                IsPassed = false,
                Measure = measures!,
                Users = users,
            };
            return response!;
        }
    }
}
