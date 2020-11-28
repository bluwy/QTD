using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTD.Towers;

namespace QTD
{
    /// <summary>
    /// Manage placing towers
    /// </summary>
    public class TowerManager : MonoBehaviour
    {
        public static TowerManager instance;

        [SerializeField]
        private LayerMask _gridLayer;

        private Tower _placingTower;

        private GridTile _targetTile;

        public bool IsPlacingTower => _placingTower is object;

        void Awake()
        {
            if (!instance)
                instance = this;
        }

        void Update()
        {
            if (_placingTower is null) return;

            HoverTower();

            if (Input.GetKeyDown(KeyCode.Escape))
                CancelTower();
            else if (Input.GetMouseButtonDown(0))
                PlaceTower();
        }

        /// <summary>
        /// </summary>
        /// <param name="tower"></param>
        public void SelectTower(GameObject tower)
        {
            if (IsPlacingTower) return;

            // Place in off-screen (hard-coded but hey)
            _placingTower = Instantiate(tower, new Vector2(1000, 1000), Quaternion.identity).GetComponent<Tower>();

            // Preview attack range
            _placingTower.ShowAttackRange();

            // Hide selects
            UIManager.instance.ShowTowerSelects(true);
        }

        private void CancelTower()
        {
            if (!IsPlacingTower) return;

            Destroy(_placingTower.gameObject);

            // Show selects
            UIManager.instance.ShowTowerSelects(false);

            // Clean-up
            _placingTower = null;
            _targetTile = null;
        }

        /// <summary>
        /// Handle pre-placing tower when hovering over tiles
        /// </summary>
        private void HoverTower()
        {
            Collider2D collided = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), _gridLayer.value);

            _targetTile = collided?.GetComponent<GridTile>();

            if (_targetTile is null || _targetTile.IsOccupied)
                _placingTower.transform.position = new Vector2(1000, 1000);
            else
                _placingTower.transform.position = _targetTile.transform.position;
        }

        private void PlaceTower()
        {
            Collider2D collided = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), _gridLayer.value);

            GridTile grid = collided?.GetComponent<GridTile>();

            // Make sure when placing tower, the grid its placing on is the same as hover (double-check)
            if (grid is object && grid == _targetTile && !grid.IsOccupied)
            {
                _placingTower.transform.position = _targetTile.transform.position;
                _placingTower.HideAllUI();

                // Tower can shoot when assigned placed tile
                _placingTower.PlacedTile = _targetTile;

                // Occupy space in tile, so no other tower can be placed here
                _targetTile.Tower = _placingTower;

                // Show selects
                UIManager.instance.ShowTowerSelects(false);

                GameManager.instance.UseGold(_placingTower.InitialCost);

                // Clean-up
                _placingTower = null;
                _targetTile = null;
            }
        }
    }
}
