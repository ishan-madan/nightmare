using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenManager : MonoBehaviour
{
    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    public void PlayButton() {
        anim.SetTrigger("Play");
    }

    public void ControlsButton() {
        anim.SetTrigger("ShowControl");
    }

    public void HomeButton() {
        anim.SetTrigger("HideControl");
    }

    public void SendToLevel01() {
        SceneManager.LoadScene("Level01");
    }
}
