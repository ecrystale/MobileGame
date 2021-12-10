using UnityEngine;
using UnityEngine.UI;

public class ButtonContinue : ButtonBehaviour
{
    protected override void HandleClick()
    {
        Game.CurrentGame.Continue();
    }
}