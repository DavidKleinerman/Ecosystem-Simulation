using Godot;
using System;

public class TestTheTest : WAT.Test
{
    public override String Title()
	{
		return "Given an Equality Assertion";
	}
	
	[Test]
	public void WhenCallingIsEqual() 
	{ 
		Assert.IsEqual(1, 2, "Then it passes"); 
	}

    [Test]
	public void WhenCallingIsGreaterThan() 
	{ 
		Assert.IsGreaterThan(2, 1, "ok");
	}
}
