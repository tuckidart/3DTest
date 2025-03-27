using UnityEngine;

[DisallowMultipleComponent]
public class Interactable : MonoBehaviour
{
    protected bool _hovering = false;
    public bool IsHovering => _hovering;

    //Called when the mouse enters the object's collider.
    public virtual void OnMouseEnter()
    {
        _hovering = true;
    }

    //Called when the mouse exits the object's collider.
    public virtual void OnMouseExit()
    {
        _hovering = false;
    }

    //Called when the player presses the mouse button while hovering over the object.
    public virtual void OnMouseDown() { }

    //Called when the player releases the mouse button while hovering over the object.
    public virtual void OnMouseUp() { }
}
