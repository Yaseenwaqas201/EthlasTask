using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour , IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform joystickBackground;
    [SerializeField] private RectTransform joystickHandle;

    private float maxJoystickDistance;
    private Vector2 joystickDirection;

    private void Start()
    {
        maxJoystickDistance = joystickBackground.sizeDelta.x /3f;
    }

    // Code for the movement of joystick
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position);
        position=Vector2.ClampMagnitude(position, maxJoystickDistance); // Here we restrict the joystick did not move further from background area 
        joystickHandle.anchoredPosition = position;
        joystickDirection = position.normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
//        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickDirection = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetJoystickDirection()
    {
        return joystickDirection;
    }

    public float GetJoystickDistance()
    {
        return Vector2.Distance(joystickHandle.anchoredPosition, Vector2.zero);
    }

    public float GetMaxJoystickDistance()
    {
        return maxJoystickDistance;
    }

    public float GetHorizontalValue()
    {
        return joystickDirection.x;
    }

    public float GetVerticalValue()
    {
        return joystickDirection.y;
    }
}
