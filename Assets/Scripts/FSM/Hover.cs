using UnityEngine;

public class Hover : CarrotState
{
    //Target scale for the hover effect (enlarged appearance).
    private Vector3 _hoverScale = new Vector3(1.1f, 1.1f, 1.1f);

    //Timer variable for lerping (smooth transition effect).
    private float _elapsedLerp = 0.0f;

    //Duration of the hover animation.
    private readonly float _animationTime = 0.1f;

    private bool _animating = false;

    public override void EnterState(Carrot carrot)
    {
        _carrot = carrot;
        _animating = true;
        _elapsedLerp = 0.0f;
    }

    public override void UpdateState()
    {
        //If the mouse is not over the carrot, return to the Idle state.
        if (!_carrot.IsHovering)
        {
            _carrot.ChangeState(ECarrotState.IDLE);
            return;
        }

        //If the animation has finished, no further processing is needed.
        if (!_animating)
        {
            return;
        }

        //Increment the lerp progress over time based on the animation duration.
        _elapsedLerp += Time.deltaTime / _animationTime;

        //Smoothly interpolate the carrot's scale from its default size to the hover scale.
        _carrot.transform.localScale = Vector3.Lerp(Vector3.one, _hoverScale, _elapsedLerp);

        //Stop the animation once it completes.
        if (_elapsedLerp >= 1.0f)
        {
            _animating = false;
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState()
    {
        _carrot.transform.localScale = Vector3.one;
    }
}
