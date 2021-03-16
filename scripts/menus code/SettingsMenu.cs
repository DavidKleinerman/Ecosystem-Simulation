using Godot;
using System;

public class SettingsMenu : Control
{
	public bool enableVSync;
	public int Multiplier=1;
	public bool flag = false;
	Vector2 Resolution = (Vector2) new Vector2(1920,1080);
	//public MSAA DispFix;
	public int Aliasing = 0;
	
	public override void _Ready()
	{
		//screenBorder = GetParent().GetNode<Global>("Global");
		this.Visible = true;
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
			OS.SetBorderlessWindow(false);
		}
		else if(this.flag == true){
			OS.SetBorderlessWindow(true);
			OS.SetWindowSize(OS.GetScreenSize());
		}
		Global.Multiplier = this.Multiplier;
		Global.enableVSync = this.enableVSync;
		Global.Resolution = this.Resolution;
		GetViewport().Size = Resolution;
		//OS.Alert("This is your message", "Message Title");
		if(this.Aliasing == 0){
			GetViewport().SetMsaa(0);
		}
		else if(this.Aliasing == 2){
			GetViewport().SetMsaa(Viewport.MSAA.Msaa2x);
		}
		else if(this.Aliasing == 4){
			GetViewport().SetMsaa(Viewport.MSAA.Msaa4x);
		}
		
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
}
