namespace WPFGame
{
    /// <summary>
    /// Helper class to
    /// hold the current
    /// resolution of
    /// the configuration
    /// </summary>
    public class ConfigResolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}
