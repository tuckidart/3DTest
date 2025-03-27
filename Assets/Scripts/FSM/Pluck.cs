using UnityEngine;

public class Pluck : CarrotState
{
    //Stores the initial scale of the carrot.
    private Vector3 _initialCarrotScale = Vector3.zero;

    //Stores the Z-distance of the carrot in screen space for accurate mouse interaction.
    private float _camZDistance = 0f;

    private Vector3 _initialPosition;

    //Maximum upward pull required to successfully pluck the carrot.
    private float _maxPull = 0.7f;
    //Minimum pull limit to prevent downward stretching.
    private float _minPull = -0.5f;

    //This is just to control the volume when stretching
    float _lastVerticalDistance = 0f;

    //This is just to reset the audio back to normal. In a more robust game this should not be here at all.
    float _defaultVolume = 0f;

    public override void EnterState(Carrot carrot)
    {
        _carrot = carrot;
        _initialPosition = _carrot.transform.position;
        _initialCarrotScale = _carrot.transform.localScale;

        //Get the carrot's Z-distance in screen space to maintain proper depth when tracking the mouse.
        _camZDistance = Camera.main.WorldToScreenPoint(carrot.transform.position).z;

        _defaultVolume = _carrot.AudioSource.volume;
        _carrot.PlayStretchSFX();
    }

    public override void UpdateState()
    {
        //Convert the mouse position to world coordinates, maintaining the original Z-depth.
        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camZDistance);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        //Calculate the vertical distance the carrot has been pulled.
        float verticalDistance = mouseWorldPosition.y - _initialPosition.y;

        //Prevent the carrot from being pulled downward beyond the allowed limit.
        if (verticalDistance < _minPull) verticalDistance = _minPull;

        if (verticalDistance > _lastVerticalDistance)
        {
            _carrot.AudioSource.volume = 0.5f;
        }
        else
        {
            _carrot.AudioSource.volume = 0f;
        }

        _lastVerticalDistance = verticalDistance;

        //Stretch the carrot’s height based on the vertical pull distance.
        _carrot.transform.localScale = new Vector3(_initialCarrotScale.x, _initialCarrotScale.y + verticalDistance, _initialCarrotScale.z);

        //If the player pulls the carrot up far enough, change to FOLLOW state.
        if (verticalDistance > _maxPull)
        {
            //Set the carrot as plucked.
            _carrot.SetPlucked();

            //Adjust the position to account for the pull distance.
            _carrot.transform.position = _carrot.transform.position + _carrot.Pivot.localPosition;

            //Apply a bounce effect.
            _carrot.Bounce(4f);
            //Change to Follow state.
            _carrot.ChangeState(ECarrotState.FOLLOW);
            return;
        }

        // If the player releases the mouse before fully pulling the carrot, reset it.
        if (Input.GetMouseButtonUp(0))
        {
            //Only playing here. Otherwise gets annoying.
            _carrot.PlayBounceSFX();

            Cursor.visible = true;
            //Change to Recall state.
            _carrot.ChangeState(ECarrotState.RECALL);
            return;
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState()
    {
        _carrot.AudioSource.volume = _defaultVolume;
        _carrot.AudioSource.loop = false;
    }
}
