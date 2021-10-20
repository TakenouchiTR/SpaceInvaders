namespace SpaceInvaders.Model
{
    /// <summary>
    ///     Represents which layer a sprite will be rendered on.<br />
    ///     Lower values will be rendered under higher values.
    /// </summary>
    public enum RenderLayer
    {
        /// <summary>
        ///     The bottom-most layer of the background layers.
        /// </summary>
        BackgroundBottom = 0,

        /// <summary>
        ///     The middle of the background layers.
        /// </summary>
        BackgroundMiddle = 1,

        /// <summary>
        ///     The top-most layer of the background layers.
        /// </summary>
        BackgroundTop = 2,

        /// <summary>
        ///     The bottom-most layer of the main playing layers.
        /// </summary>
        MainBottom = 3,

        /// <summary>
        ///     The second-lowest layer of the main playing layers.
        /// </summary>
        MainLowerMiddle = 4,

        /// <summary>
        ///     The second-highest layer of the main playing layers.
        /// </summary>
        MainUpperMiddle = 5,

        /// <summary>
        ///     The top-most layer of the main playing layers.
        /// </summary>
        MainTop = 6,

        /// <summary>
        ///     The bottom-most layer of the UI layers.
        /// </summary>
        UiBottom = 7,

        /// <summary>
        ///     The middle layer of the UI layers.
        /// </summary>
        UiMiddle = 8,

        /// <summary>
        ///     The top-most layer of the UI layers.
        /// </summary>
        UiTop = 9
    }
}