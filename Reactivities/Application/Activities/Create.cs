using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
      public class Create
      {
            public class Command : IRequest<Result<Unit>>
            {
                  public Activity Activity { get; set; }
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                  public CommandValidator()
                  {
                        RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
                  }
            }

            public class Handler : IRequestHandler<Command, Result<Unit>>
            {
                  private readonly DataContext _context;
                  private readonly IUsernameAccessor _usernameAccessor;

                  public Handler(DataContext context, IUsernameAccessor usernameAccessor)
                  {
                        _usernameAccessor = usernameAccessor;
                        _context = context;

                  }
                  public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
                  {
                        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _usernameAccessor.GetUsername());

                        var attendee = new ActivityAttendee
                        {
                            AppUser = user,
                            Activity = request.Activity,
                            IsHost = true
                        };

                        request.Activity.Attendees.Add(attendee);

                        _context.Activities.Add(request.Activity);

                        var result = await _context.SaveChangesAsync() > 0;

                        if (!result) return Result<Unit>.Failure("Failed to create activity");

                        return Result<Unit>.Success(Unit.Value);
                  }
            }
      }
}