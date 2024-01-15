using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public ButtonProperty ButtonProperty;

    public int Index;
    public Button ButtonComponent;
    public Image ButtonImage;
    public bool IsSelected;

    public void SetCorrectButton()
    {
        ButtonImage.color = ButtonProperty.Correct;
    }

    public void SetWrongButton()
    {
        ButtonImage.color = ButtonProperty.Wrong;
    }
    public void Select()
    {
        if (IsSelected)
        {
            Deselect();
        }
        else
        {
            IsSelected = true;
            ButtonImage.color = ButtonProperty.SelectedHovered;
        }
    }

    public void Deselect()
    {
        IsSelected = false;
        ButtonImage.color = ButtonProperty.Normal;
    }
    public void Hover()
    {
        if (IsSelected)
        {
            ButtonImage.color = ButtonProperty.SelectedHovered;
        }
        else
        {
            ButtonImage.color = ButtonProperty.Hovered;
        }
    }

    public void ResetHover()
    {
        if (IsSelected)
        {
            ButtonImage.color = ButtonProperty.Selected;
        }
        else
        {
            ButtonImage.color = ButtonProperty.Normal;
        }
    }
    public void ResetSelection()
    {
        IsSelected = false;
        ButtonImage.color = ButtonProperty.Normal;
    }

    public void DisableButton()
    {
        ButtonComponent.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameData.IsSolution)
            return;
        AnswerController.Instance.OnHowerChanged?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameData.IsSolution)
            return;
        AnswerController.Instance.OnHowerChanged?.Invoke(-1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameData.IsSolution)
            return;
        if (IsSelected)
            AnswerController.Instance.OnAnswerSelected?.Invoke(-1);
        else
            AnswerController.Instance.OnAnswerSelected?.Invoke(Index);
        
    }
}
