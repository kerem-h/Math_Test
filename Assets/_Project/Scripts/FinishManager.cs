using TMPro;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [Header("Texts")]

    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI notAnsweredText;
    
    private int correct;
    private int wrong;
    private int blank;

    private void Start()
    {
        var results = GameData.GetResults();
        correct = results[0];
        wrong = results[1];
        blank = results[2];
        SetTexts();
    }

    private void SetTexts()
    {
        correctText.text = correct.ToString();
        wrongText.text = wrong.ToString();
        notAnsweredText.text = blank.ToString();        
    }
    
    public void Solution()
    {
        GameData.IsSolution = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.GameScene.ToString());
    }
    public void Restart()
    {
        GameData.IsSolution = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.GameScene.ToString());
    }
}
