Script is in assets -> _Scripts -> ChatGPTResponse.cs

steps to integrate in unity --

STEP 1: setting up UI elements

1. Create a Canvas:

Right-click in the Hierarchy → UI → Canvas.
If one doesn’t already exist, Unity will create a Canvas and an EventSystem.

2. Add a TextMeshPro Input Field:

Right-click on the Canvas → UI → TextMeshPro - Input Field.
Rename it (for example, "UserPrompt").
Adjust its position and size as needed.

3. Add a TextMeshPro Text Object:

Right-click on the Canvas → UI → TextMeshPro - Text.
Rename it (for example, "ResponseText").
This will display the API’s response. Adjust its layout to ensure it’s clearly visible.

4. Add a Button:

Right-click on the Canvas → UI → Button.
Rename it (for example, "AskGPTButton").
You can customize the button’s text if desired.


Step 2: Import and Attach the Script

1. Place Your Script:

Ensure your ChatGPTRequest.cs file is in your Assets/Scripts/ folder (or any folder under Assets IN UNITY).
Verify that there are no compile errors in the Console.

2. Create a Manager GameObject:

In the Hierarchy, right-click → Create Empty.
Name it “ChatGPTManager” (or any name you prefer).

3. Attach the Script:

Select “ChatGPTManager”.
Drag and drop ChatGPTRequest.cs onto the GameObject’s Inspector, or click Add Component and search for ChatGPTRequest.
The component should appear with public fields for userPrompts, askGPT, and responseText.


Step 3: Assign UI Elements to the Script

1. Assign the Input Field(s):

In the Inspector for “ChatGPTManager,” you’ll see an array field labeled User Prompts.
Click the plus sign (+) to add an element if needed (or assign directly if there’s one already).
Drag your "UserPrompt" (TMP_InputField) from the Hierarchy into the array element.

2. Assign the Button:

Drag the "AskGPTButton" from the Hierarchy into the Ask GPT slot in the Inspector.

3. Assign the Response Text:

Drag the "ResponseText" (TMP_Text) from the Hierarchy into the Response Text slot in the Inspector.


Step 4: Set Your API Key
In the Inspector for “ChatGPTManager,” locate the apiKey field in your script (if it’s public) or hard-coded in your script.
Replace "YOUR API KEY" with your actual OpenAI API key.


Step 5: Test Your Setup
Save Your Scene.

Enter Play Mode:

In Play Mode, type a prompt into your input field.
Click the Ask GPT button.

Verify the Output:

The script will send the prompt to the API.
Once the response returns, the responseText field should update with the ChatGPT response.
Check the Console for any debug logs or errors.
