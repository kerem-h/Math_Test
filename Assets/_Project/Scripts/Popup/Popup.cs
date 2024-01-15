using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Popup : MonoBehaviour
{
    public static Popup Instance;
    
    public Action OnPopupOpened;
    public Action OnPopupClosed;

    public Action<int> OnPopupButtonClicked;
    public Action OnPopupCreated;


    public GameObject buttonPrefab;
    [SerializeField] private int buttonCount;
    [SerializeField] private int layoutCount;

    public int ButtonCount => buttonCount;

    public int LayoutCount => layoutCount;


    public List<PopupButton> Buttons { get; private set; }
    private bool _isPopupOpen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        GameManager.Instance.OnQuestionChanged += ClosePopup;
        InitializePopup();
    }

    private void InitializePopup()
    {
        Buttons = new List<PopupButton>();
        for (int i = 1; i <= ButtonCount; i++)
        {
            var index = i;
            var tempButton = new PopupButton(buttonPrefab, index, () =>
            {
                PopupButtonClicked();
                OnPopupButtonClicked?.Invoke(index);
            });
            Buttons.Add(tempButton);
        }
        OnPopupCreated?.Invoke();
    }

    public void ClosePopup()
    {
        if (!_isPopupOpen) return;
        OnPopupClosed?.Invoke();
        _isPopupOpen = false;
    }
    public void PopupButtonClicked()
    {
        if (_isPopupOpen)
        {
            OnPopupClosed?.Invoke();
        }
        else
        {
            OnPopupOpened?.Invoke();
        }
        _isPopupOpen = !_isPopupOpen;
    }
}
