using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueController_V : DialogueViewBase
{
    //built off of the yarn spinner Phone Chat Sample code
    DialogueRunner runner;
    VariableManager_V variableManager;
    public GameObject messageModulePrefab;

    //messages
    public TMPro.TextMeshProUGUI messageText;
    public TMPro.TextMeshProUGUI characterNameText;
    public float lettersPerSecond = 10f;
    public GameObject contentWindow;

    //character names
    //Serialize Field for Color of each speaker's name,
    [SerializeField] Color princessColor, oracleColor, narratorColor;
    Color currentSpeakerColor;
    string currentSpeakerName;

    //options
    public GameObject optionsContainer;
    public OptionView optionPrefab;

    //audio
    //not relevant rn

    //var management


    bool isFirstMessage = true;

    void Awake()
    {
        runner = GetComponent<DialogueRunner>();
        //create all commands in awake.
        //- Set the speaker's name for each message - DONE
        //- make text flashy
        //- Prompt the card draws, which pauses the dialogue and sets the cards that have been chosen\
        runner.AddCommandHandler("Princess", SetSenderPrincess);
        runner.AddCommandHandler("Oracle", SetSenderOracle);
        runner.AddCommandHandler("Narrator", SetSenderNarrator);
        runner.AddCommandHandler("TriggerCardView", TriggerCardView);

        optionsContainer.SetActive(false);
    }
        // Start is called before the first frame update
        void Start()
    {
        messageModulePrefab.SetActive(false);
        UpdateMessageBoxSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ACTUAL MESSAGE FUNCTIONALITY
    void UpdateMessageBoxSettings()
    {
        //i actually still don't understand what this does
        //according to the tutorial:
        //when we clone a new message box, re-style the message box based on whether SetSenderMe or
        //SetSenderThem was most recently called
        //oh so this just like. sets the situation of the text
        var message = messageModulePrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        message.text = "";
        characterNameText.text = "";
        message.color = Color.white;
        Debug.Log("I am sending messages to history");
    }

    public void CloneMessageBoxToHistory()
    {
        //this saves the content of the previous message and makes it possible for you to scroll up to review it
        // if this isn't the very first message, then clone current message box and move it up
        if (isFirstMessage == false)
        {
            var oldClone = Instantiate(
                messageModulePrefab,
                messageModulePrefab.transform.position,
                messageModulePrefab.transform.rotation,
                messageModulePrefab.transform.parent
            );
            messageModulePrefab.transform.SetAsLastSibling();
        }
        isFirstMessage = false;

        // reset message box and configure based on current settings
        messageModulePrefab.SetActive(true);
        //adds the new message
        UpdateMessageBoxSettings();
    }

    //typewriter effect *shrug*
    Coroutine currentTypewriterEffect;

    //this is for yarn to function!
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        //if you get a CS0115 error where there is no suitable method found to override, it's caused
        //by the fact that you didn't make this a DialogueViewBase subclass. this script is not supposed
        //to be a MonoBehavior!
        Debug.Log("running line");
        if (currentTypewriterEffect != null)
        {
            StopCoroutine(currentTypewriterEffect);
        }

        CloneMessageBoxToHistory();

        messageText.text = dialogueLine.TextWithoutCharacterName.Text;
        //messageText.color = currentSpeakerColor; if u want to change the color of the actual text as well
        Debug.Log("setting speaker name to " + currentSpeakerName);
        characterNameText.text = currentSpeakerName;
        characterNameText.color = currentSpeakerColor;
        currentTypewriterEffect = StartCoroutine(ShowTextAndNotify());

        IEnumerator ShowTextAndNotify()
        {
            yield return StartCoroutine(Effects.Typewriter(messageText, lettersPerSecond, null));
            currentTypewriterEffect = null;
            yield return new WaitForSeconds(.5f);
            onDialogueLineFinished();
        }
    }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {

        foreach (Transform child in optionsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        optionsContainer.SetActive(true);
        Debug.Log("running Options");

        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            DialogueOption option = dialogueOptions[i];
            var optionView = Instantiate(optionPrefab);

            optionView.transform.SetParent(optionsContainer.transform, false);

            optionView.Option = option;

            optionView.OnOptionSelected = (selectedOption) =>
            {
                optionsContainer.SetActive(false);
                onOptionSelected(selectedOption.DialogueOptionID);
            };
        }
    }

    //SET SPEAKERS HERE
    public void SetSenderPrincess()
    {
        currentSpeakerColor = princessColor;
        currentSpeakerName = "Princess - ";
    }

    public void SetSenderOracle()
    {
        currentSpeakerColor = oracleColor;
        currentSpeakerName = "Oracle - ";
    }
    public void SetSenderNarrator()
    {
        currentSpeakerColor = narratorColor;
        currentSpeakerName = "YOU - ";
    }

    //SET PROMPTS HERE
    public void TriggerCardView()
    {
        //trigger the drag and drop screen here!
        Debug.Log("Triggering Card View");
        //tell the tarottester thing to set active
    }

    //Variable Management
}

