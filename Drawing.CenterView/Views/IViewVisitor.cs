using System;
using System.Collections.Generic;

namespace Drawing.CenterView.Views;
public interface IViewVisitor
{
    public void CenterVisit(FabView view);
    public void CenterVisit(GaView view, ref Tuple<Tekla.Structures.Drawing.Drawing, string> drawingTuple);
    public bool IsValidViewForCenterVisit(GaView view);
    public bool IsValidViewForCenterVisit(FabView view);
    public Enum GetViewTypeEnumVisit(FabView view);
}
