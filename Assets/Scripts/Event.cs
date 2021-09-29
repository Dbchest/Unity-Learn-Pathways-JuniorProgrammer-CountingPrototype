using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    [SerializeField]
    private GameObject divider;

    private Animator animator;

    private Button button;

    public int Index { get; private set; }

    public void Register()
    {
        Sequence.Instance.RegistrationsList.Add(Index);
        Sequence.Instance.IsPendingRegistration = false;
    }

    public bool IsFlashing()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        return !info.IsName("Idle");
    }

    internal void SetIndex(int index)
    {
        Index = index;
    }

    internal void HideDivider()
    {
        divider.SetActive(false);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        Sequence.Demonstrating += TriggerFlash;
        Sequence.Registering += EnableInteractions;
        Sequence.Evaluated += TriggerFlash;
        Sequence.Evaluated += DisableInteractionsIfRegistrationDoesNotMatch;
        Sequence.Registered += DisableInteractions;
    }

    private void Start()
    {
        int speedMultiplier = Sequence.Instance.Difficulty;
        animator.SetFloat("Speed Multiplier", speedMultiplier);
    }

    private void TriggerFlash()
    {
        string phase = Sequence.Instance.Phase;
        if (phase == "Demonstration")
        {
            int latestEvent = Sequence.Instance.LatestEvent;
            if (latestEvent == Index)
            {
                TriggerFlashForDemonstration();
            }
        }
        else if (phase == "Registration")
        {
            int latestRegistration = Sequence.Instance.LatestRegistration;
            if (latestRegistration == Index)
            {
                TriggerFlashForRegistration();
            }
        }
    }

    private void TriggerFlashForDemonstration()
    {
        int blueColorIndex = 0;
        if (animator.GetInteger("Color Index") != blueColorIndex)
        {
            SetAnimatorColorIndex(blueColorIndex);
        }
        animator.SetTrigger("Flash");
    }

    private void TriggerFlashForRegistration()
    {
        int greenColorIndex = 1;
        int redColorIndex = 2;

        bool isMatch = Sequence.Instance.IsMatchingRegistrations;
        int colorIndex = isMatch ? greenColorIndex : redColorIndex;

        if (animator.GetInteger("Color Index") != colorIndex)
        {
            SetAnimatorColorIndex(colorIndex);
        }
        animator.SetTrigger("Flash");
    }

    private void SetAnimatorColorIndex(int colorIndex)
    {
        animator.SetInteger("Color Index", colorIndex);
    }

    private void EnableInteractions()
    {
        button.interactable = true;
    }

    private void DisableInteractions()
    {
        button.interactable = false;
    }

    private void DisableInteractionsIfRegistrationDoesNotMatch()
    {
        if (!Sequence.Instance.IsMatchingRegistrations)
        {
            DisableInteractions();
        }
    }

    private void OnDisable()
    {
        Sequence.Demonstrating -= TriggerFlash;
        Sequence.Registering -= EnableInteractions;
        Sequence.Evaluated -= TriggerFlash;
        Sequence.Evaluated -= DisableInteractionsIfRegistrationDoesNotMatch;
        Sequence.Registered -= DisableInteractions;
    }
}