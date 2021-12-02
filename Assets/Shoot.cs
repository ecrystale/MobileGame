using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public float bulletspd;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newbullet = Instantiate(bullet, transform.position, Quaternion.identity);
            newbullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletspd * Input.mousePosition.x, bulletspd * Input.mousePosition.y + 1));
        }
    }
}
