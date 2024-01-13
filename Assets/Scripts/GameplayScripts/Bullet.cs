using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed=5;
    public int bulletDamage=10;

    public float lifetime = 2f; // Adjust the lifetime as needed
    private float lifeTimeTimer;
    

    private void FixedUpdate()
    {
        // Here we moving the bullet with constant speed dependent on bulletspeed
        transform.Translate(Vector2.right*bulletSpeed*Time.deltaTime);
        
        // Reduce the lifetime
        lifeTimeTimer -= Time.deltaTime;

        // Disable the bullet if its lifetime is over
        if (lifeTimeTimer <= 0f)
        {
            ThingsToBeDoneWhenHitAndLifeTimeExpire();
        }
    }

    // Here  we disable the bullets when it Hit something or its life time Expire
    public void ThingsToBeDoneWhenHitAndLifeTimeExpire()
    {
        gameObject.SetActive(false);
    }

     // Here we set the bullets rotation  and position as of the Fire point Position
    public void FireBulletFromFirePos(Transform firePointTransform)
    {
        transform.rotation = firePointTransform.rotation;
        transform.position = firePointTransform.position;
        gameObject.SetActive(true);
        lifeTimeTimer = lifetime;
        transform.SetParent(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            ThingsToBeDoneWhenHitAndLifeTimeExpire();
        }
    }
}
