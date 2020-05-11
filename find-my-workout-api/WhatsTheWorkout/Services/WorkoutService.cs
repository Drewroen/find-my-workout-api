using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon;
using WhatsTheWorkout.Models;

namespace WhatsTheWorkout.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private readonly string tableName = "Workout";
        public WorkoutService()
        {
            _dynamoClient = new AmazonDynamoDBClient(FallbackCredentialsFactory.GetCredentials(), RegionEndpoint.USEast2);
        }

        public void PostWorkout(PostWorkoutRequest workout)
        {
            Dictionary<string, AttributeValue> temp = new Dictionary<string, AttributeValue>();
            temp.Add("UserId", new AttributeValue { S = Guid.NewGuid().ToString() });
            temp.Add("Name", new AttributeValue { S = "TEST" });

            var req = new PutItemRequest
            {
                TableName = tableName,
                Item = temp,
            };

            _dynamoClient.PutItemAsync(req);
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