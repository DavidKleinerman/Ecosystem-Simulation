using Godot;
using System;

public class Global : Node
{
	public static bool enableVSync = true;
	public static int Multiplier = 1;
	public static Vector2 Resolution;
	public static bool enableShadows = false;
	//public static int A = 0;
	public static int worldSize = 32;
	public static float biomeGrowthRate = 3;
	public static int biomeType = 0;
	public static int antiAliasing = 0;
	public static bool borderlessWindow = false;
}
