using System;
using WhatsTheWorkout.Models;

namespace WhatsTheWorkout.Services
{
    public interface IWorkoutService
    {
        void PostWorkout(PostWorkoutRequest workout);
        string GetWorkouts();
        string GetWorkout(Guid workoutId);
        string UpdateWorkout(Guid workoutId);
        string DeleteWorkout(Guid workoutId);
    }
}