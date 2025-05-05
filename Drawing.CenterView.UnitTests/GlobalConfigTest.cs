using System;
using Drawing.CenterView.Library;
using NUnit.Framework;

namespace Drawing.CenterView.UnitTests;

[TestFixture]
[TestOf(typeof(GlobalConfig))]
public class GlobalConfigTest
{

    [Test]
    public void Singleton_Instance()
    {
        var x = GlobalConfig.Instance;
        Assert.IsNotNull(x,  "GlobalConfig should not be null.");
    }

    [Test]
    public void InitializeDrawingSet_Throw()
    {
        Assert.IsNotNull(GlobalConfig.Instance.GetDrawingSet(), "Drawing set should not be null");
    }
}