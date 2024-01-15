using UnityEngine;


namespace FirebaseWebGL.Examples.Utils
{
    public class ExampleScenesHandler : MonoBehaviour
    {
        public void GoToDatabaseScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("DatabaseExampleScene");
        
        public void GoToAuthScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("AuthExampleScene");
        
        public void GoToFunctionsScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("FunctionsExampleScene");
        
        public void GoToStorageScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("StorageExampleScene");
        
        public void GoToFirestoreScene() => UnityEngine.SceneManagement.SceneManager.LoadScene("FirestoreExampleScene");
    }
}
