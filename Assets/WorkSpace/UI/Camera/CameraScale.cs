using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        string aspectName = GetAspectRatioName(Screen.width, Screen.height);
        Debug.Log("Aspect Ratio: " + aspectName);

        AdjustCamera(aspectName);
    }

    string GetAspectRatioName(int width, int height)
    {
        float aspect = (float)width / height;
        float epsilon = 0.05f;

        if (Mathf.Abs(aspect - (16f / 9f)) < epsilon) return "16:9";
        if (Mathf.Abs(aspect - (4f / 3f)) < epsilon) return "4:3";
        if (Mathf.Abs(aspect - (19.5f / 9f)) < epsilon) return "19.5:9";
        if (Mathf.Abs(aspect - (3f / 2f)) < epsilon) return "3:2";
        return "unknown";
    }

    void AdjustCamera(string aspectRatio)
    {
        // Với camera Orthographic (thường dùng trong 2D)
        if (mainCamera.orthographic)
        {
            switch (aspectRatio)
            {
                case "4:3":
                    mainCamera.orthographicSize = 6f;
                    break;
                case "16:9":
                    mainCamera.orthographicSize = 5f;
                    break;
                case "19.5:9":
                    mainCamera.orthographicSize = 4.5f;
                    break;
                default:
                    mainCamera.orthographicSize = 5.5f;
                    break;
            }
        }
        // Với camera Perspective (thường dùng trong 3D)
        else
        {
            switch (aspectRatio)
            {
                case "4:3":
                    mainCamera.fieldOfView = 110f;
                    break;
                case "16:9":
                    mainCamera.fieldOfView = 107f;
                    break;
                case "19.5:9":
                    mainCamera.fieldOfView = 95f;
                    break;
                default:
                    mainCamera.fieldOfView = 107f;
                    break;
            }
        }
    }
}
