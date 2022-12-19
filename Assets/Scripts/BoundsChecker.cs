using UnityEngine;

public class BoundsChecker : MonoBehaviour
{
    public static int ScreenWidth
    {
        get
        {
            if (Camera.main != null) return Camera.main.pixelWidth;
            return 0;
        }
    }

    public static int ScreenHeight
    {
        get
        {
            if (Camera.main != null) return Camera.main.pixelHeight;
            return 0;
        }
    }
    
    private bool _isOnScreen = true;
    
    [SerializeField] private bool _deleteIfNotOnScreen = false;
    [SerializeField] private float _outOfScreenModifier = 0;
    
    

    private void Start()
    {
    }
    
}