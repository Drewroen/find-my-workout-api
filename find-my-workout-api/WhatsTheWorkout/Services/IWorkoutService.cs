using System;
using WhatsTheWorkout.Models;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

namespace WhatsTheWorkout.Services
{
    public interface IWorkoutService
    {
        PutItemResponse PostWorkout(PostWorkoutRequest workout, string userId);
        Task<GetWorkoutResponse[]> GetWorkouts(bool allWorkouts, string userId);
        string GetWorkout(Guid workoutId);
        UpdateItemResponse UpdateWorkout(Guid workoutId, PostWorkoutRequest workout, string userId);
        DeleteItemResponse DeleteWorkout(Guid workoutId, string userId);
    }
}