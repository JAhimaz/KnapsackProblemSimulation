using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiscButtons : MonoBehaviour{

    public GameObject panel;

    public GameObject mainCamera;
    public void ExitApplication() {
        Application.Quit();
    }

    public void ShowInformation() {
        panel.SetActive(true);
        mainCamera.GetComponent<MouseOrbitImproved>().enabled = false;
    }

    public void HideInformation() {
        panel.SetActive(false);
        mainCamera.GetComponent<MouseOrbitImproved>().enabled = true;
    }
}
