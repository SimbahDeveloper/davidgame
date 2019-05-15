using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bakiak
{
    public class BakiakFinish : MonoBehaviour
    {
        public static BakiakFinish init;
        private void Start()
        {
            if (init == null)
            {
                init = this;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.gameObject.GetComponent<BakiakPlayerControler>() != null)
            {
                Debug.Log("Horray");
                collision.transform.gameObject.GetComponent<BakiakPlayerControler>().isPlaying = false;
            }
        }
    }
}
