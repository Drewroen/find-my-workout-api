using Microsoft.AspNetCore.Mvc;
using WhatsTheWorkout.Services;

namespace WhatsTheWorkout.Models
{
   public class WorkoutStep
   {
       public string StepType { get; set; }
       public string Note { get; set; }
       public string DurationType { get; set; }
       public double DurationValue { get; set; }
       public double Effort { get; set; }
   }
}