using UnityEngine;

namespace com.Machinethoughts
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        public float tilt;
        public Boundary boundary;
        public GameController gameController;
        public GameObject shot;
        public Transform shotSpawn;
        public float fireRate;
        private float nextFire;
        float rotationSpeed = 100.0f;
        float thrustForce = 3f;

        void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
            }
        }

        void FixedUpdate()
        {
            //if(gameController.gameMode == GameMode.Invader)
            //{
            MoveLeftRight();
            //} else
            //{
            //    ForwardBackAndRotate();
            //}
        }

        void MoveLeftRight()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
            GetComponent<Rigidbody>().velocity = movement * speed;
            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        }

        void ForwardBackAndRotate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);
            GetComponent<Rigidbody>().AddForce(transform.forward * thrustForce * moveVertical);
        }
    }
}