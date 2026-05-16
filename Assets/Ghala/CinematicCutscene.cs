using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages a cinematic cutscene with multiple shots.
/// Per‑shot dialogue (audio + subtitle), SFX, smooth camera movement,
/// start button, and objects activated at the end.
/// No slow‑motion, no fade transitions.
/// **Now plays only once per scene load.**
/// </summary>
public class CinematicCutscene : MonoBehaviour
{
    [Header("General References")]
    [Tooltip("The player's main camera, restored after the cutscene.")]
    public Camera defaultCamera;

    [Tooltip("TextMeshPro component for subtitles.")]
    public TMP_Text subtitleTextUI;

    [Tooltip("AudioSource used for dialogue and SFX.")]
    public AudioSource audioSource;

    [Tooltip("Optional start button (will be hidden when cutscene begins).")]
    public GameObject startButtonUI;

    [Tooltip("GameObjects to activate when the cutscene finishes completely.")]
    public GameObject[] onCutsceneEndActivate;

    [Tooltip("GameObjects to DeActivate when the cutscene finishes completely.")]
    public GameObject[] onCutsceneEndDetectiv;

    [Tooltip("OnStart")]
    public GameObject[] onStartCutsceneDetectiv;

    [Header("Smooth Camera Movement")]
    [Tooltip("If assigned, this single camera will move smoothly between shot positions. Leave null to use individual shot cameras (instant cuts).")]
    private Camera cinematicCamera;

    [Header("Cutscene Data")]
    public Shot[] shots;

    private bool isPlaying;
    private Coroutine cutsceneRoutine;
    private Coroutine dialogueRoutine;
    private bool hasPlayed = false;   // ✅ New flag to prevent replay

    public enum TransitionType
    {
        Instant,
        Smooth   // Smooth camera move (only works if cinematicCamera is set)
    }

    [System.Serializable]
    public class DialogueLine
    {
        public AudioClip audioClip;
        [TextArea(1, 3)]
        public string subtitle;
    }

    [System.Serializable]
    public class Shot
    {
        public string shotName;

        [Tooltip("Duration of this shot in real‑time seconds.")]
        public float duration = 3f;

        public ShotType shotType = ShotType.Static;

        [Tooltip("Camera reference for this shot. If using a cinematicCamera, its transform defines the target position/rotation.")]
        public Camera shotCamera;

        [Tooltip("Transition when entering this shot.")]
        public TransitionType transitionType = TransitionType.Instant;

        [Tooltip("Duration of the camera movement (only for Smooth transition).")]
        public float transitionDuration = 0.5f;

        [Tooltip("SFX to play at the start of the shot (all at once).")]
        public AudioClip[] sfxClips;

        [Tooltip("Dialogue lines played sequentially during the shot.")]
        public DialogueLine[] dialogueLines;

        [Tooltip("Waypoints for MovingPath (ignored if Static).")]
        public Transform[] pathWaypoints;
    }

    public enum ShotType
    {
        Static,
        MovingPath
    }

    void Start()
    {
        if (startButtonUI != null)
            startButtonUI.SetActive(true);

        if (onStartCutsceneDetectiv != null)
        {
            foreach (var obj in onStartCutsceneDetectiv)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    // ────────────── Public API ──────────────
    public void PlayCutscene()
    {
        // ✅ Prevent replay after first execution
        if (hasPlayed)
        {
            Debug.Log("Cutscene has already played once. Ignoring PlayCutscene().");
            return;
        }

        if (isPlaying)
            StopCutscene();

        hasPlayed = true;   // Mark as played

        if (startButtonUI != null)
            startButtonUI.SetActive(false);

        cutsceneRoutine = StartCoroutine(RunCutscene());
    }

    public void StopCutscene()
    {
        if (cutsceneRoutine != null)
            StopCoroutine(cutsceneRoutine);
        ResetCutscene();
    }

    // ────────────── Main Cutscene Coroutine ──────────────
    private IEnumerator RunCutscene()
    {
        isPlaying = true;

        // Deactivate all shot cameras (if we use per‑shot cameras)
        if (cinematicCamera == null)
            DeactivateAllShotCameras();
        else
            cinematicCamera.gameObject.SetActive(true);

        if (defaultCamera != null)
            defaultCamera.gameObject.SetActive(false);

        Transform lastTarget = null;
        if (cinematicCamera != null)
            lastTarget = cinematicCamera.transform; // start position

        foreach (var shot in shots)
        {
            if (shot == null) continue;

            Transform targetTransform = (shot.shotCamera != null) ? shot.shotCamera.transform : null;
            if (cinematicCamera == null && targetTransform != null)
                targetTransform.gameObject.SetActive(true);

            // ── Transition ──
            if (shot.transitionType == TransitionType.Smooth)
            {
                if (cinematicCamera != null && targetTransform != null && lastTarget != null)
                {
                    yield return StartCoroutine(MoveCamera(cinematicCamera.transform, lastTarget, targetTransform, shot.transitionDuration));
                }
                else
                {
                    // No cinematic camera, just fallback to instant cut
                    if (cinematicCamera == null && targetTransform != null)
                    {
                        DeactivateAllShotCameras();
                        targetTransform.gameObject.SetActive(true);
                    }
                }
            }
            else // Instant
            {
                if (cinematicCamera != null && targetTransform != null && lastTarget != null)
                {
                    cinematicCamera.transform.position = targetTransform.position;
                    cinematicCamera.transform.rotation = targetTransform.rotation;
                }
                else if (cinematicCamera == null && targetTransform != null)
                {
                    DeactivateAllShotCameras();
                    targetTransform.gameObject.SetActive(true);
                }
            }

            if (cinematicCamera != null)
                lastTarget = cinematicCamera.transform;

            // ── Play SFX ──
            if (shot.sfxClips != null && audioSource != null)
            {
                foreach (var sfx in shot.sfxClips)
                {
                    if (sfx != null)
                        audioSource.PlayOneShot(sfx);
                }
            }

            // ── Start dialogue ──
            dialogueRoutine = null;
            if (shot.dialogueLines != null && shot.dialogueLines.Length > 0)
                dialogueRoutine = StartCoroutine(PlayDialogueSequence(shot.dialogueLines));

            // ── Execute shot (timer + optional moving path) ──
            float timer = 0f;
            if (shot.shotType == ShotType.Static)
            {
                while (timer < shot.duration)
                {
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            else if (shot.shotType == ShotType.MovingPath)
            {
                Transform camTransform = cinematicCamera ? cinematicCamera.transform : targetTransform;
                if (camTransform == null || shot.pathWaypoints == null || shot.pathWaypoints.Length < 2)
                {
                    while (timer < shot.duration)
                    {
                        timer += Time.unscaledDeltaTime;
                        yield return null;
                    }
                }
                else
                {
                    while (timer < shot.duration)
                    {
                        timer += Time.unscaledDeltaTime;
                        float progress = Mathf.Clamp01(timer / shot.duration);
                        camTransform.position = GetPathPoint(shot.pathWaypoints, progress);
                        camTransform.rotation = GetPathRotation(shot.pathWaypoints, progress);
                        yield return null;
                    }
                }
            }

            // ── Clean up after shot ──
            if (dialogueRoutine != null)
            {
                StopCoroutine(dialogueRoutine);
                dialogueRoutine = null;
            }
            if (audioSource != null)
                audioSource.Stop();
            if (subtitleTextUI != null)
                subtitleTextUI.gameObject.SetActive(false);

            if (cinematicCamera == null && targetTransform != null)
                targetTransform.gameObject.SetActive(false);
        }

        // ── Cutscene finished – activate assigned objects ──
        if (onCutsceneEndActivate != null)
        {
            foreach (var obj in onCutsceneEndActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        // ── Cutscene finished – deactivate assigned objects ──
        if (onCutsceneEndDetectiv != null)
        {
            foreach (var obj in onCutsceneEndDetectiv)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        // Restore main camera
        if (defaultCamera != null)
            defaultCamera.gameObject.SetActive(true);
        if (subtitleTextUI != null)
            subtitleTextUI.gameObject.SetActive(false);
        if (audioSource != null)
            audioSource.Stop();
        if (cinematicCamera != null)
            cinematicCamera.gameObject.SetActive(false);

        isPlaying = false;
    }

    // ────────────── Dialogue Sequence ──────────────
    private IEnumerator PlayDialogueSequence(DialogueLine[] lines)
    {
        foreach (var line in lines)
        {
            if (line == null) continue;

            if (subtitleTextUI != null)
            {
                subtitleTextUI.text = line.subtitle;
                subtitleTextUI.gameObject.SetActive(!string.IsNullOrEmpty(line.subtitle));
            }

            if (line.audioClip != null && audioSource != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();
                yield return new WaitForSecondsRealtime(line.audioClip.length);
            }
            else
            {
                yield return null;
            }
        }

        if (subtitleTextUI != null)
            subtitleTextUI.gameObject.SetActive(false);
    }

    // ────────────── Smooth Camera Movement ──────────────
    private IEnumerator MoveCamera(Transform cam, Transform from, Transform to, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = from.position;
        Quaternion startRot = from.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cam.position = Vector3.Lerp(startPos, to.position, t);
            cam.rotation = Quaternion.Slerp(startRot, to.rotation, t);
            yield return null;
        }

        cam.position = to.position;
        cam.rotation = to.rotation;
    }

    // ────────────── Helpers ──────────────
    private void DeactivateAllShotCameras()
    {
        foreach (var shot in shots)
        {
            if (shot != null && shot.shotCamera != null)
                shot.shotCamera.gameObject.SetActive(false);
        }
    }

    private void ResetCutscene()
    {
        StopAllCoroutines();
        dialogueRoutine = null;
        cutsceneRoutine = null;

        if (defaultCamera != null)
            defaultCamera.gameObject.SetActive(true);
        DeactivateAllShotCameras();
        if (subtitleTextUI != null)
            subtitleTextUI.gameObject.SetActive(false);
        if (audioSource != null)
            audioSource.Stop();
        if (cinematicCamera != null)
            cinematicCamera.gameObject.SetActive(false);

        isPlaying = false;
    }

    // ────────────── Path Interpolation ──────────────
    private Vector3 GetPathPoint(Transform[] waypoints, float t)
    {
        if (waypoints.Length == 0) return Vector3.zero;
        if (waypoints.Length == 1) return waypoints[0].position;

        float totalLength = 0f;
        float[] segLengths = new float[waypoints.Length - 1];
        for (int i = 0; i < segLengths.Length; i++)
        {
            segLengths[i] = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalLength += segLengths[i];
        }

        float targetDist = t * totalLength;
        float accum = 0f;
        for (int i = 0; i < segLengths.Length; i++)
        {
            if (targetDist <= accum + segLengths[i])
            {
                float segT = (targetDist - accum) / segLengths[i];
                return Vector3.Lerp(waypoints[i].position, waypoints[i + 1].position, segT);
            }
            accum += segLengths[i];
        }
        return waypoints[waypoints.Length - 1].position;
    }

    private Quaternion GetPathRotation(Transform[] waypoints, float t)
    {
        if (waypoints.Length == 0) return Quaternion.identity;
        if (waypoints.Length == 1) return waypoints[0].rotation;

        float totalLength = 0f;
        float[] segLengths = new float[waypoints.Length - 1];
        for (int i = 0; i < segLengths.Length; i++)
        {
            segLengths[i] = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalLength += segLengths[i];
        }

        float targetDist = t * totalLength;
        float accum = 0f;
        for (int i = 0; i < segLengths.Length; i++)
        {
            if (targetDist <= accum + segLengths[i])
            {
                float segT = (targetDist - accum) / segLengths[i];
                return Quaternion.Slerp(waypoints[i].rotation, waypoints[i + 1].rotation, segT);
            }
            accum += segLengths[i];
        }
        return waypoints[waypoints.Length - 1].rotation;
    }

    // ────────────── Gizmos ──────────────
    private void OnDrawGizmos()
    {
        if (shots == null) return;

        foreach (var shot in shots)
        {
            if (shot == null || shot.shotType != ShotType.MovingPath) continue;
            if (shot.pathWaypoints == null || shot.pathWaypoints.Length < 2) continue;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < shot.pathWaypoints.Length - 1; i++)
            {
                if (shot.pathWaypoints[i] != null && shot.pathWaypoints[i + 1] != null)
                    Gizmos.DrawLine(shot.pathWaypoints[i].position, shot.pathWaypoints[i + 1].position);
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < shot.pathWaypoints.Length; i++)
            {
                if (shot.pathWaypoints[i] != null)
                    Gizmos.DrawSphere(shot.pathWaypoints[i].position, 0.2f);
            }
        }
    }
}