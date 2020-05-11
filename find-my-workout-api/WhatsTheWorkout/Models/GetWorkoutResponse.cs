using System;

namespace WhatsTheWorkout.Models
{
   public class GetWorkoutResponse
   {
       public Guid WorkoutId { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }
       public WorkoutStep[] Steps { get; set; }
   }
}