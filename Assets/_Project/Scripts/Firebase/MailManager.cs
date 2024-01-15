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
        
        MobileUserDetection();
    }

    
    private void MobileUserDetection()
    {
        var _mobileUserDetectionWebgl = new MobileUserDetectionWebgl();
        if (_mobileUserDetectionWebgl.IsMobileUser())
            OnMobileUserDetected?.Invoke();
    }

    public void ValidateMail(string email)
    {
        var valid = Helper.ValidateEmail(email);
        OnEmailValidated?.Invoke(valid);
    }

    public void FinishMailPanel(string email = "", bool isSkip = false)
    {
        Mail.Email = email;
        Mail.IsSkiped = isSkip;
        OnTestStarted?.Invoke();
    }
}
