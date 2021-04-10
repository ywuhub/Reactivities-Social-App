using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Activities;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await Mediator.Send(new List.Query());
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