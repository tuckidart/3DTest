public enum ECarrotState
{
    NONE = 0,

    IDLE,   //Carrot is stationary.
    HOVER,  //Mouse over carrot
    RECALL, //Carrot is being reseted to its default values.
    PLUCK,  //Carrot is being plucked.
    FOLLOW  //Carrot is following the mouse.
}

public abstract class CarrotState
{
    protected Carrot _carrot = null;

    public abstract void EnterState(Carrot carrot);
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
}
