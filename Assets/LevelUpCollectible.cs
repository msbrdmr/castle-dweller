using UnityEngine;

public class LevelUpCollectible : MonoBehaviour
{
    public int levelsToAdd = 5;

    private GameObject player; // Reference to the player GameObject
    public PlayerModel playerModel; // Reference to the PlayerModel component
    private PlayerView playerView; // Reference to the PlayerView component
    private PlayerController playerController; // Reference to the PlayerController component

    private bool foundPlayer = false; // Flag to track if player has been found

    private void Start()
    {
        // Start searching for the player
        FindPlayer();
    }

    private void Update()
    {
        // If player hasn't been found yet, continue searching
        if (!foundPlayer)
        {
            FindPlayer();
        }

        // Spin the collectible
        transform.Rotate(0, 0, 90 * Time.deltaTime);
    }

    private void FindPlayer()
    {
        // Try to find the player by tag every frame
        player = GameObject.FindGameObjectWithTag("Player");

        // If player is found, get its components and set foundPlayer to true
        if (player != null)
        {
            playerView = player.GetComponent<PlayerView>();
            playerController = player.GetComponent<PlayerController>();
            foundPlayer = true;
            Debug.Log("Player found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has been found and if the collider is the player
        if (foundPlayer && other.gameObject == player)
        {
            // Increase player's level
            playerModel.level += levelsToAdd;
            // Update UI or other components related to level display
            playerController.SetLevelText(playerModel.level);
            // Destroy the collectible GameObject
            Destroy(gameObject);
        }
    }
}
