using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupButton
{
    public GameObject Button;
    public Image ButtonImage;
    public Button ButtonComponent;
    public TextMeshProUGUI ButtonText;
    public int QuestionIndex;

    public PopupButton(GameObject prefab, int index, UnityAction onClick)
    {
        Button =  GameObject.Instantiate(prefab);
        ButtonImage = Button.GetComponent<Image>();
        ButtonComponent = Button.GetComponent<Button>();
        ButtonText = Button.GetComponentInChildren<TextMeshProUGUI>();
        QuestionIndex = index;
        ButtonText.text = QuestionIndex.ToString();
        ButtonComponent.onClick.AddListener(onClick);
    }
}