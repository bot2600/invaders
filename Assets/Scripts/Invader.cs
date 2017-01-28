using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Machinethoughts
{
    public class Invader : MonoBehaviour
    {
        public int row;
        public int column;
        public InvaderManager invaderManager;
        private float targetManeuver;
        public float tilt = 10;
        public float dodge = 5;

        float evasionStartTime;
        float evasionJourneyLength;
        Vector3 evasionStartPosition;
        Vector3 evasionDestination;

        bool evading = false;

        public void Setup()
        {
            gameObject.layer = LayerMask.NameToLayer("Invader");
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.position = new Vector3(column * 2, 0, row * 2);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        void Update()
        {
            if (evading)
            {
                float distanceCovered = (Time.time - evasionStartTime) * 1.6f;
                float fractionalJourney = distanceCovered / evasionJourneyLength;
                transform.position = Vector3.Lerp(evasionStartPosition, evasionDestination, fractionalJourney);
                if (evasionJourneyLength <= distanceCovered)
                {
                    BeginEvasion();
                }
            }
        }

        public void BeginEvasion()
        {
            evading = true;
            evasionStartTime = Time.time;
            targetManeuver = Random.Range(-dodge, dodge) * -Mathf.Sign(transform.position.x);
            evasionStartPosition = transform.position;
            evasionDestination = new Vector3(targetManeuver, evasionStartPosition.y, Random.Range(5, 15));
            evasionJourneyLength = Vector3.Distance(evasionStartPosition, evasionDestination);
        }

        public void EndEvasion()
        {
            if (transform.position.z < 6)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(5, 10));
            }
            evading = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Boundary")
            {
                if (evading)
                {
                    EndEvasion();
                    invaderManager.RemoveInvader(this);
                    Destroy(this);
                }

                invaderManager.ReverseMovement();
            }
        }
    }
}