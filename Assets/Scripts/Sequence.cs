using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoSingleton<Sequence>
{
    [SerializeField]
    private int initialEventCount = 3;

    [SerializeField]
    private int delay = 3;

    public int Index { get; private set; }

    public string Phase { get; private set; }

    public int Difficulty { get; internal set; }

    public int Count
    {
        get => Index + 1;
    }

    public int EventCount
    {
        get => initialEventCount + Index;
    }

    public int EventRangeMax
    {
        get => Difficulty + 1;
    }

    public int LatestEvent
    {
        get => EventsList[EventsList.Count - 1];
    }

    public int LatestRegistration
    {
        get => RegistrationsList[RegistrationsList.Count - 1];
    }

    internal bool IsPendingAnimation { get; set; }

    internal bool IsPendingRegistration { get; set; }

    internal bool IsMatchingRegistrations { get; private set; }

    internal Stack<int> EventsStack { get; private set; }

    internal List<int> EventsList { get; private set; }

    internal List<int> RegistrationsList { get; private set; }

    public static Action IndexChanged;

    public static Action Iterating;

    public static Action Demonstrating;

    public static Action Demonstrated;

    public static Action Registering;

    public static Action Evaluating;

    public static Action Evaluated;

    public static Action Registered;

    public static Action Iterated;

    internal void Invoke()
    {
        StartCoroutine(Iterate());
    }

    protected override void Initialize()
    {
        base.Initialize();

        Index = -1;
        Phase = string.Empty;

        IsPendingAnimation = false;
        IsPendingRegistration = false;
        IsMatchingRegistrations = false;

        InitializeEventsStack();
        InitializeEventsList();
        InitializeRegistrationsList();
    }

    private void InitializeEventsStack()
    {
        if (EventsStack == null)
        {
            EventsStack = new Stack<int>();
        }
    }

    private void InitializeEventsList()
    {
        if (EventsList == null)
        {
            EventsList = new List<int>();
        }
    }

    private void InitializeRegistrationsList()
    {
        if (RegistrationsList == null)
        {
            RegistrationsList = new List<int>();
        }
    }

    private IEnumerator Iterate()
    {
        Index = 0;
        IndexChanged?.Invoke();

        ////print("Iterating");
        Iterating?.Invoke();

        while (true)
        {
            Clear();
            Generate();

            yield return new WaitForSeconds(delay);

            yield return StartCoroutine(Demonstrate());
            yield return StartCoroutine(Register());

            if (!IsMatchingRegistrations)
            {
                break;
            }

            Increment();
        }
        Initialize();

        ////print("Iterated");
        Iterated?.Invoke();

        GameManager.Instance.LoadSceneAsync(0);
    }

    private void Clear()
    {
        EventsStack.Clear();
        EventsList.Clear();
        RegistrationsList.Clear();
    }

    private void Generate()
    {
        for (int i = 0; i < EventCount; i++)
        {
            EventsStack.Push(UnityEngine.Random.Range(0, EventRangeMax));
        }
    }

    private IEnumerator Demonstrate()
    {
        Phase = "Demonstration";

        while (EventsStack.Count > 0)
        {
            EventsList.Add(EventsStack.Pop());

            ////print("Demonstrating");
            Demonstrating?.Invoke();

            IsPendingAnimation = true;
            while (IsPendingAnimation)
            {
                // breaks via animator state machine behaviour
                yield return null;
            }
        }

        ////print("Demonstrated");
        Demonstrated?.Invoke();
    }

    private IEnumerator Register()
    {
        Phase = "Registration";

        ////print("Registering");
        Registering?.Invoke();

        while (RegistrationsList.Count < EventsList.Count)
        {
            IsPendingRegistration = true;
            while (IsPendingRegistration)
            {
                // breaks via interaction with user interface
                yield return null;
            }

            Evaluate();

            if (!IsMatchingRegistrations)
            {
                break;
            }
        }

        IsPendingAnimation = true;
        while (IsPendingAnimation)
        {
            // breaks via animator state machine behaviour
            yield return null;
        }

        ////print("Registered");
        Registered?.Invoke();
    }

    private void Evaluate()
    {
        ////print("Evaluating");
        Evaluating?.Invoke();

        int i = RegistrationsList.Count - 1;

        int a = EventsList[i];
        int b = RegistrationsList[i];

        IsMatchingRegistrations = a == b;

        ////print("Evaluated");
        Evaluated?.Invoke();
    }

    private void Increment()
    {
        Index++;
        IndexChanged?.Invoke();
    }
}