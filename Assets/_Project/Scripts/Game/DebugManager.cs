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
     
}
