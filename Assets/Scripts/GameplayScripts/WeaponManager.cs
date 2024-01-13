using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// Weapon Manager responsible for Firing of weapon and Selection of Weapon
public class WeaponManager : MonoBehaviour
{
    [Header(" List of Weapons")]
    public List<WeaponBase> weaponList=new List<WeaponBase>();
    public WeaponBase selectedWeapon;

    [Header(" References for Firing of Bullets")]
    [SerializeField]
    private Transform firePoint;
    private bool isOpenToFireNextBullet=true;
    
    private void Awake()
    {
        AssignRandomWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            FireBullet();
        }
    }

    void AssignRandomWeapon()
    { 
        int randomWeapon=Random.Range(0, weaponList.Count);
        selectedWeapon = weaponList[randomWeapon];
    }

   

    // Here We Fire the Bullet From Selected Weapon
    public void FireBullet()
    {
        if(!isOpenToFireNextBullet)
            return;
        StartCoroutine(StartDelayTimeBtwBullets());
        IEnumerator StartDelayTimeBtwBullets()
        {
            isOpenToFireNextBullet = false;
            selectedWeapon.Fire(firePoint);
            yield return new WaitForSeconds(selectedWeapon.delayTimeBtwBullets);
            isOpenToFireNextBullet = true;
        }
        
    }
}