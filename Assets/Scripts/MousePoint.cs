using MyGame;
using UnityEngine;
using System;

/// <summary>
/// Defines the <see cref="MousePoint" />.
/// </summary>
public class MousePoint : MonoBehaviour
{
    /// <summary>
    /// Defines the hit.
    /// </summary>
    /// 
    internal RaycastHit hit;

    /// <summary>
    /// Defines the raycastlength.
    /// </summary>
    private float raycastlength = 100;

    /// <summary>
    /// The Update.
    /// </summary>
    internal void FixedUpdate()
    {
        bool keyLeftpressed = Input.GetMouseButton(0);
        bool keyRightpressed = Input.GetMouseButton(1);
        bool key_middle_pressed = Input.GetMouseButton(2);

        if (keyLeftpressed || keyRightpressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, raycastlength))
            {
                if (keyLeftpressed)
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.name == Constants.c_terrain_tag )
                    {
                        MouseEvents.mouseClick.Invoke(new ClickEventData(mouseButtons.left_button, hit.point, hit.collider.gameObject, false, true));
                    }
                }
                else if (keyRightpressed)
                {
                    if (hit.collider.name == Constants.c_terrain_tag)
                        MouseEvents.mouseClick.Invoke(new ClickEventData(mouseButtons.right_button, hit.point, hit.collider.gameObject, false, true));
                    
                } else if (key_middle_pressed)
                {
                    MouseEvents.mouseClick.Invoke(new ClickEventData(mouseButtons.middle_button, hit.point, hit.collider.gameObject, false, true));
                }
            }
        }
    }
}
