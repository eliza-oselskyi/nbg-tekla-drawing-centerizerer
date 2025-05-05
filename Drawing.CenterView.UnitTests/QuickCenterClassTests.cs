using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;

namespace Drawing.CenterView.UnitTests;

[TestFixture("Cover Sheet", "")]
[TestFixture("Cover Sheet", "Test Value")]
[TestFixture("", "Test Value")]
[TestFixture("", "")]
public class QuickCenterClassTests
{
    private readonly GADrawing _drawing;
    private readonly View _view1;
    private readonly View _view2;

    public QuickCenterClassTests(string viewType1, string viewType2)
    {
        var size = new Size(410, 287);
        _drawing = new GADrawing("standard", size);
        //_drawing.Insert();
        _view1 = CreateView(viewType1);
        _view2 = CreateView(viewType2);
        _drawing.PlaceViews();
        //_drawing.Modify();
    }

    private View CreateView(string viewType)
    {
        var coordSys = new CoordinateSystem();
        var aabb = new AABB();
        var drawingView = new View(_drawing.GetSheet(), coordSys, coordSys, aabb);
        drawingView.Insert();
        drawingView.SetUserProperty("ViewType", viewType);
        drawingView.Modify();
        return drawingView;
    }

    private static Dictionary<string, string> GetViewTypeDict(View view)
    {
        view.GetStringUserProperties(new List<string>() { "ViewType" }, out var viewType);
        return viewType;
    }

    [Test]
    public void IsValidDrawingForCenter_OneValidView_ReturnsTrue()
    {
        var viewType1 = GetViewTypeDict(_view1);
        var viewType2 = GetViewTypeDict(_view2);

        var viewType1Enum = PluginForm.GetViewTypeEnum(viewType1);
        var viewType2Enum = PluginForm.GetViewTypeEnum(viewType2);

        if ((viewType1Enum != PluginForm.ViewType.None && viewType2Enum == PluginForm.ViewType.None) ||
            (viewType1Enum == PluginForm.ViewType.None && viewType2Enum != PluginForm.ViewType.None))
            Assert.That(DrawingUtils.IsValidDrawingForCenter(_drawing), Is.True, "Should be a valid drawing");
        else
            Assert.That(DrawingUtils.IsValidDrawingForCenter(_drawing), Is.False, "Should not be a valid drawing");
    }
}