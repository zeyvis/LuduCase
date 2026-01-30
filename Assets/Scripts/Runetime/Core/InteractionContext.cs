using UnityEngine;

namespace LuduCase.Runtime.Core
{
    /// <summary>
    /// Holds contextual data for an interaction attempt.
    /// </summary>
    public readonly struct InteractionContext
    {
        public readonly GameObject Interactor;

        public InteractionContext(GameObject interactor)
        {
            Interactor = interactor;
        }
    }
}
