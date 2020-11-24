using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTD.Towers;

namespace QTD
{
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager instance;

        [SerializeField]
        private LayerMask _gridLayer;

        private Tower _placingTower;

        private GridTile _targetTile;

        void Awake()
        {
            if (!instance)
                instance = this;
        }

        void Update()
        {
            if (_placingTower is null) return;

            HoverTower();

            if (Input.GetMouseButtonDown(0))
                PlaceTower();
        }

        public void SelectTower(GameObject tower)
        {
            if (_placingTower is object) return;

            // Place in off-screen (hard-coded but hey)
            _placingTower = Instantiate(tower, new Vector2(1000, 1000), Quaternion.identity).GetComponent<Tower>();
        }

        private void HoverTower()
        {
            Collider2D collided = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), _gridLayer.value);

            _targetTile = collided?.GetComponent<GridTile>();

            if (_targetTile is null)
                _placingTower.transform.position = new Vector2(1000, 1000);
            else
                _placingTower.transform.position = _targetTile.transform.position;
        }

        private void PlaceTower()
        {
            Collider2D collided = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), _gridLayer.value);

            // Make sure when placing tower, the grid its placing on is the same as hover (double-check)
            if (collided is object && collided?.GetComponent<GridTile>() == _targetTile)
            {
                _placingTower.transform.position = _targetTile.transform.position;

                // Tower can shoot when assigned placed tile
                _placingTower.PlacedTile = _targetTile;

                // Occupy space in tile, so no other tower can be placed here
                _targetTile.Tower = _placingTower;

                GameManager.instance.UseGold(_placingTower.InitialCost);

                // Clean-up
                _placingTower = null;
                _targetTile = null;
            }
        }
    }
}
