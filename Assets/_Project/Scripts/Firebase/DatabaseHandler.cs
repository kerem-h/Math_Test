using FirebaseWebGL.Scripts.FirebaseBridge;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    public string pathInputField;
    public string valueInputField;
    

    public void PostJSON() => FirebaseDatabase.PostJSON(pathInputField, valueInputField, gameObject.name,
        "DisplayInfo", "DisplayErrorObject");
}
