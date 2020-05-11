using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon;
using WhatsTheWorkout.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

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

        public PutItemResponse PostWorkout(PostWorkoutRequest workoutRequest, string userId)
        {
            PutItemRequest request = GeneratePutItemRequest(workoutRequest, userId);

            return _dynamoClient.PutItemAsync(request).Result;
        }

        private PutItemRequest GeneratePutItemRequest(PostWorkoutRequest workoutRequest, string userId)
        {
            List<AttributeValue> workoutStepsAttribute = new List<AttributeValue>();
            foreach(WorkoutStep s in workoutRequest.Steps)
            {
                workoutStepsAttribute.Add(new AttributeValue { S = JsonConvert.SerializeObject(s) });
            }
            Dictionary<string, AttributeValue> dynamoWorkout = new Dictionary<string, AttributeValue>();
            dynamoWorkout.Add("WorkoutId", new AttributeValue { S = Guid.NewGuid().ToString() });
            dynamoWorkout.Add("UserId", new AttributeValue { S = userId });
            dynamoWorkout.Add("Name", new AttributeValue { S = workoutRequest.Name });
            dynamoWorkout.Add("Description", new AttributeValue { S = workoutRequest.Description });
            dynamoWorkout.Add("Steps", new AttributeValue { L = workoutStepsAttribute });

            return new PutItemRequest
            {
                TableName = tableName,
                Item = dynamoWorkout,
            };
        }

        public async Task<GetWorkoutResponse[]> GetWorkouts()
        {
            List<GetWorkoutResponse> workouts = new List<GetWorkoutResponse>();
            var resp = await _dynamoClient.ScanAsync(tableName, new List<string>{ "WorkoutId", "Name", "Description", "Steps" });
            foreach(Dictionary<string, AttributeValue> dynamoWorkout in resp.Items)
            {
                GetWorkoutResponse workout = new GetWorkoutResponse();
                workout.WorkoutId = Guid.Parse(dynamoWorkout.GetValueOrDefault("WorkoutId").S);
                workout.Name = dynamoWorkout.GetValueOrDefault("Name").S;
                workout.Description = dynamoWorkout.GetValueOrDefault("Description").S;
                List<WorkoutStep> steps = new List<WorkoutStep>();
                foreach(AttributeValue step in dynamoWorkout.GetValueOrDefault("Steps").L)
                {
                    steps.Add(JsonConvert.DeserializeObject<WorkoutStep>(step.S));
                }
                workout.Steps = steps.ToArray();
                workouts.Add(workout);
            }
            
            return workouts.ToArray();
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