using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;

            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.id);

                // implement Auto Mapper instead of assigning changes 
                activity.Title = request.Activity.Title ?? activity.Title;

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}