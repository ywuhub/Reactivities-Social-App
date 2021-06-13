using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
      public class Details
      {
            public class Query : IRequest<Result<ActivityDTO>>
            {
                  public Guid Id { get; set; }
            }

            public class Handler : IRequestHandler<Query, Result<ActivityDTO>>
            {
                  private readonly DataContext _context;
                  private readonly IMapper _mapper;
                  public Handler(DataContext context, IMapper mapper)
                  {
                        _mapper = mapper;
                        _context = context;

                  }
                  public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
                  {
                        var activity = await _context.Activities
                            .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(x => x.id == request.Id);

                        return Result<ActivityDTO>.Success(activity);
                  }
            }
      }
}