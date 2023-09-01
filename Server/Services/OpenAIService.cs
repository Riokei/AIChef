using AIChef.Server.ChatEndPoint;
using AIChef.Shared;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace AIChef.Server.Services
{
    public class OpenAIService : IOpenAiAPI
    {
        private readonly IConfiguration _configuration;
        private static readonly string _baseUrl = "https://api.openai.com/v1";
        private static readonly HttpClient _httpClient = new();
        private readonly JsonSerializerOptions _jsonOptions;

        //build the function object so that AI will return JSON formatted object.

        private static ChatFunction.Parameter _recipeIdeaParameter = new()
        {
            // describes one Idea
            Type = "object",
            Required = new string[] { "index", "title", "description" },
            Properties = new
            {
                // provide a type and description for each property of the Idea model
                Index = new ChatFunction.Property
                {
                    Type = "number",
                    Description = "A unique identifier for this object",
                },
                Title = new ChatFunction.Property
                {
                    Type = "string",
                    Description = "The name for a recipe to create"
                },
                Description = new ChatFunction.Property
                {
                    Type = "string",
                    Description = "A description of the recipe"
                }
            }
        };

        private static ChatFunction _ideaFunction = new()
        {
            // describe the function we want an argument for from the AI
            Name = "CreateRecipe",
            // this description ensures we get 5 ideas back
            Description = "Generates recipes for each idea in an array of five recipe ideas",
            Parameters = new
            {
                // OpenAI requires that the parameters are an object, so we need to wrap our array in an object
                Type = "object",
                Properties = new
                {
                    Data = new // our array will come back in an object in the Data property
                    {
                        Type = "array",
                        // further ensures the AI will create 5 recipe ideas
                        Description = "An array of five recipe ideas",
                        Items = _recipeIdeaParameter
                    }
                }
            }
        };

        //private static ChatFunction.Parameter _recipeParameter = new()
        //{
        //    Type = "object",
        //    Description = "The recipe to display",
        //    Required = new[] { "title", "ingredients", "instructions", "summary" },
        //    Properties = new
        //    {
        //        Title = new
        //        {
        //            Type = "string",
        //            Description = "The title of the recipe to display",
        //        },
        //        Ingredients = new
        //        {
        //            Type = "array",
        //            Description = "An array of all the ingredients mentioned in the recipe instructions",
        //            Items = new { Type = "string" }
        //        },
        //        Instructions = new
        //        {
        //            Type = "array",
        //            Description = "An array of each step for cooking this recipe",
        //            Items = new { Type = "string" }
        //        },
        //        Summary = new
        //        {
        //            Type = "string",
        //            Description = "A summary description of what this recipe creates",
        //        },
        //    },
        //};

        //private static ChatFunction _recipeFunction = new()
        //{
        //    Name = "DisplayRecipe",
        //    Description = "Displays the recipe from the parameter to the user",
        //    Parameters = new
        //    {
        //        Type = "object",
        //        Properties = new
        //        {
        //            Data = _recipeParameter
        //        },
        //    }
        //};

        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration;
            var apiKey = configuration["OpenAI:OpenAIKey"] ?? Environment.GetEnvironmentVariable("OpenAiKey");

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", apiKey);

            _jsonOptions = new()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        Task<List<Idea>> IOpenAiAPI.CreateRecipeIdeas(string mealtime, List<string> ingredients)
        {
            throw new NotImplementedException();
        }
    }
}
