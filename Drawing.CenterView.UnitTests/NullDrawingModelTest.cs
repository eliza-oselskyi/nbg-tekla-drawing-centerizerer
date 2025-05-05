using System;
using System.Diagnostics;
using Drawing.CenterView.Library;
using NUnit.Framework;

namespace Drawing.CenterView.UnitTests;

[TestFixture]
[TestOf(typeof(NullDrawingModel))]
public class NullDrawingModelTest
{

    [Test]
    public void Singleton_Instance()
    {
        var x  = NullDrawingModel.Instance;
        Assert.That(x.Equals(null) == false, Is.True, "Singleton instance returns false");
    }
}