using Microsoft.AspNetCore.Mvc;
using WhatsTheWorkout.Services;

namespace WhatsTheWorkout.Models
{
   public class PostWorkoutRequest
   {
       public string Name { get; set; }
       public string Description { get; set; }
       public WorkoutStep[] Steps { get; set; }
   }
}