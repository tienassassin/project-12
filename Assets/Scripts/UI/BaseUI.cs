using UnityEngine;

public abstract class BaseUI : DuztineBehaviour
{
    [SerializeField] private Canvas canvas;
    
    protected virtual void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public virtual void Show(params object[] pars)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide(params object[] pars)
    {
        gameObject.SetActive(false);
    }
}
