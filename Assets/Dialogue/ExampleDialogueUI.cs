/*

The MIT License (MIT)

Copyright (c) 2015 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

// Displays dialogue lines to the player, and sends
// user choices back to the dialogue system.

// Note that this is just one way of presenting the
// dialogue to the user. The only hard requirement
// is that you provide the RunLine, RunOptions, RunCommand
// and DialogueComplete coroutines; what they do is up to you.

namespace Yarn.Unity.Example {
	public class ExampleDialogueUI : Yarn.Unity.DialogueUIBehaviour
	{
        public static ExampleDialogueUI staticDialogueUI;

		// The object that contains the dialogue and the options.
		// This object will be enabled when conversation starts, and
		// disabled when it ends.
		public GameObject dialogueContainer;
		
		// The UI element that displays lines
		public Text lineText;
		
		// A UI element that appears after lines have finished appearing
		public GameObject continuePrompt;
		
		// A delegate (ie a function-stored-in-a-variable) that
		// we call to tell the dialogue system about what option
		// the user selected
		private Yarn.OptionChooser SetSelectedOption;
		
		[Tooltip("How quickly to show the text, in seconds per character")]
		public float textSpeed = 0.025f;
		
		// The buttons that let the user choose an option
		public List<Button> optionButtons;

		public RectTransform gameControlsContainer;

        //Camera work
       // public cameraFollow3D mainCam;
      //  public DialogueCamera dialogueCam;

        //player handling
        public PlayerTalking playerDialogue;

        //dialogue display variables
        public Text invisibleText;
        public int charsPerLine;
        public string playerName = "Norra";

        //bubble positioning
        public Transform dialogueBubbleCanvas;
        public Vector3 characterOffset;
        public string lastCharName;

        //option variables
        public Image stemImage;
        public GameObject[] optionIndicators, arrows;
        public float deadZone = .1f;
        public Color notSelectedColor, selectedColor;
        public HorizontalLayoutGroup optionPanelLayout;
        public Vector2[] layoutPaddings;

        //text animation
        public Transform letterPrefab;
        [Range(100f, 300f)]
        public float animationSpeed;

		void Awake ()
		{
            staticDialogueUI = this;
			// Start by hiding the container, line and option buttons
			if (dialogueContainer != null)
			//	dialogueContainer.SetActive(false);
			
		//	lineText.gameObject.SetActive (false);
			
			foreach (var button in optionButtons) {
			//	button.gameObject.SetActive (false);
			}
			
			// Hide the continue prompt if it exists
			if (continuePrompt != null)
				continuePrompt.SetActive (false);
		}
		
		// Show a line of dialogue, gradually
		public override IEnumerator RunLine (Yarn.Line line)
		{
			// Show the text
			lineText.gameObject.SetActive (true);

            char endName = ':';
            string charName = "";
            string remainingDialogue = "";
            bool foundColon = false;
            bool clearedExtraSpace = false;
            foreach (char n in line.text)
            {
                //print("n is " + n);
                if (n == endName)
                {
                    foundColon = true;
                    //print("found colon and n is " + n);
                }
                if (!foundColon && n != endName)
                {
                    charName += n;
                    //print("printing name and n is " + n + " and charname is " + charName);
                }
                if (foundColon && n != endName && !clearedExtraSpace)
                {
                    clearedExtraSpace = true;
                }
                else if (foundColon && n != endName)
                {
                    remainingDialogue += n;
                    //print("printing dialogue and n is " + n + " and remainingDialogue is " + charName);
                }
            }
            print("char name is " + charName);
            PositionBubble(charName);
            //print("final remaining is " + remainingDialogue);
            line.text = remainingDialogue;

            var stringBuilder = new StringBuilder();
            line.text = AddLineBreaks(line.text);
            foreach (char c in line.text)
            {
                stringBuilder.Append(c);
                invisibleText.text = stringBuilder.ToString();
            }

			if (textSpeed > 0.0f) {

                stringBuilder = new StringBuilder();
              
                foreach (char c in invisibleText.text)
                {
                    stringBuilder.Append(c);
                    lineText.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(textSpeed);
                }
               
                // Display the line one character at a time
                /*var stringBuilder = new StringBuilder ();
                var charCount = 0;
                //string newLineString = " ";
                char newLineSpace = ' ';// newLineString.ToCharArray();
				foreach (char c in line.text) {
                    if (charCount >= charsPerLine && c == newLineSpace)
                    {
                        charCount = 0;
                        stringBuilder.Append("\n");
                    }else
                        stringBuilder.Append (c);
                    charCount++;
                    lineText.text = stringBuilder.ToString ();
					
                    */
			} else {
				// Display the line immediately if textSpeed == 0
				lineText.text = line.text;
			}
			
			// Show the 'press any key' prompt when done, if we have one
			if (continuePrompt != null)
				continuePrompt.SetActive (true);
			
			
			// Wait for any user input
			while (Input.anyKeyDown == false) {
				yield return null;
			}
			
			// Hide the text and prompt
			//lineText.gameObject.SetActive (false);
			
			if (continuePrompt != null)
				continuePrompt.SetActive (false);
			
		}
		
        string AddLineBreaks(string entryString)
        {
            string lineBreakString = "";
            var stringBuilder = new StringBuilder();
            var charCount = 0;
            char newLineSpace = ' ';
            foreach (char c in entryString)
            {
                if (charCount >= charsPerLine && c == newLineSpace)
                {
                    charCount = 0;
                    stringBuilder.Append("\n");
                }
                else
                    stringBuilder.Append(c);
                charCount++;
                lineBreakString = stringBuilder.ToString();
            }
            return lineBreakString;
        }

        public void PositionBubble(string charName)
        {
            Transform target = null;
            if (charName == playerName)
            {
                target = GameObject.Find("player").transform;
            }else
            {
                target = GameObject.Find("npc_" + charName).transform;
                lastCharName = charName;
            }
            if (target == null)
            {
                Debug.LogError("no npc with the name npc_" + charName + " found");
            }
            dialogueBubbleCanvas.position = target.position + characterOffset;
        }

        public void SetEllipses()
        {

            invisibleText.text = "...";
            lineText.text = "...";
        }
		// Show a list of options, and wait for the player to make a selection.
		public override IEnumerator RunOptions (Yarn.Options optionsCollection, 
		                                        Yarn.OptionChooser optionChooser)
		{
			// Do a little bit of safety checking
			if (optionsCollection.options.Count > optionIndicators.Length) {
				Debug.LogWarning("There are more options to present than there are" +
				                 "indicators to display. This will cause problems.");
			}

            PositionBubble(playerName);                                                   //show bubble over player because we're deciding dialogue

            #region ShowOptionUI
            optionIndicators[0].transform.parent.GetComponent<LayoutElement>().ignoreLayout = false;//make indicator parent respond to layout
            for (int i = 0; i < optionsCollection.options.Count; i++) {                             //for every dialogue option     
                optionIndicators[i].SetActive(true);                                                     //enable option indicators
                optionsCollection.options[i] = AddLineBreaks(optionsCollection.options[i]);              //add line breaks to the option text
            }
            foreach (GameObject arrow in arrows)                                                    //enable arrows
                arrow.SetActive(true);
            stemImage.enabled = false;                                                              //disable stem image because we're thinking
            optionPanelLayout.padding.left = optionPanelLayout.padding.right = Mathf.RoundToInt(layoutPaddings[1].x);
            optionPanelLayout.padding.top = optionPanelLayout.padding.bottom = Mathf.RoundToInt(layoutPaddings[1].y);
            #endregion

            int currentlyDisplayedOption = 0;                                             //keep track of what option we're on
            DisplayOption(optionsCollection.options[currentlyDisplayedOption],currentlyDisplayedOption);           //display first option by default
            
            SetSelectedOption = optionChooser;              //Record that we're using it
            bool axisReady = true;                          //so the joystick has to go back to zero before you can change again

            yield return new WaitForSeconds(.1f);
            while (SetSelectedOption != null)               // Wait until the chooser has been used and then removed (see SetOption below)
            {
                #region GetChangeInput
                float h = Input.GetAxis("Horizontal");
                if (h > deadZone && axisReady) {
                    if (currentlyDisplayedOption < optionsCollection.options.Count-1)
                        currentlyDisplayedOption++;
                    else
                        currentlyDisplayedOption = 0;
                    DisplayOption(optionsCollection.options[currentlyDisplayedOption], currentlyDisplayedOption);
                    axisReady = false;
                }
                else if (h < -deadZone && axisReady)
                {
                    if (currentlyDisplayedOption < 1)
                        currentlyDisplayedOption = optionsCollection.options.Count-1;
                    else
                        currentlyDisplayedOption--;
                    DisplayOption(optionsCollection.options[currentlyDisplayedOption], currentlyDisplayedOption);
                    axisReady = false;
                }
                else if (h < deadZone && h > -deadZone && !axisReady)
                {
                    axisReady = true;
                }
                #endregion
                #region GetSelectInput
                if (Input.GetButtonDown("Interact"))
                {
                    SetOption(currentlyDisplayedOption);
                    yield return null;
                }
                #endregion
                yield return null;
            }
            #region HideOptionUI
            optionIndicators[0].transform.parent.GetComponent<LayoutElement>().ignoreLayout = true; //make indicator parent not respond to layout
            for (int i = 0; i < optionsCollection.options.Count; i++)                                    //for every dialogue option 
                optionIndicators[i].SetActive(false);                                                    //disable option indicators
            foreach (GameObject arrow in arrows)                                                    //enable arrows
                arrow.SetActive(false);
            stemImage.enabled = true;                                                              //disable stem image because we're thinking
            optionPanelLayout.padding.left = optionPanelLayout.padding.right = Mathf.RoundToInt(layoutPaddings[0].x);
            optionPanelLayout.padding.top = optionPanelLayout.padding.bottom = Mathf.RoundToInt(layoutPaddings[0].y);
            #endregion
        }

        void DisplayOption(string optionToDisplay, int optionIndex)
        {
            print("displaying option " + optionIndex);
            for (int i = 0; i<optionIndicators.Length; i++)
            {
                if (i == optionIndex && optionIndicators[i].activeSelf)
                    optionIndicators[i].GetComponent<Image>().color = selectedColor;
                else if (optionIndicators[i].activeSelf)
                    optionIndicators[i].GetComponent<Image>().color = notSelectedColor;
            }
            invisibleText.text = optionToDisplay;
            lineText.text = invisibleText.text;
        }

		// Called by buttons to make a selection.
		public void SetOption (int selectedOption)
		{
			
			// Call the delegate to tell the dialogue system that we've
			// selected an option.
			SetSelectedOption (selectedOption);
			
			// Now remove the delegate so that the loop in RunOptions will exit
			SetSelectedOption = null; 
		}
		
		// Run an internal command.
		public override IEnumerator RunCommand (Yarn.Command command)
		{
			// "Perform" the command
			Debug.Log ("Command: " + command.text);

			yield break;
		}
		
		public override IEnumerator DialogueStarted ()
		{
          
        //    dialogueCam.enabled = true;
        //    mainCam.enabled = false;
			Debug.Log ("Dialogue starting!");
			
			// Enable the dialogue controls.
			if (dialogueContainer != null)
			//	dialogueContainer.SetActive(true);

			// Hide the game controls.
			if (gameControlsContainer != null) {
				gameControlsContainer.gameObject.SetActive(false);
			}
			
			yield break;
		}
		
		// Yay we're done. Called when the dialogue system has finished running.
		public override IEnumerator DialogueComplete ()
		{
          //  dialogueCam.enabled = false;
        //    mainCam.enabled = true;
            SetEllipses();
            PositionBubble(lastCharName);
			Debug.Log ("Complete!");

            // Hide the dialogue interface.
           // if (dialogueContainer != null)
                //	dialogueContainer.SetActive(false);

           playerDialogue.ReEnableBehaviors();

			// Show the game controls.
			if (gameControlsContainer != null) {
				gameControlsContainer.gameObject.SetActive(true);
			}
			
			yield break;
		}
		
	}

}
