using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChatGPTTest
{
    class Program
    {
        private static readonly string apiKey = ""; // Replace with your actual API key
        private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

        static async Task Main(string[] args)
        {
            Console.Write("Enter your prompt: ");
            string prompt = Console.ReadLine();

            string response = await GetChatGPTResponse(prompt);
            Console.WriteLine("\nChatGPT Response:");
            Console.WriteLine(response);
        }

        static async Task<string> GetChatGPTResponse(string prompt)
        {
            // Create the request payload
            var requestData = new
            {
                model = "gpt-4", // Or "gpt-3.5-turbo" as needed
                messages = new object[]
                {
                    new { role = "system", content = "You are a helpful assistant." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 100
            };

            // Serialize the payload to JSON
            string jsonData = JsonConvert.SerializeObject(requestData);

            using (var client = new HttpClient())
            {
                // Set the request headers
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                // Deserialize the response into our custom classes
                ChatGPTResponse responseObj = JsonConvert.DeserializeObject<ChatGPTResponse>(responseString);

                // Return the AI's message content if available
                if (responseObj != null && responseObj.choices != null && responseObj.choices.Length > 0)
                {
                    return responseObj.choices[0].message.content;
                }
                else
                {
                    return "No valid response received from ChatGPT.";
                }
            }
        }
    }

    // Classes to parse the JSON response
    public class ChatGPTResponse
    {
        public Choice[] choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}