namespace Drawing.CenterView.Library;

public interface IDrawingOperations : IOperations
{
    void FilterValid(List<DrawingModelBase> drawings);
}