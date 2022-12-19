using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private float _screenWidth;
    private float _screenHeight;

    public float ScreenWidth => _screenWidth;
    public float ScreenHeight => _screenHeight;

    void Awake()
    {
        if (Instance is not null)
            Debug.LogError("ERROR: More than one instance of Camera script");
        else
            Instance = this;

        var cam = Camera.main;

        if (cam is not null)
        {
            _screenWidth = cam.orthographicSize * cam.aspect;
            _screenHeight = cam.orthographicSize;
        }
    }
}