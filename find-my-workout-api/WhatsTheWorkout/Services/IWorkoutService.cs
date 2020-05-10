using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WhatsTheWorkout.Services
{
    public interface IWorkoutService
    {
        void PostWorkout();
        string GetWorkouts();
        string GetWorkout(Guid workoutId);
        string UpdateWorkout(Guid workoutId);
        string DeleteWorkout(Guid workoutId);
    }
}