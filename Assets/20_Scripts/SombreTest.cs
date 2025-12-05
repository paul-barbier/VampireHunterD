using System.Collections;
using UnityEngine;

public class SombreTest : MonoBehaviour
{
    [Header("Cible")]
    [Tooltip("Transform du joueur. Si null, on cherche l'objet taggé 'Player' au Start.")]
    public Transform player;

    [Header("Zone de chute")]
    [Tooltip("Y au sommet (position initiale du joueur ou valeur fixe).")]
    public float topY = 0f;
    [Tooltip("Y au fond du trou — valeur où l'obscurité maximale est atteinte.")]
    public float bottomY = -10f;
    [Tooltip("Si true, on capture topY à l'instant où le joueur entre dans la zone.")]
    public bool captureTopOnEnter = true;

    [Header("Lumières à assombrir")]
    [Tooltip("Liste des lights à modifier (point, spot...). Si vide, seule l'ambiance sera modifiée.")]
    public Light[] lightsToDim;

    [Header("Paramètres d'intensité")]
    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    [Tooltip("Vitesse de l'interpolation (lissage).")]
    public float smoothSpeed = 6f;

    [Header("Ambiance (optionnel)")]
    public bool modifyAmbient = true;
    public Color startAmbient = Color.white;
    public Color endAmbient = Color.black;

    [Header("Courbe de transition")]
    [Tooltip("Permet d'ajuster non-linéairement la progression de l'obscurcissement.")]
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    bool playerInside = false;
    float currentT = 0f;

    void Start()
    {
        if (player == null)
        {
            var found = GameObject.FindGameObjectWithTag("Player");
            if (found != null) player = found.transform;
        }

        // si lightsToDim non assignées, on laisse l'ambiance gérer l'effet si activé
        if (modifyAmbient)
        {
            startAmbient = RenderSettings.ambientLight;
        }
    }

    void Update()
    {
        if (!playerInside || player == null) 
        {
            // retour progressif à l'état clair si le joueur sort
            currentT = Mathf.MoveTowards(currentT, 0f, Time.deltaTime * smoothSpeed);
            ApplyDimming(currentT);
            return;
        }

        // calculer t = 0 (top) -> 1 (bottom)
        float t = 0f;
        if (!Mathf.Approximately(topY, bottomY))
        {
            t = Mathf.InverseLerp(topY, bottomY, player.position.y);
            t = Mathf.Clamp01(t);
        }

        // appliquer courbe
        float evaluated = fadeCurve.Evaluate(t);

        // lisser la transition
        currentT = Mathf.MoveTowards(currentT, evaluated, Time.deltaTime * smoothSpeed);
        ApplyDimming(currentT);
    }

    void ApplyDimming(float t)
    {
        // intensity interpolation: 0 -> top (clair), 1 -> bottom (sombre)
        float targetIntensity = Mathf.Lerp(maxIntensity, minIntensity, t);

        if (lightsToDim != null)
        {
            foreach (var l in lightsToDim)
            {
                if (l == null) continue;
                l.intensity = targetIntensity;
            }
        }

        if (modifyAmbient)
        {
            RenderSettings.ambientLight = Color.Lerp(startAmbient, endAmbient, t);
        }
    }

    // Exiger que l'objet ait un collider avec isTrigger = true couvrant la zone du trou.
    void OnTriggerEnter(Collider other)
    {
        if (player == null) return;

        if (other.transform == player || other.CompareTag("Player"))
        {
            playerInside = true;
            if (captureTopOnEnter)
            {
                topY = player.position.y;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (player == null) return;

        if (other.transform == player || other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
