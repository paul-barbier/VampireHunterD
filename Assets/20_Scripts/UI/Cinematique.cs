using System.Collections;
using TMPro;
using UnityEngine;

#region DATA

[System.Serializable]
public class Speaker
{
    public string id;                 // Ex : "Player", "Ame", "Passeur", "Boss"
    public string displayName;        // Nom affiché à l’écran
    public GameObject portrait;       // Portrait ou GameObject à afficher
}

[System.Serializable]
public struct SubtitleLine
{
    public float duration;
    [TextArea] public string text;
    public string speakerId;          // Doit correspondre à Speaker.id
}

#endregion

public class Cinematique : MonoBehaviour
{
    [Header("Subtitles")]
    public SubtitleLine[] lines;
    public GameObject textbox;
    public TMP_Text subtitles;
    public TMP_Text speakerNameText;  // ← NOUVEAU : nom du speaker

    [Header("Speakers")]
    public Speaker[] speakers;

    private PlayerCharacter _playerCharacter;

    private void Start()
    {
        textbox.SetActive(false);
        ClearSpeakerName();
        DisableAllSpeakers();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        _playerCharacter = other.GetComponent<PlayerCharacter>();

        StartCoroutine(SubtitleCoroutine());

        // éviter relance
        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator SubtitleCoroutine()
    {
        textbox.SetActive(true);

        // --- Bloque le joueur ---
        if (_playerCharacter != null)
            _playerCharacter.EnterCinematicMode();

        foreach (SubtitleLine line in lines)
        {
            subtitles.text = line.text;

            DisableAllSpeakers();
            EnableSpeaker(line.speakerId);

            yield return new WaitForSeconds(line.duration);
        }

        // --- Fin ---
        textbox.SetActive(false);
        ClearSpeakerName();
        DisableAllSpeakers();

        if (_playerCharacter != null)
            _playerCharacter.ExitCinematicMode();
    }

    // =========================
    // SPEAKERS
    // =========================

    private void DisableAllSpeakers()
    {
        foreach (Speaker speaker in speakers)
        {
            if (speaker.portrait != null)
                speaker.portrait.SetActive(false);
        }
    }

    private void EnableSpeaker(string id)
    {
        foreach (Speaker speaker in speakers)
        {
            if (speaker.id == id)
            {
                if (speaker.portrait != null)
                    speaker.portrait.SetActive(true);

                SetSpeakerName(speaker.displayName);
                return;
            }
        }

        Debug.LogWarning($"[Cinematique] Speaker '{id}' non trouvé.");
        ClearSpeakerName();
    }

    // =========================
    // SPEAKER NAME
    // =========================

    private void SetSpeakerName(string name)
    {
        if (speakerNameText != null)
            speakerNameText.text = name;
    }

    private void ClearSpeakerName()
    {
        if (speakerNameText != null)
            speakerNameText.text = "";
    }
}
