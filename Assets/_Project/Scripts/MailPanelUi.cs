using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailPanelUi : MonoBehaviour
{
    public GameObject MailPanel;
    public TMP_InputField EmailInputField;
    
    public Button StartButton;
    public GameObject SkipPanel;


    

    private void Start()
    {
        if (GameData.IsSolution)
        {
            MailPanel.SetActive(false);
            return;
        }
        MailManager.Instance.OnTestStarted += CloseMenuPanel;
        MailManager.Instance.OnMobileUserDetected += DisableSkipButton;
        MailManager.Instance.OnEmailValidated += OnEmailValidated;
        MailManager.Instance.OnSolutionPanel += CloseMenuPanel;
        StartButton.interactable = false;
        EmailInputField.onValueChanged.AddListener(MailManager.Instance.ValidateMail);
    }

    private void CloseMenuPanel() {
        MailPanel.SetActive(false);
    }

    private void OnEmailValidated(bool isValid)
    {
        StartButton.interactable = isValid;
    }
    private void DisableSkipButton()
    {
        SkipPanel.SetActive(false);
    }
    public void StartButtonClicked()
    {
        var email = EmailInputField.text;
        
        MailManager.Instance.FinishMailPanel(email);
        
    }
    public void SkipButtonClicked()
    {
        MailManager.Instance.FinishMailPanel(isSkip: false);
    }
}
