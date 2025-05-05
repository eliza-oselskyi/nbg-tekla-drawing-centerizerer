using System;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView;

public static class DrawingMethods
{
    public static string CenterView(ViewBase view, int viewType, out Tuple<Tekla.Structures.Drawing.Drawing, string> s)
    {
        var sheet = view.GetDrawing().GetSheet();
        double sheetHeightOffset = 0;
        switch (viewType)
        {
            case 1:
                sheetHeightOffset = 25.4; // 1"
                break;
            case >= 2 and <= 24:
                sheetHeightOffset = 22.225; // 7/8"
                break;
            default: Tekla.Structures.Model.Operations.Operation.DisplayPrompt(viewType.ToString()); break;
        }

        sheet.Origin.Y = sheetHeightOffset;
        var originalOriginX = view.Origin.X;
        var originalOriginY = view.Origin.Y;
        view.Origin = sheet.Origin;

        var viewCenterPoint = view.GetAxisAlignedBoundingBox().GetCenterPoint();

        var sheetHeight = sheet.Height / 2;
        var sheetWidth = (sheet.Width - 33.274) / 2;
        var xOffset = sheetWidth - viewCenterPoint.X;
        var yOffset = sheetHeight - viewCenterPoint.Y;

        if (Math.Abs(originalOriginX - xOffset) < 0.0001 &&
            Math.Abs(originalOriginY - yOffset - sheetHeightOffset) < 0.0001)
        {
            s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "NC");
            return $@"Nothing To Do. {view.GetDrawing().Name} => {(ViewType)viewType}";
        }
        else if (Math.Abs(view.ExtremaCenter.X - sheetWidth) > 0.0001 ||
                 Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001)
        {
            view.Origin.X += xOffset;
            view.Origin.Y += yOffset;
            view.Modify();
            s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "C");

            return $"Centering {view.GetDrawing().Name} => {(ViewType)viewType}";
        }

        s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "X");
        return $"Something Went Wrong At {view.GetDrawing().Name} => " + (ViewType)viewType;
    }
}