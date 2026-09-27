using UnityEngine;

public abstract class AccessComponent<T> : MonoBehaviour
    where T : class
{
    private static T _privateReference;

    public static void RegisterReference(T reference)
    {
        _privateReference = reference;
    }

    public static void UnregisterReference(T reference)
    {
        if (_privateReference != reference)
            return;

        _privateReference = null;
    }

    private bool _loggedAlready = false;

    private void _LogMissingReference()
    {
        if (HasReference)
        {
            _loggedAlready = false;
            return;
        }
        if (_loggedAlready)
            return;

        Debug.LogError(
            $"No {typeof(T).Name} reference registered. Please ensure that an implementation of {typeof(T).Name} is registered before using {GetType().Name}.",
            this
        );
        _loggedAlready = true;
    }

    protected T Reference
    {
        get
        {
            _LogMissingReference();
            return _privateReference;
        }
    }

    protected bool HasReference => _privateReference != null;
}
