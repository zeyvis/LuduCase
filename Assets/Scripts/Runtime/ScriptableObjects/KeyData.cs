using UnityEngine;

namespace LuduCase.Runtime.ScriptableObjects
{
    /// <summary>
    /// Data definition for key items.
    /// </summary>
    [CreateAssetMenu(fileName = "NewKeyData", menuName = "LuduCase/Items/Key Data")]
    public sealed class KeyData : ScriptableObject
    {
        [Tooltip("Display name of the key (e.g. 'Red Key')")]
        [SerializeField] private string m_keyName = "Key";

        public string KeyName => m_keyName;
    }
}