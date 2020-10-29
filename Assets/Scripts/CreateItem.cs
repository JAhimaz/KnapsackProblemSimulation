using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class CreateItem : MonoBehaviour {

    [SerializeField]
    private List<GameObject> itemPrefabs;
    public InputField itemWeight;
    public InputField itemValue;

    public int numberOfItems = 0;

    //public List<GameObject> createdObjects = new List<GameObject>();
    [SerializeField]
    public List<Transform> spawnLocations;
    public Vector3[] originalSpawnLocations;
    [SerializeField]
    private int maxWeight = 15000;
    [SerializeField]
    private int spawnNumber = 0;

    public GameObject informationPanel;

    public void Start() {
        originalSpawnLocations = new Vector3[spawnLocations.Count];
        for (int i = 0; i < spawnLocations.Count; i++) {
            originalSpawnLocations[i] = spawnLocations[i].transform.position;
        }
    }

    public void CreateObject() {
        //Requires Items (Usually Done In The Background, Not relevant to the Algorithm
        if (itemPrefabs != null && !string.IsNullOrWhiteSpace(itemWeight.text) && !string.IsNullOrWhiteSpace(itemValue.text) && int.Parse(itemValue.text) > 0 && int.Parse(itemWeight.text) > 0) {
            if(int.Parse(itemWeight.text) <= maxWeight) {

                string itemName = "Item " + (numberOfItems + 1);

                //Instantiate Item
                GameObject item = (GameObject)Instantiate(itemPrefabs[0], spawnLocations[spawnNumber].transform.position, Quaternion.identity);
                item.GetComponent<Item>().Initialise(itemName, int.Parse(itemValue.text), int.Parse(itemWeight.text));
                item.GetComponent<Item>().canvas = this.gameObject;
                item.GetComponent<Item>().informationPanel = informationPanel;
                item.GetComponentInChildren<TextMeshPro>().SetText("<color=green>" + itemName + "</color>\n Value: {0} \n Weight: {1}", int.Parse(itemValue.text), int.Parse(itemWeight.text));
                GetComponent<Solve>().items.Add(item);

                if (GetComponent<ShowAllHolograms>().displayItemInfo.isOn) {
                    item.GetComponent<Item>().hologram.GetComponent<Renderer>().enabled = true;
                }
                else {
                    item.GetComponent<Item>().hologram.GetComponent<Renderer>().enabled = false;
                }

                //Handles Spawning
                if (spawnNumber < spawnLocations.Count - 1) {
                    spawnNumber += 1;
                }
                else{
                    spawnNumber = 0;
                    for(int i = 0; i < spawnLocations.Count; i++) {
                        spawnLocations[i].Translate(new Vector3(0,0.25f,0));
                    }
                    
                }

                //Reset Fields
                itemValue.text = "";
                itemWeight.text = "";

                numberOfItems += 1;

            }else {
                Debug.Log("The Maximum Weight Per Item Is: " + maxWeight + "KG");
            }   
        }else {
            if (itemPrefabs == null) {
                Debug.Log("Internal Error 002");
            }
            if (string.IsNullOrWhiteSpace(itemWeight.text)) {
                Debug.Log("Please Enter A Item Weight For Your Item");
            }
            if (string.IsNullOrWhiteSpace(itemValue.text)) {
                Debug.Log("Please Enter A Item Value For Your Item");
            }
            if (int.Parse(itemValue.text) <= 0) {
                Debug.Log("Positive Values Only");
                itemValue.text = "";
            }
            if(int.Parse(itemWeight.text) <= 0) {
                Debug.Log("Positive Values Only");
                itemWeight.text = "";
            }
        }
    }

    public void RandomlyGenerate() {
        if(GetComponent<Solve>().knapsack == null) {
            GetComponent<GenerateKnapsack>().knapSackCapacity.text = Random.Range(10, 100).ToString();
            GetComponent<GenerateKnapsack>().CreateObject();
        }

        if (GetComponent<Solve>().itemsInChest != null || GetComponent<Solve>().items != null) {
            GetComponent<Solve>().deleteAllItems();
        }

        int maxItems = Random.Range(3, 10);

        for (int i = 0; i <= maxItems; i++) {
            itemWeight.text = Random.Range(1, GetComponent<Solve>().knapsack.GetComponent<KnapSack>().capacity).ToString();
            itemValue.text = Random.Range(1, 30).ToString();
            CreateObject();
        }
    }
}
