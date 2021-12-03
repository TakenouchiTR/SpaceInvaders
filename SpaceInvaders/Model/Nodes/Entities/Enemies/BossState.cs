namespace SpaceInvaders.Model.Nodes.Entities.Enemies
{
    /// <summary>
    ///     Represents the current state of the boss
    /// </summary>
    public enum BossState
    {
        /// <summary>
        ///     Phase 1 of the boss fight
        /// </summary>
        Phase1,

        /// <summary>
        ///     Transitioning to phase 2
        /// </summary>
        Phase2Transition,

        /// <summary>
        ///     Setting up phase 2
        /// </summary>
        Phase2Setup,

        /// <summary>
        ///     Phase 2 of the boss fight
        /// </summary>
        Phase2,

        /// <summary>
        ///     Transitioning to phase 3
        /// </summary>
        Phase3Transition,

        /// <summary>
        ///     Setting up phase 3
        /// </summary>
        Phase3Setup,

        /// <summary>
        ///     Phase 3 of the boss fight
        /// </summary>
        Phase3,

        /// <summary>
        ///     Being destroyed animation
        /// </summary>
        Destroyed
    }
}