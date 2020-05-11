using System;
using WhatsTheWorkout.Models;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

namespace WhatsTheWorkout.Services
{
    public interface IWorkoutService
    {
        PutItemResponse PostWorkout(PostWorkoutRequest workout, string userId);
        Task<GetWorkoutResponse[]> GetWorkouts();
        string GetWorkout(Guid workoutId);
        string UpdateWorkout(Guid workoutId);
        string DeleteWorkout(Guid workoutId);
    }
}