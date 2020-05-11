using Microsoft.AspNetCore.Mvc;
using WhatsTheWorkout.Services;
using WhatsTheWorkout.Models;

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
            return Ok(_workoutService.GetWorkouts());
        }

        // POST api/values
        [HttpPost]
        public ActionResult PostWorkout([FromBody] PostWorkoutRequest workout)
        {
            _workoutService.PostWorkout(workout);
            return Ok();
        }
    }
}