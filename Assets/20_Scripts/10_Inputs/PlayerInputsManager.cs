using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerInputsManager : MonoBehaviour
{
    private PlayerInputs _inputs = null;
    private PlayerCharacter _character = null;
    private Attack _attack = null;
    private PlayerTeleport _playerTP = null;
    private Dialogue _dialogue = null;
    private PlatformeOneWay _oneWay = null;

    private void Awake()
    {
        _character = GetComponent<PlayerCharacter>();
        _attack = GetComponent<Attack>();
        _playerTP = GetComponent<PlayerTeleport>();
        _dialogue = GetComponent<Dialogue>();
        _oneWay = GetComponent<PlatformeOneWay>();

        _inputs = new PlayerInputs();
        _inputs.Actions.Jump.started += _ => _character.StartJump();
        _inputs.Actions.Attack.started += _ => _attack.AttackZone();
        _inputs.Actions.Interact.started += _ => _playerTP.UseTP();
        _inputs.Actions.Dash.started += _ => _character.StartDash();
        _inputs.Actions.SkipDialogue.started += _ => _dialogue.SkipDialogue();
        _inputs.Actions.OneWayDown.started += _ => _oneWay.DownOneWay();
        _inputs.Enable();
    }

    private void FixedUpdate()
    {
        _character.GetMovementInput(_inputs.Actions.Move.ReadValue<float>());

        _character.GetDashInput(_inputs.Actions.DashDirection.ReadValue<Vector2>());
    }
}
