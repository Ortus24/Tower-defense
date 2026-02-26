using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverUI;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
    }
}