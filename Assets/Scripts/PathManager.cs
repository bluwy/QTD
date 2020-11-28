using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTD
{
    /// <summary>
    /// Contains all possible paths the enemies can take
    /// </summary>
    public class PathManager : MonoBehaviour
    {
        /// <summary>
        /// This struct is quite redundant, but I have no choice because Unity doesn't
        /// show "list of list" in the inspector, which would be use in <see cref="_paths"/>.
        /// This custom struct botches the behavior.
        /// </summary>
        [System.Serializable]
        private struct Path
        {
            public List<Vector2> waypoints;

            public Path(List<Vector2> waypoints)
            {
                this.waypoints = waypoints;
            }
        }

        public static PathManager instance;

        [SerializeField]
        private List<Path> _paths = new List<Path>();

        public void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public List<Vector2> GetRandomPath()
        {
            return _paths[Random.Range(0, _paths.Count - 1)].waypoints;
        }

        void OnDrawGizmosSelected()
        {
            // Draw path preview in editor
            foreach (Path path in _paths)
            {
                List<Vector2> waypoints = path.waypoints;
                for (int i = 0; i < waypoints.Count - 1; i++)
                    Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }
        }
    }
}
