using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    [SerializeField] private int id;

    Item item;

    UnityEvent<int> clickEvent;

    UnityEvent<int, bool> mouseMoveEvent;

    void Start()
    {
        clickEvent = new UnityEvent<int>();
        mouseMoveEvent = new UnityEvent<int, bool>();
    }

    public void SetVisible(bool visible)
    {
        gameObject.transform.GetChild(0).GetComponent<Image>().enabled = visible;
        //gameObject.transform.GetChild(1).GetComponent<Text>().enabled = visible;
    }

    void RefreshUI()
    {
        if (item)
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
        else
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = null;
    }

    public void AddClickListener(UnityAction<int> listener)
    {
        clickEvent.AddListener(listener);
    }

    public void AddHoverListener(UnityAction<int, bool> listener)
    {
        mouseMoveEvent.AddListener(listener);
    }

    void OnMouseDown()
    {
        clickEvent.Invoke(id);
    }

    void OnMouseOver()
    {
        mouseMoveEvent.Invoke(id, true);
    }

    void OnMouseEnter()
    {
        mouseMoveEvent.Invoke(id, true);
    }

    void OnMouseExit()
    {
        mouseMoveEvent.Invoke(id, false);
    }

    public Item GetItem()
    {
        return item;
    }

    public void AddItem(Item i)
    {
        item = i;
        SetVisible(true);
        RefreshUI();
    }

    public void RemoveItem()
    {
        item = null;
        SetVisible(false);
        RefreshUI();
    }

    public bool IsEmpty()
    {
        return item == null;
    }
}
