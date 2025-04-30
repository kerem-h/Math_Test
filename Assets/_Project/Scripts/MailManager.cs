using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MailManager : MonoBehaviour
{
    public static MailManager Instance;
    
    public Action OnMobileUserDetected;
    public Action<bool> OnEmailValidated;
    public Action OnTestStarted;
    public Action OnSolutionPanel;


    public string Email;

    public bool isValid { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(this);
        }
    }

    private void Start()
    {
        if (GameData.IsSolution) {
            FinishMailPanel();
            return;
        }
        
   
    }

   

    public void ValidateMail(string email)
    {
        var valid = Helper.ValidateEmail(email);
        OnEmailValidated?.Invoke(valid);
    }

    public void FinishMailPanel(string email = "", bool isSkip = false)
    {
        Email = email;
        Debug.Log(Email);
        Mail.IsSkiped = isSkip;
        OnTestStarted?.Invoke();
    }
}
