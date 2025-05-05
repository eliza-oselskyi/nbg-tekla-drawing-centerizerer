namespace Drawing.CenterView.Library;

/// <summary>
/// GlobalConfig Singelton
/// </summary>
public sealed class GlobalConfig
{
    /// <summary>
    /// Global DrawingHandler object.
    /// </summary>
    public static readonly Tekla.Structures.Drawing.DrawingHandler
        DrawingHandler = new Tekla.Structures.Drawing.DrawingHandler();

    private readonly DrawingSetModel _drawingSetInstance = new DrawingSetModel();
    private static readonly Lazy<GlobalConfig> Lazy = new Lazy<GlobalConfig>(() => new GlobalConfig());
    public static GlobalConfig Instance => Lazy.Value;
    private GlobalConfig() { }

    /// <summary>
    /// Returns a single instance of a drawing set
    /// </summary>
    /// <returns>DrawingSetModel</returns>
    public DrawingSetModel GetDrawingSet()
    {
        return _drawingSetInstance;
    }
}