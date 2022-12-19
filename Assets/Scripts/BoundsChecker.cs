using UnityEngine;

public class BoundsChecker : MonoBehaviour
{
    public enum OnExitingScreenAction
    {
        DoNothing,
        // Pooling is not supported yet
        DeleteObject,
        HoldOnScreen
    }
    
    // Idk how to name it properly -\(o_o)/-
    private delegate void EmptyDelegate();
    private event EmptyDelegate OnExitingScreen;

    [SerializeField] private OnExitingScreenAction _onExitingScreenAction = OnExitingScreenAction.DoNothing;
    [SerializeField] private float _outOfScreenThreshold;
    
    // SerializeField only for debugging
    [SerializeField] private bool _isOnScreen = true;
    private float _screenWidth;
    private float _screenHeight;
    
    private Transform _localTransform;

    private void Start()
    {
        _localTransform = transform;
        
        _screenWidth = CameraManager.Instance.ScreenWidth;
        _screenHeight = CameraManager.Instance.ScreenHeight;

        switch (_onExitingScreenAction)
        {
            case OnExitingScreenAction.DoNothing:
                break;
            case OnExitingScreenAction.DeleteObject:
                OnExitingScreen = () => Destroy(gameObject);
                break;
            case OnExitingScreenAction.HoldOnScreen:
                OnExitingScreen = HoldOnScreen;
                break;
        }
    }

    private void LateUpdate()
    {
        var pos = _localTransform.position;

        if (pos.x > _screenWidth + _outOfScreenThreshold)
            _isOnScreen = false;
        if (pos.x < -_screenWidth - _outOfScreenThreshold)
            _isOnScreen = false;
        if (pos.y > _screenHeight + _outOfScreenThreshold)
            _isOnScreen = false;
        if (pos.y < -_screenHeight - _outOfScreenThreshold)
            _isOnScreen = false;

        if (!_isOnScreen)
            OnExitingScreen?.Invoke();
    }

    private void HoldOnScreen()
    {
        var pos = _localTransform.position;
        var newPos = new Vector3(pos.x, pos.y, pos.z);

        // TODO: do it in a less stupid way
        if (pos.x > _screenWidth + _outOfScreenThreshold)
            newPos.x = _screenWidth + _outOfScreenThreshold;
        if (pos.x < -_screenWidth - _outOfScreenThreshold)
            newPos.x = -_screenWidth - _outOfScreenThreshold;
        if (pos.y > _screenHeight + _outOfScreenThreshold)
            newPos.y = _screenHeight + _outOfScreenThreshold;
        if (pos.y < -_screenHeight - _outOfScreenThreshold)
            newPos.y = -_screenHeight - _outOfScreenThreshold;

        _localTransform.position = newPos;

        _isOnScreen = true;
    }
}