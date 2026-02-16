using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject SpeedRightArrow;
    [SerializeField] private GameObject SpeedLeftArrow;
    [SerializeField] private GameObject SpeedUpArrow;
    [SerializeField] private GameObject SpeedDownArrow;
    [SerializeField] private Image fuelImage;



    private void Update()
    {
        UpdateStatsTextMesh();
    }

    private void UpdateStatsTextMesh()
    {
        SpeedUpArrow.SetActive(LanderMovement.Instance.GetSpeedY() >= 0);
        SpeedDownArrow.SetActive(LanderMovement.Instance.GetSpeedY() < 0);
        SpeedRightArrow.SetActive(LanderMovement.Instance.GetSpeedX() >= 0);
        SpeedLeftArrow.SetActive(LanderMovement.Instance.GetSpeedX() < 0);

        fuelImage.fillAmount = LanderMovement.Instance.GetFuelAmountNormalized();
        
        statsTextMesh.text = GameManager.Instance.GetScore() +"\n" +
        Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
        Mathf.Abs(Mathf.Round(LanderMovement.Instance.GetSpeedX() * 10f)) + "\n" +
        Mathf.Abs(Mathf.Round(LanderMovement.Instance.GetSpeedY() * 10f)); 
    }
}
