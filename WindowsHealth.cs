using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowsHealth : MonoBehaviour
{
    [SerializeField] private GameObject[] windowStates;
    public int windowHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(windowHealth == 0){
            windowStates[0].SetActive(true);
            windowStates[1].SetActive(false);
        } else if (windowHealth == 1){
            windowStates[0].SetActive(false);
            windowStates[1].SetActive(true);
        }
    }
}
