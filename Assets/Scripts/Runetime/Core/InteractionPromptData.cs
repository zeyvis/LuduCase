namespace LuduCase.Runtime.Core
{
    /// <summary>
    /// UI data for interaction feedback.
    /// </summary>
    public readonly struct InteractionPromptData
    {
        public readonly string PromptText;
        public readonly bool ShowHoldProgress;
        public readonly string CannotInteractReason;

        public InteractionPromptData(string promptText, bool showHoldProgress, string cannotInteractReason)
        {
            PromptText = promptText;
            ShowHoldProgress = showHoldProgress;
            CannotInteractReason = cannotInteractReason;
        }
    }
}
