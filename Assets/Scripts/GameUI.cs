using System;
using System.Collections;
using System.Collections.Generic;
using AICommand;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI responeText;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private Button submitButton;
    [SerializeField] private TextToSpeech textToSpeech;

    private void Awake() {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    private void OnSubmitButtonClicked() {
        var _prompt = inputField.text;
        var code = OpenAIUtil.InvokeChat(_prompt);
        Debug.Log("AI command script:" + code);
        SetResponseText(code);
        
        textToSpeech.Speak(code);
    }

    public void SetResponseText(string text) {
        responeText.text = text;
    }


}
