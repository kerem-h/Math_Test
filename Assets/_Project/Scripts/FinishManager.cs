using TMPro;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [Header("Texts")]

    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI notAnsweredText;
    
    public DatabaseHandler databaseHandler;
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
        FirebaseSender.SendData(databaseHandler, correct, wrong, blank);
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
