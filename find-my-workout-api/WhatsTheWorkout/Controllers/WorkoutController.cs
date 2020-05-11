using Microsoft.AspNetCore.Mvc;
using WhatsTheWorkout.Services;
using WhatsTheWorkout.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace WhatsTheWorkout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;
        private static readonly string USER_ID_CLAIM_TYPE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        private string getUserId()
        {
            return HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == USER_ID_CLAIM_TYPE).Value;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<string> GetWorkouts()
        {
            return Ok(_workoutService.GetWorkouts().Result);
        }

        // POST api/values
        [HttpPost]
        [Authorize]
        public ActionResult PostWorkout([FromBody] PostWorkoutRequest workout)
        {
            return Ok(_workoutService.PostWorkout(workout, getUserId()));
        }
    }
}