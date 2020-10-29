using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Solve : MonoBehaviour{

    public GameObject knapsack;
    public List<GameObject> items;

    public List<GameObject> itemsInChest;

    public Camera mainCamera;

    public Button solveButton;
    public Button emptyChestButton;

    public Transform itemChestSpawn;
    public Vector3 originalPosition;

    [SerializeField]
    private List<Transform> spawnLocations;

    private int filledCapacity = 0, totalValue = 0;

    public Transform initialCameraPoint;


    public void Start() {
        originalPosition = itemChestSpawn.transform.position;
    }

    public void SolveKnapsackProblem() {
        //We first check if there is even a knapsack generated
        if(knapsack != null) {
            //If there are items in the chest beforehand it will reset
            if(itemsInChest != null) {
                ResetChest();
            }

            //Dynamic Programming Algorithm
            //We get the max capacity of the knapsack as well as the number of items
            int knapsackCapacity = knapsack.GetComponent<KnapSack>().capacity; 
            int NB_ITEMS = items.Count;

            // We use a matrix to store the max value at each n-th item
            int[,] matrix = new int[NB_ITEMS + 1, knapsackCapacity + 1];

            // First line is initialized to 0
            for (int i = 0; i <= knapsackCapacity; i++)
                matrix[0,i] = 0;

            // We iterate on items
            for (int i = 1; i <= NB_ITEMS; i++) {
                // we iterate on each capacity
                for (int j = 0; j <= knapsackCapacity; j++) {
                    if(items[i-1].GetComponent<Item>().itemWeight > j) {
                        matrix[i, j] = matrix[i - 1, j];
                    }else {
                        // we maximize value at this rank in the matrix
                        matrix[i, j] = Mathf.Max(matrix[i - 1, j], matrix[i - 1, j - items[i - 1].GetComponent<Item>().itemWeight] + items[i - 1].GetComponent<Item>().itemValue);
                    }
                }
            }

            //At the end of our Dynamic Programming Algorithm, the max value can be
            //found in the matrix at the cell matrix[NB_ITEMS][capacity].

            //Next step is to find the items to include in the bag to maximize the value. 
            //Either the result comes from the top matrix[i-1][capacity] cell or from values[i-1] + weights[i-1][capacity-weights[i-1]] 
            //as in the matrix built previously.

            //It it comes latter one, it means the item is included in the bag to maximize the value. It gives us the following code to find the items to include :
            int res = matrix[NB_ITEMS, knapsackCapacity];
            int w = knapsackCapacity;

            for (int i = NB_ITEMS; i > 0 && res > 0; i--) {
                if (res != matrix[i - 1, w]) {
                    itemsInChest.Add(items[i - 1]);
                    //We remove items value and weight
                    res -= items[i - 1].GetComponent<Item>().itemValue;
                    w -= items[i - 1].GetComponent<Item>().itemWeight;
                    items.Remove(items[i - 1]);
                }
            }

            //Non related to the algorithm but more related to Unity, here we move the items from the usual place
            //Into the chest and update the values of each.
            for (int i = 0; i < itemsInChest.Count; i++) {
                filledCapacity += itemsInChest[i].GetComponent<Item>().itemWeight;
                totalValue += itemsInChest[i].GetComponent<Item>().itemValue;
                itemChestSpawn.transform.Translate(new Vector3(0, 1, 0));
                itemsInChest[i].transform.position = itemChestSpawn.transform.position;
            }

            if (knapsack.GetComponent<KnapSack>().capacity - filledCapacity <= 5) {
                knapsack.GetComponentInChildren<TextMeshPro>().SetText("<color=green>SOLVED!</color>\nChest Capacity: <color=red>{0}KG</color> / {1}KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);
            }
            else if (knapsack.GetComponent<KnapSack>().capacity - filledCapacity <= 10) {
                knapsack.GetComponentInChildren<TextMeshPro>().SetText("<color=green>SOLVED!</color>\nChest Capacity: <color=yellow>{0}KG</color> / {1}KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);
            }
            else {
                knapsack.GetComponentInChildren<TextMeshPro>().SetText("<color=green>SOLVED!</color>\nChest Capacity: <color=green>{0}KG</color> / {1}KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);
            }

        }
    }

    private void updateChest() {
        filledCapacity = 0;
        totalValue = 0;

        for (int i = 0; i < itemsInChest.Count; i++) {
            filledCapacity += itemsInChest[i].GetComponent<Item>().itemWeight;
            totalValue += itemsInChest[i].GetComponent<Item>().itemValue;
        }

        if (knapsack.GetComponent<KnapSack>().capacity - filledCapacity <= 5) {
            knapsack.GetComponentInChildren<TextMeshPro>().SetText("Chest Capacity: <color=red>{0}KG</color> / {1}KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);
        }
        else if (knapsack.GetComponent<KnapSack>().capacity - filledCapacity <= 10) {
            knapsack.GetComponentInChildren<TextMeshPro>().SetText("Chest Capacity: <color=yellow>{0}KG</color> / {1}KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);
        }
        else {
            knapsack.GetComponentInChildren<TextMeshPro>().SetText("Chest Capacity: <color=green>{0}KG</color> / {1}KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);
        }
    }

    public void removeItem(string name) {
        for (int i=0; i < items.Count; i++) {
            if (items[i].GetComponent<Item>().itemName.Equals(name)) {
                Destroy(items[i]);
                items.RemoveAt(i);
            }
        }

        for (int i = 0; i < itemsInChest.Count; i++) {
            if (itemsInChest[i].GetComponent<Item>().itemName.Equals(name)) {
                Destroy(itemsInChest[i]);
                itemsInChest.RemoveAt(i);
                updateChest();
            }
        }
    }

    public void ResetChest() {
        filledCapacity = 0;
        totalValue = 0;

        itemChestSpawn.transform.position = originalPosition;

        int spawnNumber = 0;

        for (int i = 0; i < spawnLocations.Count; i++) {
            spawnLocations[i].transform.position = GetComponentInParent<CreateItem>().originalSpawnLocations[i];
        }

        for (int i = 0; i < itemsInChest.Count; i++) {
            items.Add(itemsInChest[i]);
            itemsInChest[i].transform.position = spawnLocations[spawnNumber].transform.position;
            if (spawnNumber < spawnLocations.Count - 1) {
                spawnNumber += 1;
            }
            else {
                spawnNumber = 0;
                for (int a = 0; a < spawnLocations.Count; a++) {
                    spawnLocations[spawnNumber].Translate(new Vector3(0, 0.25f, 0));
                }
            }
        }

        itemsInChest.Clear();
        knapsack.GetComponentInChildren<TextMeshPro>().SetText("Chest Capacity: <color=green>{0}KG</color> / <color=green>{1}</color>KG\nTotal Value: <color=green>{2}</color>", filledCapacity, knapsack.GetComponent<KnapSack>().capacity, totalValue);

/*        solveButton.interactable = true;
        emptyChestButton.interactable = false;*/
    }

    public void deleteAllItems() {
        if (itemsInChest != null) {
            ResetChest();
        }

        if (items != null) {
            for (int i = 0; i < items.Count; i++) {
                Destroy(items[i]);
            }
            items.Clear();
        }

        filledCapacity = 0;
        totalValue = 0;

        for (int i = 0; i < spawnLocations.Count; i++) {
            spawnLocations[i].transform.position = GetComponentInParent<CreateItem>().originalSpawnLocations[i];
        }

        itemChestSpawn.transform.position = originalPosition;
        GetComponentInParent<CreateItem>().numberOfItems = 0;

    }

    public void ResetAll() {
        filledCapacity = 0;
        totalValue = 0;

        itemChestSpawn.transform.position = originalPosition;

        if (items != null) {
            for(int i = 0; i < items.Count; i++) {
                Destroy(items[i]);
            }
            items.Clear();
        }

        if (itemsInChest != null) {
            for (int i = 0; i < itemsInChest.Count; i++) {
                Destroy(itemsInChest[i]);
            }
            itemsInChest.Clear();
        }

        for (int i = 0; i < spawnLocations.Count; i++) {
            spawnLocations[i].transform.position = GetComponentInParent<CreateItem>().originalSpawnLocations[i];
        }

        GetComponentInParent<CreateItem>().numberOfItems = 0;

        Destroy(knapsack);

/*        solveButton.interactable = true;
        emptyChestButton.interactable = false;*/

        mainCamera.GetComponent<MouseOrbitImproved>().target = initialCameraPoint;

    }
}
