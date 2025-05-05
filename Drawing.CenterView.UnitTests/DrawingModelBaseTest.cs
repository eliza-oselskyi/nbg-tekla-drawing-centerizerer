using System;
using System.Collections.Generic;
using Drawing.CenterView.Library;
using NUnit.Framework;
using Tekla.Structures.Drawing;

namespace Drawing.CenterView.UnitTests;

[TestFixture]
[TestOf(typeof(DrawingModelBase))]
public class DrawingModelBaseTest : DrawingModelBase
{
    public DrawingModelBaseTest() : base(new GADrawing())
    {
    }

    [Test]
    public void GetValidViews_Throw()
    {
        Assert.DoesNotThrow((GetValidViews), "Create Implementation");
    }

    [Test]
    public void SetExcluded_Throw()
    {
        Assert.DoesNotThrow((SetExcluded), "Create Implementation");
    }
    
    // [Test]
    // public void IsValidForCenter_Throw()
    // {
    //     Assert.DoesNotThrow((IsValidForCenter), "Create Implementation");
    // }
    
    [Test]
    public void Center_Throw()
    {
        Assert.DoesNotThrow((Center), "Create Implementation");
    }

    public override void FilterValid(List<DrawingModelBase> drawings)
    {
    }
}