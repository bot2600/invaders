using UnityEngine;
using System.Collections;

namespace com.Machinethoughts
{
    public class DestroyByContact : MonoBehaviour
    {
        public GameObject explosion;
        public GameObject playerExplosion;
        public int scoreValue = 10;

        void Start()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Boundary" || tag == "Boundary" || tag == other.tag)
            {
                return;
            }

            if (other.tag == "Enemy")
            {
                if (explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                }
                var invader = other.gameObject.GetComponent<Invader>();
                if (invader != null)
                {
                    invader.invaderManager.RemoveInvader(invader);
                    invader.invaderManager.AddScore(scoreValue);
                }
            }

            if (other.tag == "Player")
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                other.GetComponent<PlayerController>().gameController.PlayerDeath();
            }

            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}