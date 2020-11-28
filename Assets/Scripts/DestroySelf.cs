using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTD
{
    /// <summary>
    /// Used in animition events to destroy gameobject after animation ends
    /// </summary>
    public class DestroySelf : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
