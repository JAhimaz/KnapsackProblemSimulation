using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerateKnapsack : MonoBehaviour{
    public GameObject knapSackPrefab;
    public InputField knapSackCapacity;

    public Camera mainCamera;

    [SerializeField]
    private Transform spawnLocation;
    [SerializeField]
    private int maxCapacity = 15000;

    public void CreateObject(){
        //Requires a knapsack Prefab or it won't instantiate
        if (knapSackPrefab != null && !string.IsNullOrWhiteSpace(knapSackCapacity.text) && int.Parse(knapSackCapacity.text) > 0) {
            if(int.Parse(knapSackCapacity.text) <= maxCapacity) {
                //Setup Knapsack Details
                Debug.Log(int.Parse(knapSackCapacity.text));

                // get a random postion to instantiate the prefab - you can change this to be created at a fied point if desired
                if (GetComponent<Solve>().knapsack != null) {
                    Destroy(GetComponent<Solve>().knapsack);
                }

                // instantiate the object
                GameObject chest = (GameObject)Instantiate(knapSackPrefab, spawnLocation.localPosition, knapSackPrefab.transform.rotation);
                GetComponent<Solve>().knapsack = chest;

                GetComponent<Solve>().knapsack.GetComponent<KnapSack>().capacity = int.Parse(knapSackCapacity.text);

                GetComponent<Solve>().knapsack.GetComponentInChildren<TextMeshPro>().SetText("Chest Capacity: <color=red>0KG</color> / {0}KG\nTotal Value: <color=green>0</color>", GetComponent<Solve>().knapsack.GetComponent<KnapSack>().capacity);

                mainCamera.GetComponent<MouseOrbitImproved>().target = GetComponent<Solve>().knapsack.transform;

                knapSackCapacity.text = "";
            }
            else {
                Debug.Log("The Maximum Possible Capacity Is " + maxCapacity + "KG");
            }   
        }
        else{
            if(knapSackPrefab == null) {
                Debug.Log("Internal Error 001");
            }
            if (string.IsNullOrWhiteSpace(knapSackCapacity.text)) {
                Debug.Log("Please Enter A Maximum Capacity For Your Knapsack");
            }
            if (int.Parse(knapSackCapacity.text) <= 0) {
                Debug.Log("Positive Values Only");
                knapSackCapacity.text = "";
            }
        }
    }

}
