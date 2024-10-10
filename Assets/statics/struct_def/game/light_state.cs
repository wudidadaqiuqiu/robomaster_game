namespace StructDef.Game {
    /// <summary>
    /// 灯光状态。
    /// </summary>
    public struct LightData {
        public LightColor color;
        public LightState state;
    }

    public enum LightColor {
        red,
        blue
    }


    public enum LightState
    {
        /// <summary>
        /// 亮起。
        /// </summary>
        on,
        /// <summary>
        /// 高亮。
        /// </summary>
        bright,
        /// <summary>
        /// 箭头流动。
        /// </summary>
        flow,
        /// <summary>
        /// 显示百分比。
        /// </summary>
        percentage,
        twink,//闪烁
        off,//关闭
    }
}