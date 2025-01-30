using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PublicEventManager
{
    public static Action<RotoManager.RotoInstructions> TestingCheckpointOne;
    public static Action<RotoManager.RotoInstructions> TestingCheckpointTwo;
    public static Action<RotoManager.RotoInstructions> TestingCheckpointThree;
}
