public class ToggleAbility : ButtonBehaviour
{
    public Ability ability;

    protected override void HandleClick()
    {
        PlayerData data = Game.CurrentGame.PlayerData;
        if (!data.Own((int)ability + ((int)Purchasable.Boom))) return;

        data.AbilitiesEnabled[((int)ability)] = !data.AbilitiesEnabled[((int)ability)];
        Game.CurrentGame.SaveGame();
    }
}