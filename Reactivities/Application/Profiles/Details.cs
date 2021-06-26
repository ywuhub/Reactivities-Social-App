using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Profiles;
using Reactivities.Persistence;

namespace Application.Profiles
{
      public class Details
      {
            public class Query : IRequest<Result<Reactivities.Application.Profiles.Profile>>
            {
                  public string Username { get; set; }
            }

            public class Handler : IRequestHandler<Query, Result<Reactivities.Application.Profiles.Profile>>
            {
                  private readonly DataContext _context;
                  private readonly IMapper _mapper;
                  public Handler(DataContext context, IMapper mapper)
                  {
                        _mapper = mapper;
                        _context = context;

                  }

                  public async Task<Result<Reactivities.Application.Profiles.Profile>> Handle(Query request, CancellationToken cancellationToken)
                  {
                        var user = await _context.Users
                            .ProjectTo<Reactivities.Application.Profiles.Profile>(_mapper.ConfigurationProvider)
                            .SingleOrDefaultAsync(x => x.Username == request.Username);

                        if (user == null) return null;

                        return Result<Reactivities.Application.Profiles.Profile>.Success(user);
                  }
            }
      }
}