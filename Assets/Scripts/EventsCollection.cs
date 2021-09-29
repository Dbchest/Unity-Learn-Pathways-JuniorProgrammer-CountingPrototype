using System.Collections.Generic;
using UnityEngine;

public class EventsCollection : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private static EventsCollection instance;

    public static EventsCollection Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogWarning("EventsCollection is null.");
            }
            return instance;
        }
    }

    internal List<Event> EventsList { get; private set; }

    public bool IsAnyEventFlashing()
    {
        foreach (var e in EventsList)
        {
            if (e.IsFlashing())
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        instance = this;
        InitializeEventsList();
        InstantiateEvents();
    }

    private void InitializeEventsList()
    {
        if (EventsList == null)
        {
            EventsList = new List<Event>();
        }
    }

    private void InstantiateEvents()
    {
        for (int i = 0; i < Sequence.Instance.EventRangeMax; i++)
        {
            GameObject instance = Instantiate(prefab, transform);
            EventsList.Add(instance.GetComponent<Event>());
            EventsList[EventsList.Count - 1].SetIndex(i);
        }
        EventsList[EventsList.Count - 1].HideDivider();
    }

    private void Start()
    {
        Sequence.Instance.Invoke();
    }
}