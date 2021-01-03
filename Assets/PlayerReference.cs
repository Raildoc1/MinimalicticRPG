using KG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{

    public Transform Player;

    public static StateSwitch stateSwitch;

    private void Awake()
    {
        stateSwitch = Player.GetComponent<StateSwitch>();
    }

}
