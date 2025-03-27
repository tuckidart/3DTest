using UnityEngine;

public class Recall : CarrotState
{
    //Stores the initial scale of the carrot at the moment it enters the state.
    private Vector3 _startScale = Vector3.zero;

    //Timer variable for lerping (smooth transition effect).
    private float _elapsedLerp = 0.0f;

    //Duration of the recall animation.
    private readonly float _animationTime = 0.1f;

    //Called when entering the Recall state.
    public override void EnterState(Carrot carrot)
    {
        _carrot = carrot;
        _startScale = _carrot.transform.localScale;
        _elapsedLerp = 0f;
    }

    public override void UpdateState()
    {
        //Increment the lerp progress over time based on the animation duration.
        _elapsedLerp += Time.deltaTime / _animationTime;

        //Smoothly interpolate the carrot's scale from its starting scale back to normal.
        _carrot.transform.localScale = Vector3.Lerp(_startScale, Vector3.one, _elapsedLerp);

        //Once the recall animation is complete:
        if (_elapsedLerp >= 1.0f)
        {
            //Apply a bounce effect when the recall finishes.
            _carrot.Bounce(4f);
            //Change to Idle state.
            _carrot.ChangeState(ECarrotState.IDLE);
            return;
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }
}
