using System;
using System.Collections.Generic;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView.Views;
public class FabView : IView
{
    public FabView(View view)
    {
        View = view;
    }

    public View View { get; set; }

    // TODO refactor out this stupid tuple!
    public Tuple<Tekla.Structures.Drawing.Drawing, string> Center(IViewVisitor visitor)
    {
        visitor.CenterVisit(this);
        return new Tuple<Tekla.Structures.Drawing.Drawing, string>(new GADrawing(), "");
    }

    // TODO remove method maybe
    public bool IsValidViewForCenter(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    // TODO remove method
    public Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnum(IViewVisitor visitor)
    {
        return visitor.GetViewTypeEnumVisit(this);
    }
}

public class GaView : IView
{
    public GaView(View view)
    {
        View = view;
    }

    public View View { get; set; }

    // TODO refactor out this stupid tuple!
    public Tuple<Tekla.Structures.Drawing.Drawing, string> Center(IViewVisitor visitor)
    {
        var drawingTuple = new Tuple<Tekla.Structures.Drawing.Drawing, string>(new GADrawing(), "");
        visitor.CenterVisit(this, ref drawingTuple);
        return  drawingTuple;
    }

    // TODO maybe don't need this method
    public bool IsValidViewForCenter(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    // TODO remove method
    public Dictionary<string, string> GetViewTypeDict(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public Enum GetViewTypeEnum(IViewVisitor visitor)
    {
        throw new NotImplementedException();
    }
}
