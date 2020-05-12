using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
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

        public async Task<GetWorkoutResponse[]> GetWorkouts(bool allWorkouts, string userId)
        {
            Table WorkoutTable = Table.LoadTable(_dynamoClient, tableName);

            ScanFilter scanFilter = new ScanFilter();
            if(!allWorkouts)
            {
                scanFilter.AddCondition("UserId", ScanOperator.Equal, userId);
            }

            ScanOperationConfig config = new ScanOperationConfig()
            {
                AttributesToGet = new List<string> { "WorkoutId", "Name", "Description", "Steps" } ,
                Filter = scanFilter,
                Select = SelectValues.SpecificAttributes
            };
            
            Search search = WorkoutTable.Scan(config);
            List<Document> documentList = new List<Document>();
            List<GetWorkoutResponse> workoutResp = new List<GetWorkoutResponse>();
            do
            {
                documentList = await search.GetNextSetAsync();
                foreach (var document in documentList)
                {
                    workoutResp.Add(GenerateGetWorkoutResponseFromDocument(document));
                }
            } while (!search.IsDone);
            return workoutResp.ToArray();
        }

        private GetWorkoutResponse GenerateGetWorkoutResponseFromDocument(Document document)
        {
                DynamoDBEntry entry;
                GetWorkoutResponse workout = new GetWorkoutResponse();
                if (document.TryGetValue("WorkoutId", out entry))
                {
                    workout.WorkoutId = entry.AsGuid();
                }
                if (document.TryGetValue("Name", out entry))
                {
                    workout.Name = entry.AsString();
                }
                if (document.TryGetValue("Description", out entry))
                {
                    workout.Description = entry.AsString();
                }
                if (document.TryGetValue("Steps", out entry))
                {
                    List<string> stepsBeforeConversion = entry.AsListOfString();
                    List<WorkoutStep> stepsAfterConversion = new List<WorkoutStep>();
                    foreach (string step in stepsBeforeConversion)
                    {
                        stepsAfterConversion.Add(JsonConvert.DeserializeObject<WorkoutStep>(step));
                    }
                    workout.Steps = stepsAfterConversion.ToArray();
                }

                return workout;
        }

        public string GetWorkout(Guid workoutId)
        {
            return "Workout obtained";
        }

        public UpdateItemResponse UpdateWorkout(Guid workoutId, PostWorkoutRequest workout, string userId)
        {
            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
                { "WorkoutId", new AttributeValue { S = workoutId.ToString() } },
                { "UserId", new AttributeValue { S = userId } }
            };

            Dictionary<string, AttributeValueUpdate> updates = new Dictionary<string, AttributeValueUpdate>();
            if (workout.Steps != null)
            {
                List<AttributeValue> workoutStepsAttribute = new List<AttributeValue>();
                foreach(WorkoutStep s in workout.Steps)
                {
                    workoutStepsAttribute.Add(new AttributeValue { S = JsonConvert.SerializeObject(s) });
                }
                
                updates["Steps"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.PUT,
                    Value = new AttributeValue { L = workoutStepsAttribute }
                };
            }

            if (workout.Name != null)
            {
                updates["Name"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.PUT,
                    Value = new AttributeValue { S = workout.Name }
                };
            }

            if (workout.Description != null)
            {
                updates["Description"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.PUT,
                    Value = new AttributeValue { S = workout.Description }
                };
            }

            UpdateItemRequest updateRequest = new UpdateItemRequest
            {
                TableName = tableName,
                Key = key,
                AttributeUpdates = updates
            };

            return _dynamoClient.UpdateItemAsync(updateRequest).Result;
        }

        public DeleteItemResponse DeleteWorkout(Guid workoutId, string userId)
        {
            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
            {
                { "WorkoutId", new AttributeValue { S = workoutId.ToString() } },
                { "UserId", new AttributeValue { S = userId } }
            };

            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = key
            };
            
            return _dynamoClient.DeleteItemAsync(request).Result;
        }

    }
}