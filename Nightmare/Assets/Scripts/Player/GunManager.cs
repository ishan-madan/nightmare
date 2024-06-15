using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    PlayerShooting playerShooting;
    string gunType = "AR";

    public Slider ARSlider;
    public Slider shotgunSlider;
    public Slider sniperSlider;
    public Image ARHighlight;
    public Image shotgunHighlight;
    public Image sniperHighlight;
    public RectTransform ARRectTransform;
    public RectTransform shotgunRectTransform;
    public RectTransform sniperRectTransform;
    public Text shotgunBulletText;
    public Text sniperBulletText;
    public GameObject ShotgunBulletCluster;
    public GameObject SniperBulletCluster;
    
    float reloadTimerValue;
    [HideInInspector]
    public int shotgunBullets = 15;
    [HideInInspector]
    public int sniperBullets = 10;

    void Start() {
        playerShooting = FindObjectOfType<PlayerShooting>();
        shotgunBullets = 15;
        sniperBullets = 10;
    }

    void Update() {
        if (Input.GetButton ("Fire1")){
            shootGun();
        }

        if (Input.GetKeyDown("1")){
            gunType = "AR";
            ARHighlight.enabled = true;
            shotgunHighlight.enabled = false;
            sniperHighlight.enabled = false;
            UpdateHighlights();
        }

        if (Input.GetKeyDown("2")){
            gunType = "shotgun";
            ARHighlight.enabled = false;
            shotgunHighlight.enabled = true;
            sniperHighlight.enabled = false;
            UpdateHighlights();
        }

        if (Input.GetKeyDown("3")){
            gunType = "sniper";
            ARHighlight.enabled = false;
            shotgunHighlight.enabled = false;
            sniperHighlight.enabled = true;
            UpdateHighlights();
        }

        shotgunBulletText.text = shotgunBullets + "/15";
        sniperBulletText.text = sniperBullets + "/10";

        if (shotgunBullets <= 0){
            shotgunBulletText.color = Color.red;
        }
        else {
            shotgunBulletText.color = Color.white;
        }

        if (sniperBullets <= 0){
            sniperBulletText.color = Color.red;
        }
        else {
            sniperBulletText.color = Color.white;
        }

        UpdateReloadTimer();
    }


    void shootGun() {
        if (gunType == "AR"){
            if(playerShooting.ARTimer >= playerShooting.timeBetweenARBullets && Time.timeScale != 0)
            {
                playerShooting.Shoot("default");
            }
        }
        else if (gunType == "shotgun"){
            if(playerShooting.shotgunTimer >= playerShooting.timeBetweenShotgunBullets && shotgunBullets > 0 && Time.timeScale != 0)
            {
                playerShooting.Shoot("shotgun");
            }
            else if(shotgunBullets <= 0){
                if(!playerShooting.gunMisfire.isPlaying){
                    playerShooting.gunMisfire.Play();
                }
            }
        }
        else if (gunType == "sniper"){
            if(playerShooting.sniperTimer >= playerShooting.timeBetweenSniperBullets && sniperBullets > 0 && Time.timeScale != 0)
            {
                playerShooting.Shoot("sniper");
            }
            else if(sniperBullets <= 0){
                if(!playerShooting.gunMisfire.isPlaying){
                    playerShooting.gunMisfire.Play();
                }
            }
        }
    }

    void UpdateReloadTimer() {
        ARSlider.value = FindReloadTimerValue("AR");
        shotgunSlider.value = FindReloadTimerValue("shotgun");
        sniperSlider.value = FindReloadTimerValue("sniper");
    }

    float FindReloadTimerValue(string gun){
        if (gun == "AR"){
            reloadTimerValue = playerShooting.ARTimer / playerShooting.timeBetweenARBullets;
        }
        else if (gun == "shotgun") {
            reloadTimerValue = playerShooting.shotgunTimer / playerShooting.timeBetweenShotgunBullets;
        }
        else if (gun == "sniper") {
            reloadTimerValue = playerShooting.sniperTimer / playerShooting.timeBetweenSniperBullets;
        }

        if (reloadTimerValue > 1){
            reloadTimerValue = 1;
        }
        
        return reloadTimerValue;
    } 


    void UpdateHighlights() {
        if (gunType == "AR"){
            StartCoroutine(MoveHighlights(ARRectTransform));
        }
        else if (gunType == "shotgun"){
            StartCoroutine(MoveHighlights(shotgunRectTransform));
        }
        else if (gunType == "sniper"){
            StartCoroutine(MoveHighlights(sniperRectTransform));
        }
    }

    IEnumerator MoveHighlights(RectTransform rt)
    {
        float step = 35;
        while (step > 0)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 26 - step);
            step -= 1;
            yield return new WaitForEndOfFrame();
        }
    }

    public void SpawnSniperBullets(Transform pos){
        GameObject bulletCluster = Instantiate(SniperBulletCluster, pos);
        bulletCluster.transform.parent = null;
        bulletCluster.transform.position = pos.position;
    }

    public void SpawnShotgunBullets(Transform pos){
        GameObject bulletCluster = Instantiate(ShotgunBulletCluster, pos);
        bulletCluster.transform.parent = null;
        bulletCluster.transform.position = pos.position;
    }





}
