using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonBehaviour : TimeoutBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HandlerWrapper);
    }

    private void HandlerWrapper()
    {
        if (CheckAndReset(PublicVars.DEBOUNCE_INTERVAL))
        {
            HandleClick();
        }
    }

    protected abstract void HandleClick();
}