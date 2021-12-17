using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonBehaviour : TimeoutBehaviour
{
    public Button Button;
    private void Start()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(HandlerWrapper);
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