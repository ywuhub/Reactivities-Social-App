using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Profiles;
using Reactivities.Persistence;

namespace Application.Followers
{
      public class List
      {
            public class Query : IRequest<Result<List<Reactivities.Application.Profiles.Profile>>>
            {
                  public string Predicate { get; set; }
                  public string Username { get; set; }
            }

            public class Handler : IRequestHandler<Query, Result<List<Reactivities.Application.Profiles.Profile>>>
            {
                  private readonly DataContext _context;
                  private readonly IMapper _mapper;
                  private readonly IUsernameAccessor _userAccessor;
                  public Handler(DataContext context, IMapper mapper, IUsernameAccessor userAccessor)
                  {
                        _userAccessor = userAccessor;
                        _mapper = mapper;
                        _context = context;
                  }

                  public async Task<Result<List<Reactivities.Application.Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
                  {
                        var profiles = new List<Reactivities.Application.Profiles.Profile>();

                        switch (request.Predicate)
                        {
                              case "followers":
                                    profiles = await _context.UserFollowings.Where(x => x.Target.UserName == request.Username)
                                        .Select(u => u.Observer)
                                        .ProjectTo<Reactivities.Application.Profiles.Profile>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
                                        .ToListAsync();
                                    break;
                              case "following":
                                    profiles = await _context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                                        .Select(u => u.Target)
                                        .ProjectTo<Reactivities.Application.Profiles.Profile>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
                                        .ToListAsync();
                                    break;
                        }

                        return Result<List<Reactivities.Application.Profiles.Profile>>.Success(profiles);
                  }
            }
      }
}