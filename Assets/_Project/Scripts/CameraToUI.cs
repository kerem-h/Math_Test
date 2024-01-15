using UnityEngine;
using UnityEngine.UI;

public class CameraToUI : MonoBehaviour
{
    public Camera cameraToRender; 
    public RawImage rawImage;  
    public RenderTexture renderTexture; 

    void Start() {
        
        cameraToRender.targetTexture = renderTexture;
        rawImage.texture = renderTexture;
    }
}
