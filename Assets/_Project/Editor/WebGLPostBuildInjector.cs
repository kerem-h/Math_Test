using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;

public class WebGLPostBuildInjector
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // Only run if building for WebGL
        if (target != BuildTarget.WebGL) return;

        // Path to the index.html in the WebGL build folder
        string indexFile = Path.Combine(pathToBuiltProject, "index.html");
        if (!File.Exists(indexFile))
        {
            Debug.LogWarning("index.html not found at: " + indexFile);
            return;
        }

        // Read the existing file
        string content = File.ReadAllText(indexFile);

        // This is the snippet we want to insert above </body>
        // It references Firebase scripts, config, initialization, plus the saveTestData function.
        string firestoreSnippet = @"
    <!-- Firebase + Firestore -->
    <script src=""https://www.gstatic.com/firebasejs/8.10.1/firebase-app.js""></script>
    <script src=""https://www.gstatic.com/firebasejs/8.10.1/firebase-firestore.js""></script>
    <script>
      // Firebase Configuration
      var firebaseConfig = {
          apiKey: ""AIzaSyBqjg2SFpRD1nDVpzI0xATysAAaJLGuz2s"",
          authDomain: ""devenez-pilote-649a8.firebaseapp.com"",
          projectId: ""devenez-pilote-649a8"",
          storageBucket: ""devenez-pilote-649a8.appspot.com"",
          messagingSenderId: ""717174658347"",
          appId: ""1:717174658347:web:0d1ce9920ce6c0b10ce16e"",
          measurementId: ""G-38N76PH36F""
      };
      // Initialize Firebase if not already
      if (!firebase.apps.length) {
        firebase.initializeApp(firebaseConfig);
      }
      var db = firebase.firestore();

      /**
       * Called from Unity via:
       * Application.ExternalCall(
       *   ""saveTestData"",
       *   user,         // e.g. 'user@example.com'
       *   testName,     // e.g. 'Instrument Test'
       *   timestamp,    // e.g. '2025-02-19T12:34:56Z'
       *   elapsedTime,  // e.g. 30.0
       *   testScore     // e.g. 85.0
       * );
       */
      function saveTestData(user, testName, timestamp, elapsedTime, testScore) {
        console.log(""Received from Unity:"", { user, testName, timestamp, elapsedTime, testScore });
        try {
          const testData = {
            ElapsedTime: elapsedTime,
            TestScore: testScore
          };

          // Save to Firestore under: /users/{user}/{testName}/{timestamp}
          db.collection(""users"")
            .doc(user)
            .set({
              [testName]: {
                [timestamp]: testData
              }
            }, { merge: true })
            .then(() => {
              console.log(""Test result successfully saved to Firestore!"");
            })
            .catch((error) => {
              console.error(""Error writing to Firestore:"", error);
            });
        } catch (error) {
          console.error(""Error in saveTestData:"", error);
        }
      }
    </script>
";

        // We'll insert right before the closing </body> tag
        const string insertionTag = "</body>";
        int insertIndex = content.IndexOf(insertionTag);
        if (insertIndex == -1)
        {
            Debug.LogWarning("Could not find </body> tag in index.html. Script not injected.");
            return;
        }

        // Insert the snippet above </body>
        content = content.Insert(insertIndex, firestoreSnippet + "\n");

        // Write the modified file back to disk
        File.WriteAllText(indexFile, content);

        Debug.Log("Firebase & saveTestData snippet injected into index.html at: " + indexFile);
    }
}
