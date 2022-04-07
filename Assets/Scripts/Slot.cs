using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    [SerializeField] private int id;

    public Item item { get; set; }

    UnityEvent<int> clickEvent;

    void Start()
    {
        clickEvent = new UnityEvent<int>();
    }

    public void SetVisible(bool visible)
    {
        gameObject.transform.GetChild(0).GetComponent<Image>().enabled = visible;
        gameObject.transform.GetChild(1).GetComponent<Text>().enabled = visible;
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
