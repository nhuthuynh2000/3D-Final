using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationsData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "moving";
    [SerializeField] private string stoppingParameterName = "stopping";
    [SerializeField] private string landingParameterName = "landing";
    [SerializeField] private string airborneParameterName = "Airborne";

    [Header("Grounded Parameter Names")]
    [SerializeField] private string idleParameterName = "isIdling";
    [SerializeField] private string dashParameterName = "isDashing";
    [SerializeField] private string walkParameterName = "isWalking";
    [SerializeField] private string runParameterName = "isRunning";
    [SerializeField] private string sprintParameterName = "isSprinting";
    [SerializeField] private string mediumStopParameterName = "isMediumStopping";
    [SerializeField] private string hardStopParameterName = "isHardStopping";
    [SerializeField] private string rollParameterName = "isRolling";
    [SerializeField] private string hardLandingParameterName = "isHardLanding";

    [Header("Airborne Parameter Names")]
    [SerializeField] private string fallParameterName = "isFalling";

    public int groundedParameterHash { get; private set; }
    public int movingParameterHash { get; private set; }
    public int landingParameterHash { get; private set; }
    public int stoppingParameterHash { get; private set; }
    public int airborneParameterHash { get; private set; }



    public int idleParameterHash { get; private set; }
    public int dashParameterHash { get; private set; }
    public int walkParameterHash { get; private set; }
    public int runParameterHash { get; private set; }
    public int sprintParameterHash { get; private set; }
    public int mediumStopParameterHash { get; private set; }
    public int hardStopParameterHash { get; private set; }
    public int rollParameterHash { get; private set; }
    public int hardLandingParameterHash { get; private set; }


    public int fallParameterHash { get; private set; }

    public void Initialize()
    {
        groundedParameterHash = Animator.StringToHash(groundedParameterName);
        movingParameterHash = Animator.StringToHash(movingParameterName);
        stoppingParameterHash = Animator.StringToHash(stoppingParameterName);
        landingParameterHash = Animator.StringToHash(landingParameterName);
        airborneParameterHash = Animator.StringToHash(airborneParameterName);

        idleParameterHash = Animator.StringToHash(idleParameterName);
        dashParameterHash = Animator.StringToHash(dashParameterName);
        walkParameterHash = Animator.StringToHash(walkParameterName);
        runParameterHash = Animator.StringToHash(runParameterName);
        sprintParameterHash = Animator.StringToHash(sprintParameterName);
        mediumStopParameterHash = Animator.StringToHash(mediumStopParameterName);
        hardStopParameterHash = Animator.StringToHash(hardStopParameterName);
        rollParameterHash = Animator.StringToHash(rollParameterName);
        hardLandingParameterHash = Animator.StringToHash(hardLandingParameterName);

        fallParameterHash = Animator.StringToHash(fallParameterName);
    }
}
