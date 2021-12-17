using System.Collections;
using UnityEngine;

public class PlayerShoot : TimeoutBehaviour
{
    public bool Firing = false;
    ArrayList shotSpawners = new ArrayList();

    void Start()
    {
        foreach (Transform tr in transform)
        {
            if (tr.tag == "ShotSpawner")
            {
                shotSpawners.Add(tr);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.touchCount > 0 && !Game.CurrentGame.Paused && CheckAndReset(Game.CurrentGame.PlayerData.rateOfFire))
        {
            foreach (Transform shotSpawner in shotSpawners)
            {
                shotSpawner.GetComponent<PlayerShotSpawner>().shoot();
            }
        }
    }
}
