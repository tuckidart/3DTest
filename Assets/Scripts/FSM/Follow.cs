using UnityEngine;

public class Follow : CarrotState
{
    private Vector3 _currentMousePos = Vector3.zero;
    private Vector3 _initialMousePosition = Vector3.zero;
    private Vector3 _movementDelta = Vector3.zero;
    private Vector3 _rotationDelta = Vector3.zero;

    //Force applied when the carrot is released.
    private Vector3 _throwForce = Vector3.zero;

    //Speed values for following and rotating.
    private float _followSpeed = 10f;
    private float _rotationSpeed = 50f;
    private float _rotationAmount = 120f;

    public override void EnterState(Carrot carrot)
    {
        _carrot = carrot;

        //Calculate initial offset between the carrot and the mouse cursor.
        _initialMousePosition = Input.mousePosition - Camera.main.WorldToScreenPoint(_carrot.transform.position);

        _carrot.RB.isKinematic = true;
        _carrot.transform.eulerAngles = Vector3.zero;
    }

    public override void UpdateState()
    {
        SmoothFollow();
        CalculateThrowForce();

        //If the player releases the mouse button, transition to the Recall state.
        if (Input.GetMouseButtonUp(0))
        {
            _carrot.ChangeState(ECarrotState.RECALL);
            return;
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState()
    {
        _carrot.RB.isKinematic = false;

        //Apply the calculated throw force when releasing the carrot.
        _carrot.RB.AddForce(_throwForce, ForceMode.Force);

        Cursor.visible = true;
    }

    private void SmoothFollow()
    {
        //Calculate the target position in world space based on mouse position.
        Vector3 finalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition - _initialMousePosition);
        //Keeping the carrot at a fixed height since we don't move the camera.
        finalPos.y = 1f;

        //Interpolate the carrot's position towards the target.
        _carrot.transform.position = Vector3.Lerp(_carrot.transform.position, finalPos, Time.deltaTime * _followSpeed);

        bool isDragging = false;
        Vector3 direction = _carrot.transform.position - finalPos;

        if (direction != Vector3.zero)
        {
            isDragging = true;
        }

        //Smoothly adjust movement and rotation deltas for natural motion.
        _movementDelta = Vector3.Lerp(_movementDelta, direction, Time.deltaTime * 25f);
        Vector3 movementRotation = (isDragging ? _movementDelta : direction) * _rotationAmount;
        _rotationDelta = Vector3.Lerp(_rotationDelta, movementRotation, Time.deltaTime * _rotationSpeed);

        //Apply rotation, clamping within a safe range to avoid extreme tilting.
        _carrot.transform.eulerAngles = new Vector3(
            Mathf.Clamp(-_rotationDelta.z, -60, 60),
            _carrot.transform.eulerAngles.y,
            Mathf.Clamp(_rotationDelta.x, -60, 60)
        );
    }

    private void CalculateThrowForce()
    {
        //Calculate throwing force based on the change in mouse position.
        Vector3 mouseDelta = (Input.mousePosition - _currentMousePos) * 100.0f;
        _throwForce = new Vector3(mouseDelta.x, 0f, mouseDelta.y);

        //Update the current mouse position for the next frame.
        _currentMousePos = Input.mousePosition;
    }
}
