using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowAllHolograms : MonoBehaviour {
    // Start is called before the first frame update

    public Toggle displayItemInfo;

    public void DisplayItemInfo(){
        if (displayItemInfo.isOn){
            for (int i = 0; i < GetComponent<Solve>().itemsInChest.Count; i++) {
                GetComponent<Solve>().itemsInChest[i].GetComponent<Item>().hologram.GetComponent<Renderer>().enabled = true;
            }

            for (int i = 0; i < GetComponent<Solve>().items.Count; i++) {
                GetComponent<Solve>().items[i].GetComponent<Item>().hologram.GetComponent<Renderer>().enabled = true;
            }
        }

        if (!displayItemInfo.isOn) {
            for (int i = 0; i < GetComponent<Solve>().itemsInChest.Count; i++) {
                GetComponent<Solve>().itemsInChest[i].GetComponent<Item>().hologram.GetComponent<Renderer>().enabled = false;
            }

            for (int i = 0; i < GetComponent<Solve>().items.Count; i++) {
                GetComponent<Solve>().items[i].GetComponent<Item>().hologram.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update(){
        
    }
}
