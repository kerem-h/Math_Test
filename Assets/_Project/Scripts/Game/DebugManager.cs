using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    #region  Singleton

    public static DebugManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(this);
        }
    }
    

    #endregion
     [SerializeField] private TextMeshProUGUI debugText;
     [SerializeField] private GameObject debugPanel;
     public bool IsDebugBuild = false;

     public GameObject debugSelectionUi;
     public TextMeshProUGUI debugQuestionText;
     
     
     private bool _isDebugActive;
     private void Start() {
         debugPanel.SetActive(false);
         ClearDebugText();
     }

     private void Update()
     {
         if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D)) {
             _isDebugActive = !_isDebugActive;
             debugPanel.SetActive(_isDebugActive);
         }
         
         if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F)) {
             OpenDebugPanel();
         }


         if (debugSelectionUi.activeSelf)
         {
             if (Input.GetKeyDown(KeyCode.Alpha1))
             {
                 debugQuestionText.text += "1";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha2))
             {
                 debugQuestionText.text += "2";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha3))
             {
                 debugQuestionText.text += "3";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha4))
             {
                 debugQuestionText.text += "4";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha5))
             {
                 debugQuestionText.text += "5";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha6))
             {
                 debugQuestionText.text += "6";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha7))
             {
                 debugQuestionText.text += "7";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha8))
             {
                 debugQuestionText.text += "8";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha9))
             {
                 debugQuestionText.text += "9";
             }
             else if (Input.GetKeyDown(KeyCode.Alpha0))
             {
                 debugQuestionText.text += "0";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad0))
             {
                 debugQuestionText.text += "0";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad1))
             {
                 debugQuestionText.text += "1";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad2))
             {
                 debugQuestionText.text += "2";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad3))
             {
                 debugQuestionText.text += "3";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad4))
             {
                 debugQuestionText.text += "4";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad5))
             {
                 debugQuestionText.text += "5";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad6))
             {
                 debugQuestionText.text += "6";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad7))
             {
                 debugQuestionText.text += "7";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad8))
             {
                 debugQuestionText.text += "8";
             }
             else if (Input.GetKeyDown(KeyCode.Keypad9))
             {
                 debugQuestionText.text += "9";
             }
             else if (Input.GetKeyDown(KeyCode.Backspace))
             {
                 debugQuestionText.text = debugQuestionText.text.Substring(0, debugQuestionText.text.Length - 1);
             }
             else if (Input.GetKeyDown(KeyCode.Return))
             {
                 if (debugQuestionText.text == "") return;
                 var questionIndex = int.Parse(debugQuestionText.text);
                if ( questionIndex < 0 || questionIndex > GameData.QuestionCount[GameData.CurrentTest]) {
                    debugQuestionText.text = "";
                    return;
                }
                GameManager.Instance.ChangeQuestion(questionIndex - 1);
                 GameManager.Instance.OnQuestionChanged?.Invoke();
                 debugQuestionText.text = "";
                 debugSelectionUi.SetActive(false);

             }
             else if (Input.GetKeyDown(KeyCode.KeypadEnter))
             {
                 if (debugQuestionText.text == "") return;
                 var questionIndex = int.Parse(debugQuestionText.text);
                 if ( questionIndex < 0 || questionIndex > GameData.QuestionCount[GameData.CurrentTest]) {
                     debugQuestionText.text = "";
                     return;
                 }
                 GameManager.Instance.ChangeQuestion(questionIndex - 1 );
                 GameManager.Instance.OnQuestionChanged?.Invoke();
                 debugQuestionText.text = "";
                 debugSelectionUi.SetActive(false);
             }
         }
     }

     public void AddLogs(string log) {
         debugText.text += "--------------------------------\n";
         debugText.text += "-" + log + "\n";
     }
     
     public void ClearDebugText() {
         debugText.text = "";
     }
     
     public void CloseDebugPanel() {
         _isDebugActive = false;
         debugPanel.SetActive(false);
     }

     public void OpenDebugPanel()
     {
         debugSelectionUi.SetActive(!debugSelectionUi.activeSelf);
     }
}
