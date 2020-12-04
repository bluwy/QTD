#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace QTD
{
    /// <summary>
    /// Custom editor that draws timeline of wave's spawns
    /// </summary>
    [CustomPropertyDrawer(typeof(Wave))]
    public class WaveEditor : PropertyDrawer
    {
        private const float TIMELINE_HEIGHT = 2f;
        private const float TIMELINE_MARGIN_BOTTOM = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw default ui
            EditorGUI.PropertyField(position, property, label, true);

            float defaultUiHeight = EditorGUI.GetPropertyHeight(property);

            // Draw timeline
            List<SpawnGroup> spawnGroups = GetSpawnGroups(property);

            // Draw timeline background
            Rect bgRect = position;
            bgRect.y += defaultUiHeight;
            bgRect.height -= defaultUiHeight;
            EditorGUI.DrawRect(bgRect, Color.gray);

            // Draw timeline bars
            float waveDuration = GetWaveDuration(spawnGroups);
            float cumulativeDuration = 0f;
            for (int i = 0; i < spawnGroups.Count; i++)
            {
                SpawnGroup group = spawnGroups[i];

                Rect groupRect = position;
                groupRect.x += cumulativeDuration / waveDuration * groupRect.width;
                groupRect.y += defaultUiHeight + (TIMELINE_HEIGHT + TIMELINE_MARGIN_BOTTOM) * i;
                groupRect.width = (group.Count * group.Interval) / waveDuration * groupRect.width;
                groupRect.height = TIMELINE_HEIGHT;

                EditorGUI.DrawRect(groupRect, Color.red);

                cumulativeDuration += group.SpawnDuration;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            List<SpawnGroup> spawnGroups = GetSpawnGroups(property);
            return EditorGUI.GetPropertyHeight(property) + (TIMELINE_HEIGHT + TIMELINE_MARGIN_BOTTOM) * spawnGroups.Count;
        }

        /// <summary>
        /// Get list of spawn group's from serialized property
        /// </summary>
        private List<SpawnGroup> GetSpawnGroups(SerializedProperty property)
        {
            List<SpawnGroup> spawnGroups = new List<SpawnGroup>();
            SerializedProperty spawnGroupsProperty = property.FindPropertyRelative("_spawnGroups");

            for (int i = 0; i < spawnGroupsProperty.arraySize; i++)
            {
                SerializedProperty groupProperty = spawnGroupsProperty.GetArrayElementAtIndex(i);
                spawnGroups.Add(new SpawnGroup(
                    groupProperty.FindPropertyRelative("_enemy").objectReferenceValue as GameObject,
                    groupProperty.FindPropertyRelative("_count").intValue,
                    groupProperty.FindPropertyRelative("_interval").floatValue,
                    groupProperty.FindPropertyRelative("_spawnDuration").floatValue
                ));
            }

            return spawnGroups;
        }

        /// <summary>
        /// Total up spawn durations
        /// </summary>
        private float GetWaveDuration(List<SpawnGroup> spawnGroups)
        {
            float duration = 0f;
            foreach (SpawnGroup group in spawnGroups)
                duration += group.SpawnDuration;
            return duration;
        }
    }
}
#endif
