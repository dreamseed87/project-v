using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerController : NormalSingleton<PlayerController>
{
    [field: SerializeField] public PlayerDInfo P_DInfo { get; set; }
    [field: SerializeField] public PlayerSInfo P_SInfo { get; set; }

    [field: SerializeField] private DynamicMoveProvider moveProvider { get; set; } = null;
    [field: SerializeField] private ActionBasedContinuousTurnProvider turnProvider { get; set; } = null;

    public bool CanMove { get; private set; }
    public bool CanTurn { get; private set; }

    private void Start() {
        P_DInfo = new PlayerDInfo();
        P_SInfo = new PlayerSInfo();
    }

    // stop player
    public void StopPlayer() {
        moveProvider.moveSpeed = 0;
        turnProvider.turnSpeed = 0;
        CanMove = false;
        CanTurn = false;
    }

    public void StopOnlyMove() {
        moveProvider.moveSpeed = 0;
        CanMove = false;
    }

    public void MovePlayer() {
        moveProvider.moveSpeed = 1;
        turnProvider.turnSpeed = 60;
        CanMove = true;
        CanTurn = true;
    }
}
