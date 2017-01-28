using System.Collections;
using UnityEngine;

namespace com.Machinethoughts
{
    public class UFO : MonoBehaviour
    {
        float speed = 3;
        float direction = 1;
        public UFOManager ufoManager;
        bool firing = false;

        // Use this for initialization
        void Start()
        {
            direction = (Random.value < 0.5) ? 1 : -1;
            transform.position = new Vector3(direction == 1 ? -16.5f : 17, 0f, 17.5f);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.right * (speed * direction * Time.deltaTime));
            if (transform.position.x > 18 || transform.position.x < -18)
            {
                Destroy(this.gameObject);
                Destroy(this);
                ufoManager.timeOfLastUFO = Time.deltaTime;
            }
        }

        void Fire()
        {
            var line = GetComponent<LineRenderer>();
            var positions = new Vector3[2];
            positions[0] = gameObject.transform.position;
            positions[1] = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -15);
            line.SetPositions(positions);
            line.enabled = true;
            StartCoroutine(CompleteFire());
        }

        IEnumerator CompleteFire()
        {
            yield return new WaitForSeconds(1);
            GetComponent<LineRenderer>().enabled = false;
        }

        void OnCollisionEnter()
        {
            //ufoManager.timeOfLastUFO = Time.deltaTime;
            //Destroy(this.gameObject);
            //Destroy(this);
            Fire();
            ////ExplosionManager.use.Explode(ExplosionType.Invader, transform.position);
            //exploded = true;
            //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // So the missile can't hit it again, since we don't deactivate right away

            //GetComponent<Renderer>().enabled = false;

            //// Choose random point value of 50, 100, 150, or 300 (unlike the real Space Invaders, where this depends on the number of missiles fired)
            ////TODO:do this right or make it fixed
            //var points = Random.Range(1, 5) * 50;
            //if (points == 200)
            //{
            //    points = 300;
            //}
            ////TODO:add the score to the real stuff
            ////GameManager.use.AddScore(points);

            //// Increase pitch over time, while decreasing volume
            //while (GetComponent<AudioSource>().pitch < 3.0)
            //{
            //    GetComponent<AudioSource>().pitch += Time.deltaTime * 1.5f;
            //    GetComponent<AudioSource>().volume -= Time.deltaTime * 0.4f;
            //}
        }
    }
}