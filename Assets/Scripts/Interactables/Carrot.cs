using System.Collections;
using UnityEngine;
using System;

public class Carrot : Interactable
{
    #region Variables

    [SerializeField]
    private AudioClip _bounceSfx = null;
    [SerializeField]
    private AudioClip _pluckSfx = null;
    [SerializeField]
    private AudioClip _stretchSfx = null;

    [SerializeField]
    private AudioSource _audioSource = null;
    public AudioSource AudioSource => _audioSource;

    [Space]

    [SerializeField]
    private Transform _pivot = null;
    public Transform Pivot => _pivot;

    [SerializeField]
    private Rigidbody _rb = null;
    public Rigidbody RB => _rb;

    private CarrotState _carrotState = null;

    //Predefined states for the carrot.
    private Idle _idle = null;
    private Hover _hover = null;
    private Pluck _pluck = null;
    private Follow _follow = null;
    private Recall _recall = null;

    private bool _plucked = false;
    public bool IsPlucked => _plucked;

    //Delegate for callbacks when the carrot is plucked.
    private Action _onPlucked = null;

    #endregion

    #region Unity Methods

    //Coroutine on start is awesome.
    private IEnumerator Start()
    {
        //Initialize all carrot states.
        _idle = new Idle();
        _hover = new Hover();
        _pluck = new Pluck();
        _follow = new Follow();
        _recall = new Recall();

        //Wait for a random duration before setting the carrot to RECALL state.
        float rand = UnityEngine.Random.Range(0.5f, 5f);
        yield return new WaitForSeconds(rand);

        ChangeState(ECarrotState.RECALL);
    }

    private void Update()
    {
        _carrotState?.UpdateState();
    }

    private void FixedUpdate()
    {
        _carrotState?.FixedUpdateState();
    }

    #endregion

    #region State Methods

    //Changes the carrot's state based on the given state type.
    public void ChangeState(ECarrotState carrotState)
    {
        //Exit the current state before switching.
        _carrotState?.ExitState();

        //Assign the new state based on the enum value.
        switch (carrotState)
        {
            case ECarrotState.IDLE:
                _carrotState = _idle;
                break;
            case ECarrotState.HOVER:
                _carrotState = _hover;
                break;
            case ECarrotState.PLUCK:
                _carrotState = _pluck;
                break;
            case ECarrotState.FOLLOW:
                _carrotState = _follow;
                break;
            case ECarrotState.RECALL:
                _carrotState = _recall;
                break;
            default:
                break;
        }

        //Enter the new state.
        _carrotState?.EnterState(this);
    }

    #endregion

    #region Input Methods

    //Handles mouse click interactions.
    public override void OnMouseDown()
    {
        //If the carrot is in the hover state:
        if (_carrotState == _hover)
        {
            Cursor.visible = false;

            //Initiate PLUCK state if not plucked or else following.
            if (!_plucked)
            {
                ChangeState(ECarrotState.PLUCK);
                return;
            }

            ChangeState(ECarrotState.FOLLOW);
        }
    }

    #endregion

    #region Other Methods

    //Marks the carrot as plucked and detaches it from its parent.
    public void SetPlucked()
    {
        _audioSource.clip = _pluckSfx;
        _audioSource.Play();

        //Marks the carrot as plucked.
        _plucked = true;

        //Detaches it from its parent.
        transform.parent = null;

        //Trigger pluck callbacks.
        _onPlucked?.Invoke();
    }

    //Triggers a bounce effect with a given force.
    public void Bounce(float force)
    {
        StartCoroutine(Squish(force));
    }

    //Coroutine to create a "squish" effect, simulating a bounce.
    private IEnumerator Squish(float force)
    {
        //Duration of the bounce effect.
        //This can be tweaked.
        float _bounceDuration = 0.5f;
        float _bounceTime = 0f;

        while (_bounceTime < _bounceDuration)
        {
            _bounceTime += Time.deltaTime;

            //Calculate the "bouncy" effect using a sine wave.
            float bounceFactor = Mathf.Sin(_bounceTime * Mathf.PI * force / _bounceDuration);
            transform.localScale = Vector3.one + new Vector3(0, bounceFactor * 0.1f, 0);

            yield return null;
        }
    }

    #endregion

    //In a more robust game this should not be here at all.
    #region Audio Methods

    public void PlayBounceSFX()
    {
        _audioSource.clip = _bounceSfx;
        _audioSource.Play();
    }

    public void PlayStretchSFX()
    {
        _audioSource.volume = 0f;
        _audioSource.loop = true;
        _audioSource.clip = _stretchSfx;
        _audioSource.Play();
    }

    #endregion

    #region Callback Methods

    //Registers a callback to be triggered when the carrot is plucked.
    public void AddOnPluckedCallback(Action callback) => _onPlucked += callback;

    #endregion
}
