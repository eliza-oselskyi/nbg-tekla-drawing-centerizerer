using System;
using System.Collections.Generic;
using Drawing.CenterView.Views;
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
            return $@"Nothing To Do. {view.GetDrawing().Name} => {(GaViewType)viewType}";
        }
        else if (Math.Abs(view.ExtremaCenter.X - sheetWidth) > 0.0001 ||
                 Math.Abs(view.ExtremaCenter.Y - sheetHeight) > 0.0001)
        {
            view.Origin.X += xOffset;
            view.Origin.Y += yOffset;
            view.Modify();
            s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "C");

            return $"Centering {view.GetDrawing().Name} => {(GaViewType)viewType}";
        }

        s = new Tuple<Tekla.Structures.Drawing.Drawing, string>(view.GetDrawing(), "X");
        return $"Something Went Wrong At {view.GetDrawing().Name} => " + (GaViewType)viewType;
    }

    internal static Dictionary<string, string> GetViewTypeDict(ViewBase view)
    {
        view.GetStringUserProperties(new List<string>() { "ViewType" }, out var viewType);
        return viewType;
    }
        public static GaViewType GetViewTypeEnum(Dictionary<string, string> viewType)
        {
            try
            {
                viewType.TryGetValue("ViewType", out var vt);
                return vt switch
                {
                    "Cover Sheet" => GaViewType.CoverSheet,
                    "Building Sheet 1" => GaViewType.BuildingSheet1,
                    "Building Sheet 2" => GaViewType.BuildingSheet2,
                    "Plane Identification Plan" => GaViewType.PlaneIdentificationPlan,
                    "Anchor Rod Plan" => GaViewType.AnchorRodPlan,
                    "Base Plate Details" => GaViewType.BasePlateDetails,
                    "Reactions" => GaViewType.Reactions,
                    "Shakeout Plan" => GaViewType.ShakeoutPlan,
                    "Roof Framed Opening Plan" => GaViewType.RoofFramedOpeningPlan,
                    "Crane Beam Plan" => GaViewType.CraneBeamPlan,
                    "Cross Section" => GaViewType.CrossSection,
                    "Portal Cross Section" => GaViewType.PortalCrossSection,
                    "Roof Framing Plan" => GaViewType.RoofFramingPlan,
                    "Roof Framing Plan - Secondary" => GaViewType.RoofFramingPlanSecondary,
                    "Roof Framing Plan - Openings" => GaViewType.RoofFramingPlanOpenings,
                    "Roof Framing Plan - Purlin Bracing" => GaViewType.RoofFramingPlanPurlinBracing,
                    "Endwall Framing" => GaViewType.EndwallFraming,
                    "Endwall Partition Framing" => GaViewType.EndwallPartitionFraming,
                    "Sidewall Framing" => GaViewType.SidewallFraming,
                    "Sidewall Partition Framing" => GaViewType.SidewallPartitionFraming,
                    "Wall Sheeting" => GaViewType.WallSheeting,
                    "Partition Wall Sheeting" => GaViewType.PartitionWallSheeting,
                    "Roof Sheeting" => GaViewType.RoofSheeting,
                    "Wall Liner" => GaViewType.WallLiner,
                    "Partition Wall Liner" => GaViewType.PartitionWallLiner,
                    "Roof Liner" => GaViewType.RoofLiner,
                    "Roof Panel Clip Layout" => GaViewType.RoofPanelClipLayout,
                    "Mezzanine Plan - Framing Only" => GaViewType.MezzaninePlanFramingOnly,
                    "Mezzanine Plan - Joists Only" => GaViewType.MezzaninePlanJoistsOnly,
                    "Mezzanine Plan - Decking" => GaViewType.MezzaninePlanDecking,
                    _ => GaViewType.None
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return GaViewType.None;
            }
        }
}