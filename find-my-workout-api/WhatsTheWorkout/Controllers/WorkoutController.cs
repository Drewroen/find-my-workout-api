using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhatsTheWorkout.Services;

namespace WhatsTheWorkout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> GetWorkouts()
        {
            return _workoutService.GetWorkouts();
        }
    }
}