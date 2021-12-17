public class CheatButton : ButtonBehaviour
{
    public bool Wipe, Gold, Level;

    protected override void HandleClick()
    {
        if (Wipe)
        {
            Game.CurrentGame.PlayerData = new PlayerData();
            Game.CurrentGame.SaveGame();
        }

        if (Gold)
        {
            Game.CurrentGame.UpdateCoins(1000);
        }

        if (Level)
        {
            Game.CurrentGame.UpdateProgress(Game.CurrentGame.LastLevel());
        }
    }
}