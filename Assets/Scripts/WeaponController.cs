using UnityEngine;

namespace com.Machinethoughts
{
    public class WeaponController : MonoBehaviour
    {
        public GameObject shot;
        public Transform shotSpawn;

        void Start()
        {

        }

        public void Fire()
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
    }
}