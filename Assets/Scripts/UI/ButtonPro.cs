using UnityEngine.UI;

public class ButtonPro : Button
{
    // todo: review later
    /*
     
    // single click handler
    public float limitTime = 0.5f;
    public UnityEvent clickEvent = new();
    private float _timeDown;
    private bool _isPressed;
    
    // hold handler
    public float holdTime = 1;
    public UnityEvent holdEvent = new();
    private Coroutine _coroutine;

    // double click handler
    public float doubleClickInterval = 0.2f;
    public UnityEvent doubleClickEvent = new();
    private float _lastTimeClick;
    private bool _firstClick;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        _isPressed = true;
        
        // handle single click event
        _timeDown = Time.unscaledTime;
        
        // handle hold event
        _coroutine = StartCoroutine(InvokeHoldEvent());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
        // handle single click
        if (Time.unscaledTime - _timeDown )
        
        // handle hold event
        if (_coroutine != null) StopCoroutine(_coroutine);

        // handle double click
        if (!_firstClick)
        {
            _firstClick = true;
            _lastTimeClick = Time.unscaledTime;
        }
        else
        {
            if (Time.unscaledTime - _lastTimeClick <= doubleClickInterval)
            {
                doubleClickEvent?.Invoke();
                _firstClick = false;
            }
            else
            {
                _lastTimeClick = Time.unscaledTime;
            }
        }
    }

    private IEnumerator InvokeHoldEvent()
    {
        yield return new WaitForSeconds(holdTime);
        holdEvent?.Invoke();
    }
    
    */
}