using UnityEngine;

public class LevelPage : MonoBehaviour
{
    public int LevelsPerRow = 3;
    public int StartLevel;
    public int EndLevel;
    public GameObject LevelGroupPrefab;
    public GameObject LevelButtonPrefab;
    public PageManager Page;

    private void Awake()
    {
        float numLevels = (EndLevel - StartLevel);
        int numRow = Mathf.CeilToInt(numLevels / LevelsPerRow);
        for (int i = 0; i < numRow; i++)
        {
            int baseLevel = i * LevelsPerRow;
            LevelGroup levelGroup = Instantiate(LevelGroupPrefab, transform).GetComponent<LevelGroup>();
            levelGroup.Setup(baseLevel, baseLevel + LevelsPerRow - 1, LevelButtonPrefab, Page);
        }
    }
}