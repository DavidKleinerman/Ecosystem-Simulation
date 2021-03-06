using Godot;
using System;

public class SettingsMenu : Control
{
	public bool enableVSync=false;
	public int Multiplier=1;
	public bool flag = false;
	Vector2 Resolution = (Vector2) new Vector2(1920,1080);
	//public MSAA DispFix;
	public int Aliasing = 0;
	public bool enableShadows=false;
	string path = "res://scripts/menus code/SettingsData/SettingsMenu.txt";
	public override void _Ready()
	{
		//screenBorder = GetParent().GetNode<Global>("Global");
		this.Visible = true;
		SelectSettingsMenuItems();
		GetViewport().Size = Global.Resolution;
		
	}
	private void SelectSettingsMenuItems(){
		if(Global.enableShadows == true){
			this.enableShadows = true;
			GetNode<Godot.ItemList>("ShadowQuality").Select(0);
		}
		else if(Global.enableShadows == false){
			this.enableShadows = false;
			GetNode<Godot.ItemList>("ShadowQuality").Select(1);
		}
		if(Global.antiAliasing == 0){
			this.Aliasing = 0;
			GetNode<Godot.ItemList>("ItemList5").Select(0);
		}
		else if(Global.antiAliasing == 2){
			this.Aliasing = 2;
			GetNode<Godot.ItemList>("ItemList5").Select(1);
		}
		else if(Global.antiAliasing == 4){
			this.Aliasing = 4;
			GetNode<Godot.ItemList>("ItemList5").Select(2);
		}
		if(Global.Multiplier == 1){
			this.Multiplier = 1;
			GetNode<Godot.ItemList>("ItemList2").Select(0);
		}
		else if(Global.Multiplier == 2){
			this.Multiplier = 2;
			GetNode<Godot.ItemList>("ItemList2").Select(1);
		}
		else if(Global.Multiplier == 4){
			this.Multiplier = 4;
			GetNode<Godot.ItemList>("ItemList2").Select(2);
		}
		if(Global.borderlessWindow == false){
			this.flag = false;
			GetNode<Godot.ItemList>("ItemList").Select(0);
		}
		else if(Global.borderlessWindow == true){
			this.flag = true;
			GetNode<Godot.ItemList>("ItemList").Select(1);
		}
		if(Global.Resolution.ToString() == "(640, 480)"){
			this.Resolution = Global.Resolution;
			GetNode<Godot.ItemList>("ItemList3").Select(2);
		}
		else if(Global.Resolution.ToString() == "(1280, 720)"){
			this.Resolution = Global.Resolution;
			GetNode<Godot.ItemList>("ItemList3").Select(1);
		}
		else if(Global.Resolution.ToString() == "(1920, 1080)"){
			this.Resolution = Global.Resolution;
			GetNode<Godot.ItemList>("ItemList3").Select(0);
		}
		if(Global.enableVSync == true){
			this.enableVSync = true;
			GetNode<Godot.ItemList>("ItemList4").Select(0);
		}
		else if(Global.enableVSync == false){
			this.enableVSync = false;
			GetNode<Godot.ItemList>("ItemList4").Select(1);
		}
		
	}
	private void _on_ItemList_item_selected(int index){
		
		GD.Print(index, "\n");
		if(index == 1){
			this.flag=true;
			//OS.SetBorderlessWindow(true);
			//OS.SetWindowSize(OS.GetScreenSize());
			
		}
		else if(index == 0){
			this.flag = false;
			//OS.SetBorderlessWindow(false);
		}
	}
	private void _on_ItemList4_item_selected(int index)
	{
		
		GD.Print(index,"\n");
		if(index == 0){
			//Global.enableVSync = true;
			this.enableVSync = true;
			//OS.SetUseVsync(enableVSync);
		}
		else if(index == 1){
			//Global.enableVSync=false;
			this.enableVSync = false;
			//OS.SetUseVsync(enableVSync);
		}
	}
	private void _on_ItemList2_item_selected(int index)
	{
		this.Visible = true;
		if(index == 0){
			//Global.Multiplier = 1;
			this.Multiplier = 1;
		}
		else if(index == 1){
			//Global.Multiplier = 2;
			this.Multiplier = 2;
		}
		else if(index == 2){
			//Global.Multiplier = 4;
			this.Multiplier = 4;
		}
	}
	private void _on_SaveChanges_pressed()
	{
		if(this.flag == false){
			//OS.SetBorderlessWindow(false);
			Global.borderlessWindow = this.flag;
		}
		else if(this.flag == true){
			Global.borderlessWindow = this.flag;
			//OS.SetBorderlessWindow(true);
			//OS.SetWindowSize(OS.GetScreenSize());
		}
		Global.Multiplier = this.Multiplier;
		Global.enableVSync = this.enableVSync;
		Global.Resolution = this.Resolution;
		GetViewport().Size = Resolution;
		//OS.Alert("This is your message", "Message Title");
		if(this.Aliasing == 0){
			GetViewport().SetMsaa(0);
			Global.antiAliasing = this.Aliasing;
		}
		else if(this.Aliasing == 2){
			GetViewport().SetMsaa(Viewport.MSAA.Msaa2x);
			Global.antiAliasing = this.Aliasing;
		}
		else if(this.Aliasing == 4){
			GetViewport().SetMsaa(Viewport.MSAA.Msaa4x);
			Global.antiAliasing = this.Aliasing;
		}
		Global.enableShadows = this.enableShadows;
		OS.WindowFullscreen = this.flag;
		// OS.SetBorderlessWindow(this.flag);
		// if (flag)
		// 	OS.SetWindowSize(OS.GetScreenSize());
		writeFile(path);
		
		
		
	}
	
	
	private void _on_ItemList3_item_selected(int index)
	{
		if(index == 0){
			this.Resolution = new Vector2(1920,1080);
		}
		else if(index == 1){
			this.Resolution = new Vector2(1280,720);
		}
		else if(index == 2){
			this.Resolution = new Vector2(640,480);
		}
	}
	private void _on_ItemList5_item_selected(int index)
	{
		if(index == 0){
			this.Aliasing = 0;
		}
		else if(index == 1){
			this.Aliasing = 2;
		}
		else if(index == 2){
			this.Aliasing = 4;
		}
	}
	private void _on_ShadowQuality_item_selected(int index)
	{
		if(index == 0){
			this.enableShadows = true;
		}
		else if(index == 1){
			this.enableShadows = false;
		}
	}

	private void writeFile(string path)
	{
		File f = new File();
		f.Open(path,File.ModeFlags.Write);
		//f.SeekEnd();
		f.StoreLine(Global.enableShadows.ToString());
		f.StoreLine(Global.antiAliasing.ToString());
		f.StoreLine(Global.Multiplier.ToString());
		f.StoreLine(Global.borderlessWindow.ToString());
		f.StoreLine(Global.Resolution.ToString());
		f.StoreLine(Global.enableVSync.ToString());
		f.Close();
	}
	
}
