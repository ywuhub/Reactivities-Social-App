using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Persistence;

namespace Application.Followers
{
      public class FollowToggle
      {
            public class Command : IRequest<Result<Unit>>
            {
                  public string TargetUsername { get; set; }
            }

            public class Handler : IRequestHandler<Command, Result<Unit>>
            {
                  private readonly DataContext _context;
                  private readonly IUsernameAccessor _userAccessor;
                  public Handler(DataContext context, IUsernameAccessor userAccessor)
                  {
                        _userAccessor = userAccessor;
                        _context = context;
                  }

                  public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
                  {
                        var observer = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                        var target = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

                        if (target == null) return null;

                        var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

                        if (following == null) {
                            following = new Domain.UserFollowing {
                                Observer = observer,
                                Target = target
                            };
                        } else {
                            _context.UserFollowings.Remove(following);
                        }

                        var success = await _context.SaveChangesAsync() > 0;

                        if (success) return Result<Unit>.Success(Unit.Value);

                        return Result<Unit>.Failure("Failure to update the following list");
                  }
            }
      }
}