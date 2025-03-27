public class Idle : CarrotState
{
    public override void EnterState(Carrot carrot)
    {
        _carrot = carrot;
    }

    public override void UpdateState()
    {
        //If the carrot is in a hovering condition, switch to the HOVER state.
        if (_carrot.IsHovering)
        {
            _carrot.ChangeState(ECarrotState.HOVER);
            return;
        }
    }

    public override void FixedUpdateState() { }

    public override void ExitState() { }
}
