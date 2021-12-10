using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchScene : MonoBehaviour
{
    public MenuManager Menu;
    public string TargetScene;
    public float DebounceInterval = 0.2f;

    private float _timeout = 0f;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnSwitch);
    }

    private void Update()
    {
        _timeout -= Time.deltaTime;
        if (_timeout <= 0) _timeout = 0;
    }

    public void OnSwitch()
    {
        if (_timeout <= 0)
        {
            PublicVars.TransitionManager.FadeToScene(TargetScene, 0.5f);
            _timeout = DebounceInterval;
        }
    }
}