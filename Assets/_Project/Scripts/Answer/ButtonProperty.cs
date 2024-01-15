using UnityEngine;

[CreateAssetMenu(fileName = "AnswerProperty", menuName = "ButtonProperties", order = 0)]
public class ButtonProperty : ScriptableObject
{
    public Color Hovered;
    public Color Selected;
    public Color SelectedHovered;
    public Color Normal;
    public Color Correct;
    public Color Wrong;
}
