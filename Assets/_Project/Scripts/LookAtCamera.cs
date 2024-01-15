using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    
    private Camera mainCamera;
    private void Start() {
        mainCamera = Camera.main;
        transform.LookAt(mainCamera.transform);
    }
}
