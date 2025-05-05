using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using TSMO = Tekla.Structures.Model.Operations;
using View = Tekla.Structures.Drawing.View;

namespace Drawing.CenterView.Views;

public class TestProgram
{
    private static readonly DrawingHandler DrawingHandler = new DrawingHandler();
    private static readonly DrawingSelector DrawingSelector = DrawingHandler.GetDrawingSelector();

    public static void TestEntryPoint()
    {
        var selectedDrawings = DrawingSelector.GetSelected();

        if (selectedDrawings.GetSize() <= 0)
        {
            PromptNoneSelected();
        }
        else
        {
            PromptSelected();
        }
    }

    private static void PromptSelected()
    {
        Console.WriteLine("Drawings selected prompt");
        var drawingHandler = new DrawingHandlerExtension(DrawingSelector.GetSelected());
        var stopWatch = new Stopwatch();
        
            const string boxTitle = "Center All Drawings?";
            const string boxQuestion = "Should ONLY the selected drawings be centered?\n\n" +
                                       "  Yes = Center only selected\n" +
                                       "  No = Center all";
            var boxResult = MessageBox.Show(boxQuestion,
                boxTitle,
                MessageBoxButtons.YesNoCancel,
                0,
                0,
                MessageBoxOptions.DefaultDesktopOnly);

            switch (boxResult)
            {
                case DialogResult.Yes:
                    stopWatch.Start();
                    drawingHandler.CenterDriver();
                    break;
                case DialogResult.No:
                    stopWatch.Start();
                    PromptNoneSelected();
                    break;
                case DialogResult.Cancel:
                    Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Aborting.");
                    return;
                case DialogResult.None:
                case DialogResult.OK:
                case DialogResult.Abort:
                case DialogResult.Retry:
                case DialogResult.Ignore:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }  

    private static void PromptNoneSelected()
    {
        Console.WriteLine("Drawings none selected prompt");
        
        var drawingHandler = new DrawingHandlerExtension(DrawingHandler.GetDrawings());
        var stopWatch = new Stopwatch();
        
        DialogResult result =CustomMessageBox.ShowPrompt();
        drawingHandler.CenterDriver(result);
    }
}

public class ViewHandler : IViewVisitor
{
    public void CenterVisit(FabView view)
    {
        Console.WriteLine(@"Centering FabView");
    }

    // TODO needs tests! Need to refactor out the tuple construct that's really annoying.
    public void CenterVisit(GaView view, ref Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple)
    {
        var currentView = view.View.GetView();
        var dict = DrawingMethods.GetViewTypeDict(currentView);
        var viewTypeEnum = DrawingMethods.GetViewTypeEnum(dict);
        var reportStringBuilder = new StringBuilder();
        try
        {
            switch (currentView.GetDrawing().Title3)
                {
                case "X":
                    //    TSMO.Operation.DisplayPrompt(
                    //       $@"({counter}/{total}) Skipping {currentView.GetDrawing().Name}.");
                    TSMO.Operation.DisplayPrompt("SKIPPING TEST");
                    break;
                default:
                    if (viewTypeEnum != GaViewType.None)
                    {
                        var reportString = DrawingMethods.CenterView(currentView, (int)viewTypeEnum,
                            out var dt);
                        drawingTuple = dt;
                        reportStringBuilder.AppendLine(reportString);
                        //TSMO.Operation.DisplayPrompt($@"({counter}/{total}) " + reportString);
                        //counter++;
                        DrawingUtils.RenameDrawingTitle3FromTuple(dt);
                    }

                    break;
            }
        }
        catch (Exception e) when (e is KeyNotFoundException)
        {
            TSMO.Operation.DisplayPrompt(@"Invalid View: " +
                                         currentView.ToString());
        }
        //Console.WriteLine(@"Centering GaView");
    }

    // TODO do I even need this method in here?
    public bool IsValidViewForCenterVisit(GaView view)
    {
        var drawing = view.View.GetDrawing();
        var memberCount = 0;
        var views = drawing.GetSheet().GetViews();

        while (views.MoveNext())
        {
            var viewTypeDict = DrawingMethods.GetViewTypeDict(view.View.GetView());
            var type = DrawingMethods.GetViewTypeEnum(viewTypeDict);
            if (type is not GaViewType.None) memberCount++;
        }

        return memberCount == 1; // valid if memberCount is 1
    }

    // TODO do I even need this method in here?
    bool IViewVisitor.IsValidViewForCenterVisit(FabView view)
    {
        var drawing = view.View.GetDrawing();
        var memberCount = 0;
        var views = drawing.GetSheet().GetViews();

        while (views.MoveNext())
        {
            if (views.Current is View { ViewType: not View.ViewTypes._3DView
                    and not View.ViewTypes.DetailView
                    and not View.ViewTypes.DetailView
                })
            {
                memberCount++;
            }
        }
        return memberCount == 1;
    }

    public Enum GetViewTypeEnumVisit(FabView view)
    {
        return GaViewType.Assembly;
    }
}
