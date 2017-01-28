using UnityEngine;

namespace com.Machinethoughts
{
    public class DestroyByBoundary : MonoBehaviour
    {
        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Boundary" || tag == other.tag)
            {
                return;
            }
            Destroy(other.gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Boundary" || tag == other.tag)
            {
                return;
            }
            Destroy(other.gameObject);
        }
    }
}