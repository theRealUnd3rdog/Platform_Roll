using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private int _noOfPlayers;

    private void Update()
    {
        // For testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            Instantiate(Player, Vector3.up * 3.5f, Quaternion.identity);

            TimeManager.ResetTimer();
            TimeManager.ResumeTimer();
        }
    }
}
