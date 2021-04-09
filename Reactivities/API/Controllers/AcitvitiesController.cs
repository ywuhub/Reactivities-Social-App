using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Activities;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ActivitiesController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _mediator.Send(new List.Query());
        }

        /*
        [HttpGet("{id}")]
        public Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            return Ok();
        }
        */
    }
}