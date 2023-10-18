using UnityEngine;

public abstract class BaseUI : DuztineBehaviour
{
    [SerializeField] private Canvas canvas;
    
    protected virtual void Awake()
    {
        canvas.worldCamera = Camera.main;
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
