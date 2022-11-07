using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private bool state = false;

    [SerializeField]
    private char character = ' ';

    [SerializeField]
    private Material matOff;

    [SerializeField]
    private Material matOn;

    [SerializeField]
    private bool isNumber;

    public bool State
    {
        get { return state; }
        set { 
            if (value == state) return;
            state = value;

            if (state)  // Set material to off state
            {
                gameObject.GetComponent<Renderer>().material = matOn;
            } else // Set material to on state
            {
                gameObject.GetComponent<Renderer>().material = matOff;
            }
        }
    }

    public char Character
    {
        get { return character; }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
