using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private MainMenu _mainMenu = null;
    private CinematiqueTuto _cinematiqueTuto = null;

    //private void Awake()
    //{
    //    _character = GetComponent<PlayerCharacter>();
    //    _attack = GetComponent<Attack>();
    //    _playerTP = GetComponent<PlayerTeleport>();
    //    _dialogue = FindFirstObjectByType<Dialogue>();
    //    _oneWay = GetComponent<PlatformeOneWay>();
    //    _dialogueMenu = FindFirstObjectByType<UIDialogue>();
    //    _mainMenu = GetComponent<MainMenu>();

    //    _inputs = new PlayerInputs();
    //    _inputs.Player.Jump.started += _ => _character.StartJump();
    //    _inputs.Player.Attack.started += _ => _attack.AttackZone();
    //    _inputs.Player.Interact.started += _ => _playerTP.UseTP();
    //    _inputs.Player.Dash.started += _ => _character.StartDash();
    //    _inputs.Player.OneWayDown.started += _ => _oneWay.DownOneWay();
    //    _inputs.Player.SkipDialogue.started += _ => _dialogue.SkipDialogue();
    //    _inputs.Menu.PauseMenu.started += _ => _dialogueMenu.Pause();
    //    _inputs.Menu.Back.started += _ => _dialogueMenu.Back();
    //    _inputs.Menu.BackMenu.started += _ => _mainMenu.BackMenu();
    //    _inputs.Enable();
    //}

    private void Awake()
    {
        _character = GetComponent<PlayerCharacter>();
        _attack = GetComponent<Attack>();
        _playerTP = GetComponent<PlayerTeleport>();
        _dialogue = FindFirstObjectByType<Dialogue>();
        _oneWay = GetComponent<PlatformeOneWay>();

        _dialogueMenu = FindFirstObjectByType<UIDialogue>();
        _mainMenu = GetComponent<MainMenu>();

        _cinematiqueTuto = FindFirstObjectByType<CinematiqueTuto>();


        _inputs = new PlayerInputs();

        _inputs.Player.Jump.started += OnJump;
        _inputs.Player.Attack.started += OnAttack;
        _inputs.Player.Interact.started += OnInteract;
        _inputs.Player.Dash.started += OnDash;
        _inputs.Player.OneWayDown.started += OnOneWayDown;
        _inputs.Player.SkipDialogue.started += OnSkipDialogue;

        _inputs.Menu.PauseMenu.started += OnPause;
        _inputs.Menu.Back.started += OnBack;
        _inputs.Menu.BackMenu.started += OnBackMenu;
        _inputs.Menu.SkipCinematique.started += OnCinematique;
    }

    private void FixedUpdate()
    {
        if (_character == null) return;
        _character.GetMovementInput(_inputs.Player.Move.ReadValue<float>());

        Vector2 dashDirection = _inputs.Player.DashDirection.ReadValue<Vector2>();
        if (dashDirection != Vector2.zero)
        {
            _character.GetDashInput(dashDirection);
        }
    }
    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (_character == null) return;
        _character.StartJump();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (_attack == null) return;
        _attack.AttackZone();
    }

    private void OnDash(InputAction.CallbackContext ctx)
    {
        if (_character == null) return;
        _character.StartDash();
    }
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_playerTP == null) return;
        _playerTP.UseTP();
    }

    private void OnOneWayDown(InputAction.CallbackContext ctx)
    {
        if (_oneWay == null) return;
        _oneWay.DownOneWay();
    }

    private void OnSkipDialogue(InputAction.CallbackContext ctx)
    {
        if (_dialogue == null) return;
        _dialogue.SkipDialogue();
    }
    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (_dialogueMenu == null) return;
        _dialogueMenu.Pause();
    }

    private void OnBack(InputAction.CallbackContext ctx)
    {
        if (_dialogueMenu == null) return;
        _dialogueMenu.Back();
    }

    private void OnBackMenu(InputAction.CallbackContext ctx)
    {
        if (_mainMenu == null) return;
        _mainMenu.BackMenu();
    }
    private void OnCinematique(InputAction.CallbackContext ctx)
    {
        if (_cinematiqueTuto == null) return;
        _cinematiqueTuto.SkipCinematique();
        Debug.Log("FEUUUUUUUUUR");
    }

    private void OnEnable()
    {
        _inputs?.Enable();
    }

    private void OnDisable()
    {
        if (_inputs == null) return;

        _inputs.Player.Jump.started -= OnJump;
        _inputs.Player.Attack.started -= OnAttack;
        _inputs.Player.Interact.started -= OnInteract;
        _inputs.Player.Dash.started -= OnDash;
        _inputs.Player.OneWayDown.started -= OnOneWayDown;
        _inputs.Player.SkipDialogue.started -= OnSkipDialogue;

        _inputs.Menu.PauseMenu.started -= OnPause;
        _inputs.Menu.Back.started -= OnBack;
        _inputs.Menu.BackMenu.started -= OnBackMenu;
        _inputs.Menu.SkipCinematique.started -= OnCinematique;

        _inputs.Disable();
        _inputs.Dispose();
    }
}
