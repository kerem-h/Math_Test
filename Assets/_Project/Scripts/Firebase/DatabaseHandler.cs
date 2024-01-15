using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    public string pathInputField;
    public string valueInputField;
    

    public void GetJSON() =>
        FirebaseDatabase.GetJSON(pathInputField, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void PostJSON() => FirebaseDatabase.PostJSON(pathInputField, valueInputField, gameObject.name,
        "DisplayInfo", "DisplayErrorObject");

    public void PushJSON() => FirebaseDatabase.PushJSON(pathInputField, valueInputField, gameObject.name,
        "DisplayInfo", "DisplayErrorObject");

    public void UpdateJSON() => FirebaseDatabase.UpdateJSON(pathInputField, valueInputField,
        gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void DeleteJSON() =>
        FirebaseDatabase.DeleteJSON(pathInputField, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForValueChanged() =>
        FirebaseDatabase.ListenForValueChanged(pathInputField, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForValueChanged() => FirebaseDatabase.StopListeningForValueChanged(pathInputField, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForChildAdded() =>
        FirebaseDatabase.ListenForChildAdded(pathInputField, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForChildAdded() => FirebaseDatabase.StopListeningForChildAdded(pathInputField, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForChildChanged() =>
        FirebaseDatabase.ListenForChildChanged(pathInputField, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForChildChanged() => FirebaseDatabase.StopListeningForChildChanged(pathInputField, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForChildRemoved() =>
        FirebaseDatabase.ListenForChildRemoved(pathInputField, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForChildRemoved() => FirebaseDatabase.StopListeningForChildRemoved(pathInputField, gameObject.name, "DisplayInfo", "DisplayErrorObject");
    

    public void ToggleBooleanWithTransaction() =>
        FirebaseDatabase.ToggleBooleanWithTransaction(pathInputField, gameObject.name, "DisplayInfo",
            "DisplayErrorObject");

    public void DisplayData(string data)
    {
        //outputText.color = outputText.color == Color.green ? Color.blue : Color.green;
        //outputText.text = data;
        Debug.Log(data);
    }

    public void DisplayInfo(string info)
    {
        //outputText.color = Color.white;
        //outputText.text = info;
        Debug.Log(info);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        //outputText.color = Color.red;
        //outputText.text = error;
        Debug.LogError(error);
    }
}
