using System.Collections.Generic;
using UnityEngine;
using LuduCase.Runtime.ScriptableObjects;

namespace LuduCase.Runtime.Player
{
    /// <summary>
    /// Simple inventory system to store collected keys.
    /// </summary>
    public sealed class Inventory : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<KeyData> m_collectedKeys = new List<KeyData>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a key to the inventory.
        /// </summary>
        public void AddKey(KeyData key)
        {
            if (key != null && !m_collectedKeys.Contains(key))
            {
                m_collectedKeys.Add(key);
                Debug.Log($"[Inventory] Added Key: {key.KeyName}");
            }
        }

        /// <summary>
        /// Checks if the inventory contains a specific key.
        /// </summary>
        public bool HasKey(KeyData key)
        {
            return m_collectedKeys.Contains(key);
        }

        #endregion
    }
}