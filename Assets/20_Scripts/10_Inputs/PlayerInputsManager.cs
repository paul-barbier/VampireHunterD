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
    private UIDialogue _dialogueMenu = null;

    private void Awake()
    {
        _character = GetComponent<PlayerCharacter>();
        _attack = GetComponent<Attack>();
        _playerTP = GetComponent<PlayerTeleport>();
        _dialogue = FindFirstObjectByType<Dialogue>();
        _oneWay = GetComponent<PlatformeOneWay>();
        _dialogueMenu = FindFirstObjectByType<UIDialogue>();

        _inputs = new PlayerInputs();
        _inputs.Player.Jump.started += _ => _character.StartJump();
        _inputs.Player.Attack.started += _ => _attack.AttackZone();
        _inputs.Player.Interact.started += _ => _playerTP.UseTP();
        _inputs.Player.Dash.started += _ => _character.StartDash();
        _inputs.Player.OneWayDown.started += _ => _oneWay.DownOneWay();
        _inputs.Player.SkipDialogue.started += _ => _dialogue.SkipDialogue();
        _inputs.Menu.PauseMenu.started += _ => _dialogueMenu.Pause();
        _inputs.Enable();
    }

    private void FixedUpdate()
    {
        _character.GetMovementInput(_inputs.Player.Move.ReadValue<float>());

        _character.GetDashInput(_inputs.Player.DashDirection.ReadValue<Vector2>());
    }
}
