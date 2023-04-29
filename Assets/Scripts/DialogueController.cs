using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DialogueController : MonoBehaviour
{
    //built off of the yarn spinner Phone Chat Sample code
    DialogueRunner runner;
    public GameObject messageModulePrefab;

    public TMPro.TextMeshProUGUI messageText;
    public TMPro.TextMeshProUGUI characterNameText;
    public float lettersPerSecond = 10f;

    //character names
    [SerializeField] Color oracleColor, princessColor;
    Color currentSpeakerColor, currentSpeakerNameColor;
    [SerializeField] Color oracleNameColor, princessNameColor;
    string currentSpeakerName;

    //options
    public GameObject optionsContainer;
    public OptionView optionPrefab;

    bool isFirstMessage = true;

    void Awake()
    {
        runner = GetComponent<DialogueRunner>();
        optionsContainer.SetActive(false);

        //set commands

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //built off of the yarn spinner Phone Chat Sample code
    void UpdateMessageBoxSettings()
    {
        var message = messageModulePrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        message.text = "";
        characterNameText.text = "";
        message.color = Color.white;
    }

    public void CloneMessageBoxToHistory()
    {

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
        UpdateMessageBoxSettings();

    }
    //makes the options happen
    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {

        foreach (Transform child in optionsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        optionsContainer.SetActive(true);

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
}
