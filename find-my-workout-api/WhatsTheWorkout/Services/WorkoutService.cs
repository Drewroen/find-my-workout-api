using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WhatsTheWorkout.Services
{
    public class WorkoutService : IWorkoutService
    {
        public void PostWorkout()
        {

        }

        public string GetWorkouts()
        {
            return "Workouts obtained";
        }

        public string GetWorkout(Guid workoutId)
        {
            return "Workout obtained";
        }

        public string UpdateWorkout(Guid workoutId)
        {
            return "Workout updated";
        }

        public string DeleteWorkout(Guid workoutId)
        {
            return "Workout deleted";
        }

    }
}