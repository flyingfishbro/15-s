using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletPoolObject : MonoBehaviour
{
    public BulletGun gun { get; private set; }

    private List<Bullet> bullets;

    const int DEFAULT_POOL_SIZE = 10;


    
    public static BulletPoolObject GetPoolObjectInstance(
        Bullet bulletPrefab,
        BulletGun gun,
        int poolSize
        )
    {
        if (poolSize < DEFAULT_POOL_SIZE) poolSize = DEFAULT_POOL_SIZE;

        BulletPoolObject instance = (new GameObject("BulletPoolObject")).
            AddComponent<BulletPoolObject>();

        instance.gun = gun;
        instance.transform.parent = gun.transform;
        instance.transform.localPosition = Vector3.zero;

        //BulletStatus bulletStatus = Resources.Load<BulletStatusScriptableobject>("Scriptable/BulletStatus").FindGunStatus(gun.gunCode);
        


        instance.bullets = new List<Bullet>();
        for (int i = 0; i < poolSize; ++i)
        {
            Bullet bulletInstance = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);
            instance.bullets.Add( bulletInstance );

            bulletInstance.BulletInitialized(gun, instance);

            bulletInstance.SetBullet(false);
        }

        return instance;
    }



    private int indexOfBullet;

    private void IncreaseIndexOfBullet()
    {
        ++indexOfBullet;
        indexOfBullet %= bullets.Count;
    }

    public Bullet GetBullet()
    {
        Bullet bulletObject = bullets[indexOfBullet];
        bulletObject.SetBullet(true);

        IncreaseIndexOfBullet();

        return bulletObject;
    }



















}
