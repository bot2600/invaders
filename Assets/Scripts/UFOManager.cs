using UnityEngine;

namespace com.Machinethoughts
{
    public class UFOManager : MonoBehaviour
    {
        public GameObject prefab;
        public UFO ufo;
        float minWaitTime = 0.015f;
        float maxWaitTime = 0.025f;
        public float timeOfLastUFO;
        public GameObject player;

        GameController gameController;

        // Use this for initialization
        void Start()
        {
            timeOfLastUFO = Time.deltaTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (ufo == null)
            {
                //var timer = Random.Range(minWaitTime, maxWaitTime);
                //if (timer < Time.deltaTime - timeOfLastUFO)
                //{
                var ufoPrefab = Instantiate(prefab);
                ufo = ufoPrefab.GetComponent<UFO>();
                ufo.ufoManager = this;
                //}
            }
        }
    }
}