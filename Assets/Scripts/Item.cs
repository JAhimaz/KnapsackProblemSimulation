using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public int itemValue, itemWeight;

    public GameObject informationPanel;

    public GameObject canvas;

    Component halo;

    public GameObject hologram, Xhologram;

    public void Start() {
        Xhologram.GetComponent<Renderer>().enabled = false;
        halo = GetComponent("Halo");

        Xhologram.GetComponent<TextMeshPro>().isOverlay = true;

        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
    }

    public void Initialise(string name, int value, int weight) {
        itemName = name;
        itemValue = value;
        itemWeight = weight;
    }

    private void OnMouseOver() {
        if (!informationPanel.activeSelf) {
            if (!canvas.GetComponent<ShowAllHolograms>().displayItemInfo.isOn) {
                hologram.GetComponent<Renderer>().enabled = true;
            }

            Xhologram.GetComponent<Renderer>().enabled = true;
            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            if (Input.GetMouseButtonDown(0)) {
                canvas.GetComponent<Solve>().removeItem(itemName);
            }
        }
    }

    private void OnMouseExit() {
        if (!canvas.GetComponent<ShowAllHolograms>().displayItemInfo.isOn) {
            hologram.GetComponent<Renderer>().enabled = false;
        }
        Xhologram.GetComponent<Renderer>().enabled = false;
        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
    }

    //Setters
    public void setItemName(string name) {
        itemName = name;
    }

    public void setItemValue(int value) {
        itemValue = value;
    }

    public void setItemWeight(int weight) {
        itemWeight = weight;
    }

    //Getters

    public string getItemName() {
        return itemName;
    }

    public int getItemValue() {
        return itemValue;
    }
    public int getItemWeight() {
        return itemWeight;
    }
}
