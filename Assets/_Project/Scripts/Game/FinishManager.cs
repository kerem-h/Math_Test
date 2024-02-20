using TMPro;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [Header("Texts")]

    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI notAnsweredText;
    
    //[SerializeField] private TextMeshProUGUI correctText2;
    //[SerializeField] private TextMeshProUGUI wrongText2;
    //[SerializeField] private TextMeshProUGUI notAnsweredText2;
    
    //[SerializeField] private TextMeshProUGUI correctText3;
    //[SerializeField] private TextMeshProUGUI wrongText3;
    //[SerializeField] private TextMeshProUGUI notAnsweredText3;

    public DatabaseHandler databaseHandler;
    private int[] correct;
    private int[] wrong;
    private int[] blank;

    private void Start()
    {
        var results = GameData.GetResults();
        correct = results[0];
        wrong = results[1];
        blank = results[2];
        SetTexts();
        FirebaseSender.SendData(databaseHandler, correct[0], wrong[0], blank[0]);
    }

    private void SetTexts()
    {
        correctText.text = correct[0].ToString();
        wrongText.text = wrong[0].ToString();
        notAnsweredText.text = blank[0].ToString();        
        
        // correctText2.text = correct[1].ToString();
        // wrongText2.text = wrong[1].ToString();
        // notAnsweredText2.text = blank[1].ToString();        

        // correctText3.text = correct[2].ToString();
        // wrongText3.text = wrong[2].ToString();
        // notAnsweredText3.text = blank[2].ToString();        
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
