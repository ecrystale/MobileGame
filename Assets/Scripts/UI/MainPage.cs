public class MainPage : PageManager
{
    public LevelButton StartButton;

    public void Setup()
    {
        StartButton.ContinueMode = !Game.CurrentGame.IsInGame;
        if (Game.CurrentGame.IsInGame)
        {
            StartButton.Text.text = "Restart";
            StartButton.levelIDToLoad = Game.CurrentGame.CurrentLevelSummary.ID;
            return;
        }

        StartButton.Text.text = (Game.CurrentGame.LevelProgress == 0) ? "Start" : "Continue";
    }
}
