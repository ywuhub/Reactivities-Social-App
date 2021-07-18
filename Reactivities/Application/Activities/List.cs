using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
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
                  private readonly IUsernameAccessor _useraccessor;
                  public Handler(DataContext context, IMapper mapper, IUsernameAccessor useraccessor)
                  {
                        _useraccessor = useraccessor;
                        _mapper = mapper;
                        _context = context;
                  }

                  public async Task<Result<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
                  {
                        var activities = await _context.Activities
                                                    .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new {currentUsername = _useraccessor.GetUsername()})
                                                    .ToListAsync(cancellationToken);

                        return Result<List<ActivityDTO>>.Success(activities);
                  }
            }
      }
}