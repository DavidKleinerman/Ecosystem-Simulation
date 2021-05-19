using Godot;
using System;

public class SpeciesHolderTests : WAT.Test
{
    public override String Title()
	{
		return "SpeciesHolder Unit Tests";
	}
    [Test]
	public void _ProcessConstDeltaOneCallTest() 
	{
        float X = 0.05f;
        SpeciesHolder sh = new SpeciesHolder();
        sh.MockSimulationStarted();
        sh._Process(X);
        Assert.IsEqual(X, sh.GetCurrentWaitingTime(), "Then it passes.");

	}
    [Test]
	public void _ProcessConstDeltaMultipleCallsTest() 
	{
        float X = 0.05f;
        int Y = 3;
        SpeciesHolder sh = new SpeciesHolder();
        sh.MockSimulationStarted();
        for (int i = 0; i < Y; i++)
            sh._Process(X);
        Assert.IsEqual(0.05f + 0.05f + 0.05f, sh.GetCurrentWaitingTime(), "Then it passes.");

	}
    [Test]
	public void _ProcessVaryingDeltaMultipleCallsTest() 
	{
        float X = 0.05f;
        int Y = 3;
        SpeciesHolder sh = new SpeciesHolder();
        sh.MockSimulationStarted();
        for (int i = 0; i < Y; i++){
            sh._Process(X);
            X += 0.01f;
        }
        Assert.IsEqual(0.05f + 0.06f + 0.07f, sh.GetCurrentWaitingTime(), "Then it passes.");

	}
}
