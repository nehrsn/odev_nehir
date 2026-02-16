using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplierTextMesh;

    void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultiplierTextMesh.text = "x" + landingPad.GetScoreMultiplier();
    }
}
