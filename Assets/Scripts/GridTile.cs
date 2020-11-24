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

        public bool IsOccupied
        {
            get { return Tower is object; }
        }

        void Update()
        {
            if (IsOccupied && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                // Collide with grid tile layer and ui layer (possibly tower ui)
                Collider2D collided = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << gameObject.layer);

                if (collided?.GetComponent<GridTile>() == this)
                {
                    Tower.ShowAttackRange();
                    Tower.ShowOptions();
                }
                else
                {
                    Tower.HideAllUI();
                }
            }
        }
    }
}
