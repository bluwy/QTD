using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using QTD.Towers;

namespace QTD
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GridTile : MonoBehaviour
    {
        public Tower Tower { get; set; }

        public bool IsOccupied => Tower is object;

        void Update()
        {
            // When click on tile and has tower, show tower options
            if (IsOccupied && !TowerManager.instance.IsPlacingTower && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // Collide with grid tile layer and ui layer (possibly tower ui)
                Collider2D collided = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << gameObject.layer);

                if (collided?.GetComponent<GridTile>() == this)
                {
                    // Is clicking on tile, show ui
                    Tower.ShowAttackRange();
                    Tower.ShowOptions();
                }
                else
                {
                    // Assume click other places, then close ui
                    Tower.HideAllUI();
                }
            }
        }
    }
}
