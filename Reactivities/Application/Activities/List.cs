using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Activities;
using Application.Core;
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
            public class Query : IRequest<Result<PagedList<ActivityDTO>>> 
            { 
                  public PagingParams Params { get; set; }
            }

            public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDTO>>>
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

                  public async Task<Result<PagedList<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
                  {
                        var query =  _context.Activities
                                          .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider, new {currentUsername = _useraccessor.GetUsername()})
                                          .AsQueryable();

                        return Result<PagedList<ActivityDTO>>.Success(
                              await PagedList<ActivityDTO>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                        );
                  }
            }
      }
}