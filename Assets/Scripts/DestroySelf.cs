using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTD
{
    public class DestroySelf : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
