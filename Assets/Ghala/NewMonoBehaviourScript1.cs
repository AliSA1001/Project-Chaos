using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    public CinematicCutscene cutscene;

    void Start()
    {
        cutscene.PlayCutscene();
    }
}