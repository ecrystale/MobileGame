using UnityEngine;

public class PlayerShoot : TimeoutBehaviour
{
    public bool Firing = false;
    private int _numberOfBullets => Game.CurrentGame.PlayerData.NumberOfBullets;
    private float _rateOfFire => Game.CurrentGame.PlayerData.RateOfFire;
    private float _bulletSpawnOffset => Game.CurrentGame.PlayerData.BulletSpawnOffset;

    float shotSpawnOffset = 0.3f;

    protected override void Update()
    {
        base.Update();
        if (Input.touchCount > 0 && !Game.CurrentGame.Paused && CheckAndReset(_rateOfFire))
        {
            int bulletsCount = _numberOfBullets;
            Vector2 spawnOffset = Vector2.right * shotSpawnOffset;
            Vector2 spawnStart = Vector2.up * shotSpawnOffset + (Vector2)transform.position - (spawnOffset * (bulletsCount - 1) / 2);
            for (int i = 0; i < bulletsCount; i++)
            {
                Game.CurrentGame.ObjectPooler.instantiateObjFromPool("PlayerShot", spawnStart + spawnOffset * i, Quaternion.identity);
            }
        }
    }
}
