using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = transform.parent.GetComponentInParent<Player>();
    }

    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        if (isAnimationTransition())
        {
            return;
        }
        player.OnMovementStateAnimationEnterEvent();
    }
    public void TriggerOnMovementStateAnimationExitEvent()
    {
        if (isAnimationTransition())
        {
            return;
        }
        player.OnMovementStateAnimationExitEvent();
    }
    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        if (isAnimationTransition())
        {
            return;
        }
        player.OnMovementStateAnimationTransitionEvent();
    }
    private bool isAnimationTransition(int layerIndex = 0)
    {
        return player.animator.IsInTransition(layerIndex);
    }
}
