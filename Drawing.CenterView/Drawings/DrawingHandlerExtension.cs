using System;
using System.Windows.Forms;
using Drawing.CenterView.Views;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model.Operations;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView;

public class DrawingHandlerExtension
{
    private ViewHandler _viewHandler = new ViewHandler();
    public DrawingEnumerator Drawings;
    private DrawingHandler _drawingHandler = new DrawingHandler();

    public DrawingHandlerExtension(DrawingEnumerator drawings)
    {
        Drawings = drawings;
    }

    public void CenterDriver(DialogResult dialogResult)
    {
        switch (dialogResult)
        {
            case  DialogResult.OK:
                CenterDriver(null );
                break;
            case DialogResult.Yes:
                CenterDriver(true);
                break;
            case DialogResult.No:
                CenterDriver(false);
                break;
            case DialogResult.Cancel:
                Operation.DisplayPrompt("Aborting.");
                break;
        }
    }

    // TODO Fix this method. Needs tests!
    private void CenterDriver(bool? flag)
    {
        var centerHandler = new ViewHandler();
        var drawingHandler = new DrawingHandler();
        while (Drawings.MoveNext())
        {
            var drawingsCurrent = Drawings.Current;
            IView currentView = null;

            switch (drawingsCurrent)
            {
                case GADrawing gaDrawing when DrawingUtils.IsValidDrawingForCenter(gaDrawing):
                {
                    if (flag == true) continue; // True == fab only, false erection only
                    if (flag is not (null or false)) continue;

                        drawingHandler.SetActiveDrawing(gaDrawing);
                    var allDrawingViews = gaDrawing.GetSheet().GetViews();
                    while (allDrawingViews.MoveNext())
                    {
                        var viewTypeDict = DrawingMethods.GetViewTypeDict((View)allDrawingViews.Current);
                        var viewTypeEnum = DrawingMethods.GetViewTypeEnum(viewTypeDict);

                        if (viewTypeEnum == GaViewType.None) continue;
                        currentView = new GaView((View)allDrawingViews.Current);
                    }

                    break;
                }
                case AssemblyDrawing assemblyDrawing when DrawingUtils.IsValidDrawingForCenter(assemblyDrawing):
                {
                        drawingHandler.SetActiveDrawing(assemblyDrawing);
                    var view = assemblyDrawing.GetSheet().GetViews();
                    while (view.MoveNext())
                    {
                        if (flag == false) continue;
                        if (flag is not (null or true)) continue;
                        if (view.Current.GetView().IsSheet) continue;

                        var fabView = new FabView((View)view.Current);
                        var viewTypeEnum = (GaViewType)fabView.GetViewTypeEnum(_viewHandler);
                        if (viewTypeEnum == GaViewType.None) continue;
                        currentView = fabView;
                    }

                    break;
                }
            }

            //DrawingUtils.FinalizeDrawing(currentView?.Center(centerHandler)!);
        }
    }
    // TODO fix this method. Needs tests!
    public void CenterDriver()
    {
        var centerHandler = new ViewHandler();
        while (Drawings.MoveNext())
        {
            var drawingsCurrent = Drawings.Current;

            switch (drawingsCurrent)
            {
                case GADrawing gaDrawing when DrawingUtils.IsValidDrawingForCenter(gaDrawing):
                {
                    _drawingHandler.SetActiveDrawing(gaDrawing, false);
                    var allDrawingViews = _drawingHandler.GetActiveDrawing().GetSheet().GetViews();
                    Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple = null;
                    while (allDrawingViews.MoveNext())
                    {
                        var viewTypeDict = DrawingMethods.GetViewTypeDict((View)allDrawingViews.Current);
                        var viewTypeEnum = DrawingMethods.GetViewTypeEnum(viewTypeDict);

                        if (viewTypeEnum == GaViewType.None) continue;
                        IView currentView = new GaView((View)allDrawingViews.Current);
                        drawingTuple = currentView.Center(centerHandler);
                        //DrawingUtils.FinalizeDrawing(currentView.Center(centerHandler));
                    }
                    // TODO fix this
                    DrawingUtils.FinalizeDrawing(drawingTuple);


                    break;
                }
                case AssemblyDrawing assemblyDrawing when DrawingUtils.IsValidDrawingForCenter(assemblyDrawing):
                {
                    _drawingHandler.SetActiveDrawing(assemblyDrawing, false);
                    var view = assemblyDrawing.GetSheet().GetViews();
                    while (view.MoveNext())
                    {
                        if (view.Current.GetView().IsSheet) continue;
                        var fabView = new FabView((View)view.Current);
                        var viewTypeEnum = (GaViewType)fabView.GetViewTypeEnum(_viewHandler);
                        if (viewTypeEnum == GaViewType.None) continue;
                        IView currentView = fabView;
                        currentView.Center(centerHandler);
                    }

                    break;
                }
            }

        }
    }
}
