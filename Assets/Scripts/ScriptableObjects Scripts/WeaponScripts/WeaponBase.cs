using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon", order = 1)]
public class WeaponBase : ScriptableObject , WeaponInterface 
{
    public float delayTimeBtwBullets; // Delay time Between Firing Bullets one by one
    public int speedOfBullet;
    public int lifeTimeOfBullet;
    public  int damageOfBullet;
    [SerializeField] private Bullet bulletPrefab;
    private List<Bullet> bullets;



    private void OnEnable()
    {
        bullets=new List<Bullet>();
        UpdateBulletValue();
    }

    void UpdateBulletValue()
    {
        bulletPrefab.bulletSpeed = speedOfBullet;
        bulletPrefab.lifetime = lifeTimeOfBullet;
        bulletPrefab.bulletDamage = damageOfBullet;
    }
    public virtual void Fire(Transform FirePoint )
    {
        Bullet bulletAssign=null;
           
            if (bullets.Count > 0)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (!bullet.gameObject.activeSelf)
                    {
                        bulletAssign = bullet;
                        bullet.FireBulletFromFirePos(FirePoint);
                        break;
                    }
                }
            }

            if (bulletAssign == null)
            {
                Bullet newbullet = Instantiate(bulletPrefab, FirePoint.position, quaternion.identity,FirePoint);
                bullets.Add(newbullet);
            }
              
    }
}
