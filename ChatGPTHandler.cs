using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;                // Import TextMeshPro namespace
using Newtonsoft.Json;      // For JSON serialization/deserialization
using System.Text;          // For encoding JSON payload

// This MonoBehaviour handles sending the user's prompt to the ChatGPT API
// and displaying the response in the UI.
public class ChatGPTHandler : MonoBehaviour
{
    // Reference to the TextMeshPro input field where the user types their prompt.
    [SerializeField] private TMP_InputField inputField;

    // Reference to the TextMeshPro text component where the ChatGPT response will be displayed.
    [SerializeField] private TMP_Text outputText;

    // Your OpenAI API key; replace with your actual key.
    private string apiKey = "your-api-key-here";

    // URL endpoint for the ChatGPT API.
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    // Start is called before the first frame update.
    // Here we subscribe to the onEndEdit event of the input field.
    void Start()
    {
        // When editing of the input field ends (user presses Enter or clicks away),
        // the OnInputEndEdit method is triggered.
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    // This method is called when the input field's editing ends.
    // The parameter is ignored since we get the current text directly from the input field.
    public void OnInputEndEdit(string ignored)
    {
        // Retrieve the current text from the input field.
        string prompt = inputField.text;

        // Log the entered prompt for debugging.
        Debug.Log("Input field text: " + prompt);

        // Check if the prompt is not empty.
        if (!string.IsNullOrEmpty(prompt))
        {
            // Optionally, display a waiting message in the output text.
            outputText.text = "Waiting for response...";

            // Start the coroutine that sends the prompt to ChatGPT.
            StartCoroutine(GetChatGPTResponse(prompt));
        }
        else
        {
            // Log a warning if the prompt is empty.
            Debug.Log("Input is empty.");
        }
    }

    // Coroutine that sends the prompt to the ChatGPT API and processes the response.
    private IEnumerator GetChatGPTResponse(string prompt)
    {
        // Log that we are sending a request.
        Debug.Log("Sending request with prompt: " + prompt);

        // Create the payload for the API call.
        // It includes the model, a system message, the user's prompt, and a token limit.
        var requestData = new
        {
            model = "gpt-4", // Use "gpt-3.5-turbo" if desired.
            messages = new object[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = prompt }
            },
            max_tokens = 100 // Limit the response length.
        };

        // Convert the payload into a JSON string.
        string jsonData = JsonConvert.SerializeObject(requestData);

        // Convert the JSON string into a byte array, which is required for the request.
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        // Create a UnityWebRequest configured for a POST request.
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            // Set the payload for the request.
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            // Prepare to receive the response as plain text.
            request.downloadHandler = new DownloadHandlerBuffer();
            // Set the content type to JSON.
            request.SetRequestHeader("Content-Type", "application/json");
            // Set the authorization header with your API key.
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // Send the request and wait for the response.
            yield return request.SendWebRequest();

            // Check if the request was successful.
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the raw JSON response from the API.
                string jsonResponse = request.downloadHandler.text;
                Debug.Log("Received JSON: " + jsonResponse);

                // Deserialize the JSON response into our custom data classes.
                ChatGPTResponse responseObj = JsonConvert.DeserializeObject<ChatGPTResponse>(jsonResponse);

                // Check if we received at least one valid choice.
                if (responseObj != null && responseObj.choices != null && responseObj.choices.Length > 0)
                {
                    // Extract the assistant's response from the first choice.
                    string chatResponse = responseObj.choices[0].message.content;
                    Debug.Log("ChatGPT Response: " + chatResponse);

                    // Display the response in the output text UI element.
                    outputText.text = chatResponse;
                }
                else
                {
                    // Log an error and update the UI if no valid response was received.
                    Debug.Log("Error: No valid response received.");
                    outputText.text = "Error: No valid response received.";
                }
            }
            else
            {
                // Log the error and update the UI if the web request fails.
                Debug.Log("Error: " + request.error);
                outputText.text = "Error: " + request.error;
            }
        }
    }
}

// Classes used for deserializing the JSON response from the ChatGPT API.
// The structure here should match the API's returned JSON format.
[System.Serializable]
public class ChatGPTResponse
{
    // Array of choices returned by the API.
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    // Each choice includes a message.
    public Message message;
}

[System.Serializable]
public class Message
{
    // The role of the message sender (e.g., "system", "user", "assistant").
    public string role;
    // The content of the message.
    public string content;
}