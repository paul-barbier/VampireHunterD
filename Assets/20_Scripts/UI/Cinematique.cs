using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

#region DATA

[System.Serializable]
public class Speaker
{
    public string id;           // Ex : "Player", "Ame", "Passeur", "Boss"
    public GameObject portrait; // Portrait ou GameObject à afficher
}

[System.Serializable]
public struct SubtitleLine
{
    public float duration;
    [TextArea] public string text;
    public string speakerId; // Doit correspondre à Speaker.id
}

#endregion

public class Cinematique : MonoBehaviour
{
    [Header("Subtitles")]
    public SubtitleLine[] lines;
    public GameObject textbox;
    public TMP_Text subtitles;

    [Header("Speakers")]
    public Speaker[] speakers;

    private PlayerCharacter _playerCharacter;

    private void Start()
    {
        textbox.SetActive(false);
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
        DisableAllSpeakers();
        _playerCharacter._isCinematic = false;

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
                return;
            }
        }

        Debug.LogWarning($"[Cinematique] Speaker '{id}' non trouvé.");
    }
}
