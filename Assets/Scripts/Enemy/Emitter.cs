using System.Collections;
using UnityEngine;

public class Emitter : MonoBehaviour
{

    private ObjectPooler objectPooler;
    private bool canShoot = true;

    [SerializeField]
    private float rateOfFire = 0.1f;

    [SerializeField]
    private float shotSpeed = 8f;

    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;

    [SerializeField]
    private int spokes = 3;

    [SerializeField]
    private int angularVelocity = 180;

    void Start()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    void Update()
    {
        transform.Rotate(0, 0, angularVelocity * Time.deltaTime);
        if (canShoot)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        canShoot = false;

        float thetaStep = (endAngle - startAngle) / spokes;
        float initialAngle = startAngle;

        for (int i = 0; i < spokes + 1; i++)
        {
            float xDir = transform.position.x + Mathf.Sin(initialAngle * Mathf.PI / 180f);
            float yDir = transform.position.y + Mathf.Cos(initialAngle * Mathf.PI / 180f);

            Vector3 shotVector = new Vector3(xDir, yDir, 0f);
            Vector2 shotDirection = (shotVector - transform.position).normalized;

            GameObject shot = objectPooler.instantiateObjFromPool("EnemyShotType1", transform.position, transform.rotation);
            shot.GetComponent<EnemyShotBehavior>().setDirection(shotDirection);
            shot.GetComponent<EnemyShotBehavior>().setShotSpeed(shotSpeed);
            initialAngle += thetaStep;
        }

        yield return new WaitForSeconds(rateOfFire);

        canShoot = true;
    }
}
