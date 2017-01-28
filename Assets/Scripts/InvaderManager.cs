using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace com.Machinethoughts
{
    public class InvaderManager : MonoBehaviour
    {
        int columns = 10;
        int rows = 4;
        float startHeight = 57.3f;
        float minHeight = 36.3f;
        Vector2 spacing = new Vector2(5, 6);
        public Transform invaderContainer;
        public GameObject invaderPrefab;
        float accurateBombChance = 0.42f;
        public GameObject player { get { return gameController == null ? null : gameController.player; } }
        public GameState gameState { get { return gameController == null ? GameState.Title : gameController.gameState; } }
        List<Invader> invaders = new List<Invader>();
        bool enraged = false;
        int direction;
        float speed = 1.6f;
        float maxSpeed = 14.0f;
        float fireRate = 0.75f;
        private float nextFire = 0.0f;

        public int bombCount = 0;
        GameController gameController;

        // Use this for initialization
        void Start()
        {
            gameController = GetComponent<GameController>();
            var processingColumn = 0;
            var processingRow = 0;

            for (processingRow = 0; processingRow < rows; processingRow++)
            {
                for (processingColumn = 0; processingColumn < columns; processingColumn++)
                {
                    GameObject invaderObject = Instantiate(invaderPrefab);
                    var invader = invaderObject.GetComponent<Invader>();
                    invader.row = processingRow;
                    invader.column = processingColumn;
                    invader.Setup();
                    invader.invaderManager = this;
                    invaders.Add(invader);
                    invaderObject.transform.SetParent(invaderContainer, false);
                }
            }
            nextFire = Time.time + 0.4f;
        }

        void Update()
        {
            if (invaders.Count == 0 || enraged)
            {
                return;
            }
            invaderContainer.Translate(Vector3.right * (speed * direction * Time.deltaTime));
        }

        void FixedUpdate()
        {
            if (gameState != GameState.Active)
            {
                return;
            }
            MissileControl();
        }

        public void HandlePlayerDeath()
        {
            if (!enraged)
            {
                return;
            }
            speed = speed / 2f;
            invaders.ForEach(x => x.EndEvasion());
            enraged = false;
        }

        public void AddScore(int score)
        {
            gameController.AddScore(score);
        }

        public void StartInvaders()
        {
            //var bunkerManager = GetComponent<BunkerManager>();
            //bunkerManager.enabled = true;
            //if (startHeight > minHeight + 6)
            //{
            //    GetComponent<BunkerManager>().SetBunkers();
            //}
            //else
            //{
            //    GetComponent<BunkerManager>().DisableBunkers();
            //}

            invaders.ForEach(invader =>
            {
                invader.invaderManager = this;
                invader.gameObject.SetActive(true);
                invader.Setup();
            });

            direction = 1;
            bombCount = 0;
        }

        public void MissileControl()
        {
            if (Time.time < nextFire && !enraged)
            {
                return;
            }
            FireMissile();
        }

        void FireMissile()
        {
            if (player == null)
            {
                return;
            }
            Invader closestInvader = null;
            if (invaders.Count == 0)
            {
                return;
            }

            if (Random.value < accurateBombChance)
            {
                var closestDistance = 99999.0;
                invaders.ForEach(invader =>
                {
                    var thisDistance = Mathf.Abs(invader.gameObject.transform.position.x - player.transform.position.x);
                    if (thisDistance < closestDistance)
                    {
                        closestInvader = invader;
                        closestDistance = thisDistance;
                    }
                });
            }
            else
            {
                var index = Random.Range(0, Mathf.Max(0, invaders.Count - 1));
                closestInvader = invaders[index];
            }

            if (closestInvader != null)
            {
                closestInvader.GetComponent<WeaponController>().Fire();
            }
            nextFire = Time.time + fireRate;
        }

        public bool CanIEnrage()
        {
            if (gameController.gameState != GameState.Active)
            {
                return false;
            }

            bool tooClose = false;
            invaders.ForEach(invader =>
            {
                if (invader.transform.position.z - player.transform.position.z < 4)
                {
                    tooClose = true;
                }
            });

            if (tooClose)
            {
                //INFO:redoing this later
                speed = speed * 2.6f;
                //invaders.ForEach(x => x.BeginEvasion());
            }
            return tooClose;
        }

        public Invader GetLowest(int column)
        {
            int lowestRow = 0;
            invaders.Where(c => c.column == column).ToList().ForEach(invader =>
            {
                lowestRow = Mathf.Max(lowestRow, invader.row);
            });
            return invaders.FirstOrDefault(x => x.column == column && x.row == lowestRow);
        }

        public void ReverseMovement()
        {
            direction = -direction;
            gameController.FlipBoundaryTriggers();
            MoveInvadersDown();
        }

        public void MoveInvadersDown()
        {
            if (enraged)
            {
                speed = speed * 1.6f;
                speed = Mathf.Min(speed, maxSpeed);
                return;
            }
            var startPos = invaderContainer.position.z;
            var endPos = startPos - 1;
            var t = 0.0;
            while (t < 1.0)
            {
                t += Time.deltaTime * 0.06;
                invaderContainer.position = new Vector3(invaderContainer.position.x, invaderContainer.position.y, Mathf.Lerp(startPos, endPos, (float)t));
            }
            if (!enraged)
            {
                enraged = CanIEnrage();
            }
        }

        public void RemoveInvader(Invader invader)
        {
            invaders.Remove(invader);
            Destroy(invader);
            if (invaders.Count == 20)
            {
                speed = speed * 1.6f;
                fireRate = fireRate / 2;
            }

            if (invaders.Count == 10)
            {
                speed = speed * 1.6f;
                fireRate = fireRate / 2;
            }

            if (invaders.Count == 0)
            {
                gameController.CompleteLevel();
            }
        }

        public void LowerStartHeight()
        {
            startHeight = Mathf.Max(startHeight - spacing.y / 2, minHeight);    // Have the invaders start down a row next level (up to a point)
        }
    }
}