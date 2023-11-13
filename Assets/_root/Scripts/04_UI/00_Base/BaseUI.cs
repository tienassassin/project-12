using UnityEngine;

public abstract class BaseUI : AssassinBehaviour
{
    [SerializeField] private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        canvas.worldCamera = Camera.main;
        AssignUICallback();
    }

    protected virtual void AssignUICallback()
    {
    }

    public virtual void Open(params object[] args)
    {
        gameObject.SetActive(true);
    }

    public virtual void Close(params object[] args)
    {
        gameObject.SetActive(false);
    }
}
