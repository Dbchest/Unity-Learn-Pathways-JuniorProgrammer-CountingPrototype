using UnityEngine;

public class EventFlash : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (Sequence.Instance.IsPendingAnimation)
        {
            if (Sequence.Instance.Phase == "Demonstration")
            {
                Sequence.Instance.IsPendingAnimation = false;
            }

            else if (Sequence.Instance.Phase == "Registration")
            {
                if (!EventsCollection.Instance.IsAnyEventFlashing())
                {
                    Sequence.Instance.IsPendingAnimation = false;
                }
            }
        }
    }
}