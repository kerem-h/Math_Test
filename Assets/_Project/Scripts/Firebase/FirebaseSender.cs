using System;
using UnityEngine;

public static class FirebaseSender
{
    public static void SendData(DatabaseHandler _handler, int correct, int wrong, int blank)
    {
        if (Mail.IsSkiped || Application.platform != RuntimePlatform.WebGLPlayer) return;
        
        _handler.pathInputField = Mail.Email + " "+ DateTime.Now.ToString("dd:MM:yyyy HH:mm:ss");

        _handler.valueInputField = "{\"correct\": " + correct + ", \"wrong\": " +
                                    wrong + ", \"blank\": " +
                                    blank + "}";
        _handler.PostJSON();
    }
}
