using AutoMapper;
using CouncilVoting.Domain.Entities;
using CouncilVoting.Infrastructure.Respositories;
using MediatR;

namespace CouncilVoting.Application.Commands.SaveVote
{
    public class CastVoteCommandHandler : IRequestHandler<CastVoteCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVotingRepository _votingRepository;
        private readonly IMapper _mapper;
        public CastVoteCommandHandler(IUnitOfWork unitOfWork, IVotingRepository votingRepository, IMapper mapper)
        {
            _votingRepository = votingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        ///     Cast vote command handler handle method implemntation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> Handle(CastVoteCommand request, CancellationToken cancellationToken)
        {
            var voting = _mapper.Map<Voting>(request);
            
            _votingRepository.Create(voting);
            var result = await _unitOfWork.Save(cancellationToken);
            if(result)
            {
                return "Your vote is successfully submitted";
            }
            else
            {
                return "There is some issue at this moment, please try later";
            }
        }
    }
}
