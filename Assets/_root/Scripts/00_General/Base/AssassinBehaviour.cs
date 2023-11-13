using UnityEngine;

public abstract class AssassinBehaviour : MonoBehaviour
{
    private Transform _cachedTransform;

    public Transform Transform
    {
        get
        {
            if (!_cachedTransform)
            {
                _cachedTransform = this.transform;
            }

            return _cachedTransform;
        }   
    }

    protected virtual void Awake()
    {
        RegisterEvents();
    }

    protected virtual void OnDestroy()
    {
        UnregisterEvents();
    }

    protected virtual void RegisterEvents() { }

    protected virtual void UnregisterEvents() { }
}
