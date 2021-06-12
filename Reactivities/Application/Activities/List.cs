using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
      public class List
      {
            public class Query : IRequest<Result<List<ActivityDTO>>> { }

            public class Handler : IRequestHandler<Query, Result<List<ActivityDTO>>>
            {
                  private readonly DataContext _context;
                  private readonly IMapper _mapper;
                  public Handler(DataContext context, IMapper mapper)
                  {
                        _mapper = mapper;
                        _context = context;
                  }

                  public async Task<Result<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
                  {
                        var activities = await _context.Activities
                                                    .Include(a => a.Attendees)
                                                    .ThenInclude(u => u.AppUser)
                                                    .ToListAsync(cancellationToken);

                        var activitiesToReturn = _mapper.Map<List<ActivityDTO>>(activities);

                        return Result<List<ActivityDTO>>.Success(activitiesToReturn);
                  }
            }
      }
}