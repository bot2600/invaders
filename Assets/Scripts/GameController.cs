using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace com.Machinethoughts
{
    public class GameController : MonoBehaviour
    {
        private int score = 0;
        private int lives = 3;
        [SerializeField]
        GameObject playerPrefab;
        [SerializeField]
        Transform playerSpawn;
        public GameObject player;
        public GameState gameState = GameState.Title;
        public GameObject[] boundaryTriggers = new GameObject[2];
        public InvaderManager invaderManager;

        //public InformationBarController informationBarController;
        //public TopBarController topBarController;
        //public MainMenuController mainMenuController;

        void Start()
        {
            invaderManager = GetComponent<InvaderManager>();
            invaderManager.StartInvaders();
            invaderManager.enabled = true;
            InitializeBoundaryTriggers();
            GetComponent<UFOManager>().enabled = true;
        }

        void Update()
        {

        }

        public void InitializeBoundaryTriggers()
        {
            boundaryTriggers[0].SetActive(true);
            boundaryTriggers[1].SetActive(false);
        }

        public void FlipBoundaryTriggers()
        {
            foreach (var boundary in boundaryTriggers)
            {
                boundary.SetActive(!boundary.activeSelf);
            }
        }

        public void PlayerDeath()
        {
            lives--;
            //topBarController.UpdateLives(lives);
            if (lives < 1)
            {
                gameState = GameState.Over;
                GameOver();
                return;
            }
            gameState = GameState.Respawning;
            //informationBarController.Show("Respawning");
            if (player.activeInHierarchy)
            {
                Destroy(player);
            }
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2);
            //informationBarController.Hide();
            player = SpawnPlayer();
            invaderManager.HandlePlayerDeath();
            gameState = GameState.Active;
        }

        GameObject SpawnPlayer()
        {
            //TODO: this check is here because hitting space click the menu Start! even when the canvas is disabled
            if (player != null && player.activeInHierarchy)
            {
                return player;
            }
            var spawn = Instantiate(playerPrefab, playerSpawn);
            spawn.GetComponent<PlayerController>().gameController = this;
            return spawn;
        }

        public void NewGame()
        {
            player = SpawnPlayer();
            score = 0;
            lives = 3;
            //topBarController.UpdateLives(lives);
            //mainMenuController.Hide();
            //topBarController.Show();
            gameState = GameState.Active;
        }

        public void CompleteLevel()
        {

        }

        public void GameOver()
        {
            //informationBarController.Show("Game Over");
            StartCoroutine(Reset());
        }

        IEnumerator Reset()
        {
            yield return new WaitForSeconds(4);
            //informationBarController.Hide();
            //topBarController.Hide();
            //mainMenuController.Show();
            gameState = GameState.Title;
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }

        public void AddScore(int points)
        {
            score += points;
        }

        public void SubtractLife()
        {

        }
    }
}
