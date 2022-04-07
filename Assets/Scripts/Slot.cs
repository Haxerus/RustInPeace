using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slot : MonoBehaviour
{

    [SerializeField] private int id;

    UnityEvent<int> clickEvent;

    // Start is called before the first frame update
    void Start()
    {
        clickEvent = new UnityEvent<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddClickListener(UnityAction<int> listener)
    {
        clickEvent.AddListener(listener);
    } 

    void OnMouseDown()
    {
        clickEvent.Invoke(id);
    }
}
