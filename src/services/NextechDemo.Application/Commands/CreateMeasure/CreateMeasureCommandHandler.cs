using AutoMapper;
using CouncilVoting.Domain.Entities;
using CouncilVoting.Infrastructure.Respositories;
using CouncilVoting.Shared.Models;
using MediatR;

namespace CouncilVoting.Application.Commands.CreateMeasure
{
    public class CreateMeasureCommandHandler : IRequestHandler<CreateMeasureCommand, MeasureResponseModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMeasureRepository _measureRepository;
        private readonly IMapper _mapper;
        public CreateMeasureCommandHandler(IUnitOfWork unitOfWork, IMeasureRepository measureRepository, IMapper mapper)
        {
            _measureRepository = measureRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        ///     Create measure command handler handle method implemntation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MeasureResponseModel> Handle(CreateMeasureCommand request, CancellationToken cancellationToken)
        {
            var measure = new Measure(request.Subject, request.Description, request.MinimumNoOfVotesRequired);
            foreach(var item  in request.Options)
            {
                measure.AddOption(measure.Id, item);
            } 
            _measureRepository.Create(measure);
            await _unitOfWork.Save(cancellationToken);
            var result = new MeasureResponseModel
            {
                Id = measure.Id,
                Subject = measure.Subject,
                Description = measure.Description,
                MinimumNoOfVotesRequired = measure.MinimumNoOfVotesRequired,
                OptionResponse = measure.Options.Select(x => new OptionResponseModel
                {
                    Id = x.Id,
                    MeasureId = x.MeasureId,
                    Option = x.Value
                }).ToList()
            };
            return result;

        }
    }
}
