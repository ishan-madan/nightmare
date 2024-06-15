using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerARShot = 15;
    public float timeBetweenARBullets = 0.15f;
    public int damagePerShotgunShot = 20;
    public float timeBetweenShotgunBullets = 1.25f;
    public int damagePerSniperShot = 200;
    public float timeBetweenSniperBullets = 4.5f;
    public float range = 100f;


    float timer = 0;
    [HideInInspector]
    public float ARTimer = 10;
    [HideInInspector]
    public float shotgunTimer = 10;
    [HideInInspector]
    public float sniperTimer = 10;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    public AudioSource ARAudio;
    public AudioSource ShotgunAudio;
    public AudioSource SniperAudio;
    public AudioSource gunMisfire;
    Light gunLight;
    float effectsDisplayTime = 0.1f;
    Ray[] shotgunRays = new Ray[9];
    RaycastHit shotgunHit;
    public LineRenderer[] shotgunLines;
    RaycastHit[] sniperHit;
    GunManager gunManager;
    public Material ARMat;
    public Material SniperMat;
    public Color ARLightColor = new Color(1, 0.8455189f, 0.3820755f);
    public Color ShotgunLightColor = new Color(0.5f, 0, 1);
    public Color SniperLightColor = new Color(1, 0, 0);
    ParticleSystem.MainModule main;


    void Awake ()
    {
        for (int i = 0; i < shotgunRays.Length; i++){
            shotgunRays[i] = new Ray();
        }

        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunLight = GetComponent<Light> ();
        main = gunParticles.main;
    }

    void Start() {
        gunManager = FindObjectOfType<GunManager>();
    }


    void Update ()
    {
        timer += Time.deltaTime;
        ARTimer += Time.deltaTime;
        shotgunTimer += Time.deltaTime;
        sniperTimer += Time.deltaTime;
		

        if(timer >= effectsDisplayTime)
        {
            DisableEffects ();
        }

    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
        for (int i = 0; i < shotgunLines.Length; i++){
            shotgunLines[i].enabled = false;
        }
    }


    public void Shoot (string gun)
    {
        timer = 0f;

        if (gun == "default"){
            ARShoot();
        }
        else if (gun == "shotgun"){
            ShotgunShoot();
        }
        else if (gun == "sniper"){
            SniperShoot();
        }

        Debug.Log(gunLight.color);

    }

    void ARShoot() {
        ARAudio.Play();
        ARTimer = 0f;
        gunLine.material = ARMat;
        gunLine.widthMultiplier = 1f;
        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);
        gunLight.color = ARLightColor;
        gunLight.enabled = true;
        gunParticles.Stop ();
        main.startColor = ARLightColor;
        gunParticles.Play ();

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerARShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }

    void ShotgunShoot() {
        ShotgunAudio.Play();
        gunManager.shotgunBullets -= 1;
        shotgunTimer = 0f;
        gunLight.color = ShotgunLightColor;
        gunLight.enabled = true;
        gunParticles.Stop ();
        main.startColor = ShotgunLightColor;
        gunParticles.Play ();

        for (int i = 0; i < shotgunLines.Length; i++){
            shotgunLines[i].enabled = true;
            shotgunLines[i].SetPosition (0, transform.position);
            shotgunRays[i].origin = transform.position;
            
            shotgunRays[i].direction = Quaternion.AngleAxis(-20 + 5*i, Vector3.up) * transform.forward;

            if(Physics.Raycast (shotgunRays[i], out shotgunHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shotgunHit.collider.GetComponent <EnemyHealth> ();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage (damagePerShotgunShot, shotgunHit.point);
                }
                shotgunLines[i].SetPosition (1, shotgunHit.point);
            }
            else
            {
                shotgunLines[i].SetPosition (1, shotgunRays[i].origin + shotgunRays[i].direction * range);
            }
        }
    }

    void SniperShoot() {
        SniperAudio.Play();
        gunLine.material = SniperMat;
        gunManager.sniperBullets -= 1;
        sniperTimer = 0f;
        gunLine.widthMultiplier = 5f;
        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);
        gunLight.color = SniperLightColor;
        gunLight.enabled = true;
        gunParticles.Stop ();
        main.startColor = SniperLightColor;
        gunParticles.Play ();

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        sniperHit = Physics.RaycastAll(shootRay, range, shootableMask);

        if(sniperHit.Length > 0)
        {
            for (int i = 0; i < sniperHit.Length; i++){
                EnemyHealth enemyHealth = sniperHit[i].collider.GetComponent <EnemyHealth> ();
                if(enemyHealth != null)
                {
                    enemyHealth.TakeDamage (damagePerSniperShot, sniperHit[i].point);
                }
            }
        }
        gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        
    }

}
