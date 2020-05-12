using Microsoft.AspNetCore.Mvc;
using WhatsTheWorkout.Services;
using WhatsTheWorkout.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

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

        [HttpGet]
        [Authorize]
        public ActionResult GetWorkouts([FromQuery] bool allWorkouts = true)
        {
            try
            {
                return Ok(_workoutService.GetWorkouts(allWorkouts, getUserId()).Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult PostWorkout([FromBody] PostWorkoutRequest workout)
        {
            try
            {
                return Ok(_workoutService.PostWorkout(workout, getUserId()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("{workoutId}")]
        public ActionResult UpdateWorkout(Guid workoutId, [FromBody] PostWorkoutRequest workout)
        {
            try
            {
                return Ok(_workoutService.UpdateWorkout(workoutId, workout, getUserId()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("{workoutId}")]
        public ActionResult DeleteWorkout(Guid workoutId)
        {
            try
            {
                return Ok(_workoutService.DeleteWorkout(workoutId, getUserId()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}