using System;
using UnityEngine;

public class AnswerController : MonoBehaviour
{
    #region Singletion

    public static AnswerController Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    public Action<int> OnAnswerSelected;
    public Action<int> OnHowerChanged;
    
    public int HoverIndex
    {
        get => _hoverIndex;
        set
        {
            _hoverIndex = value;
            OnHowerChanged?.Invoke(_hoverIndex);
        }
    }
    public int SelectedAnswerIndex
    {
        get => _selectedAnswerIndex;
        set
        {
            GameData.SelectQuestion(value);
            _selectedAnswerIndex = value;
            OnAnswerSelected?.Invoke(_selectedAnswerIndex);
        }
    }

    

    private int _hoverIndex = -2;
    private int _selectedAnswerIndex;

    private void Start()
    {
        ResetAnswers();
        if (GameData.IsSolution) return;
        GameManager.Instance.OnQuestionChanged += ResetAnswers;

        InputManager.Instance.OnNextAnswer += () => ChangeHowerIndex(_hoverIndex + 1);
        InputManager.Instance.OnPreviousAnswer += () => ChangeHowerIndex(_hoverIndex - 1);
        InputManager.Instance.OnAnswerSelected += () => SelectedAnswerIndex = _hoverIndex;
    }
    
    private void ChangeHowerIndex(int index) {
        if (index < 0) index = GameData.AnswerCount - 1;
        else if (index >= GameData.AnswerCount) index = 0;
        HoverIndex = index;
    }
    
    private void ResetAnswers()
    {
        HoverIndex = 0;
        GameData.GetSelectedAnswerIndex((value) => 
        {
            SelectedAnswerIndex = value;
        });
    }


    
}
