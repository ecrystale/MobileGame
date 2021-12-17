using UnityEngine.UI;

public class ToggleAbility : ButtonBehaviour
{
    public Text ToggleText;
    public Ability Ability;
    public bool Enabled;

    protected override void HandleClick()
    {
        PlayerData data = Game.CurrentGame.PlayerData;
        if (((int)Ability) < 0 || !data.Own((int)Ability + ((int)Purchasable.Boom))) return;

        data.AbilitiesEnabled[((int)Ability)] = !data.AbilitiesEnabled[((int)Ability)];
        Enabled = data.AbilitiesEnabled[((int)Ability)];
        UpdateText();
        Game.CurrentGame.SaveGame();
    }

    public void UpdateText()
    {
        ToggleText.text = Enabled ? "Disable" : "Enable";
    }
}