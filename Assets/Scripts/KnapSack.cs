using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnapSack : MonoBehaviour{
    [SerializeField]
    List<GameObject> storedItems;
    [SerializeField]
    public int capacity, filledCapacity, totalValue;

    public TextMeshPro Hologram;

}
