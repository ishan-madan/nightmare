using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollide : MonoBehaviour
{
    GunManager gunManager;

    void Start() {
        gunManager = FindObjectOfType<GunManager>();
    }
    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player"){
            if (transform.tag == "shotgunBullet"){
                gunManager.shotgunBullets += 3;
                if (gunManager.shotgunBullets > 15){
                    gunManager.shotgunBullets = 15;
                }
            }

            if (transform.tag == "sniperBullet"){
                gunManager.sniperBullets += 3;
                if (gunManager.sniperBullets > 10){
                    gunManager.sniperBullets = 10;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
