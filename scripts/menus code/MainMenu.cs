using Godot;
using System;

public class MainMenu : Control
{
	public int countLine =0;
	public string path = "res://scripts/menus code/SettingsData/SettingsMenu";
	public string getLine; 
	public bool enableShadows;
	public bool enableVSync;
	public bool borderlessWindow;
	public int Multiplier;
	public int antiAliasing;
	public Vector2 resolution;
	public override void _Ready()
	{
		this.Visible = true;
		readFile(path);
	}
	private void readFile(string path){
		File f = new File();
		f.Open(path,File.ModeFlags.Read);
		GD.Print("File Opened to read data!\n");
		getLine = f.GetLine();
		GD.Print(getLine);
		if(getLine == "True"){
			Global.enableShadows = true;
			this.enableShadows = true;
		}
		else if(getLine == "False"){
			Global.enableShadows = false;
			this.enableShadows = false;
		}
		getLine = f.GetLine();
		GD.Print(getLine);
		if(getLine == "0"){
			GetViewport().SetMsaa(0);
			Global.antiAliasing = 0;
			this.antiAliasing = 0;
		}
		else if(getLine == "2"){
			GetViewport().SetMsaa(Viewport.MSAA.Msaa2x);
			Global.antiAliasing = 2;
			this.antiAliasing = 2;
		}
		else if(getLine == "4"){
			GetViewport().SetMsaa(Viewport.MSAA.Msaa4x);
			Global.antiAliasing = 4;
			this.antiAliasing = 4;
		}
		getLine = f.GetLine();
		GD.Print(getLine);
		if(getLine == "1"){
			Global.Multiplier = 1;
			this.Multiplier = 1;
		}
		else if(getLine == "2"){
			Global.Multiplier = 2;
			this.Multiplier = 2;
		}
		else if(getLine == "4"){
			Global.Multiplier = 4;
			this.Multiplier = 4;
		}
		getLine = f.GetLine();
		GD.Print(getLine);
		if(getLine == "True"){
			Global.borderlessWindow = true;
			this.borderlessWindow = true;
			//OS.SetBorderlessWindow(this.borderlessWindow);
			//OS.SetWindowSize(OS.GetScreenSize());
		}
		else if(getLine == "False"){
			Global.borderlessWindow = false;
			this.borderlessWindow = false;
			//OS.SetBorderlessWindow(this.borderlessWindow);
		}
		getLine = f.GetLine();
		GD.Print(getLine);
		if(getLine == "(640, 480)"){
			this.resolution = new Vector2(640,480);
			Global.Resolution = this.resolution;
			
		}
		else if(getLine == "(1280, 720)"){
			this.resolution = new Vector2(1280,720);
			Global.Resolution = this.resolution;
		}
		else if(getLine == "(1920, 1080)"){
			this.resolution = new Vector2(1920,1080);
			Global.Resolution = this.resolution;
		}
		GetViewport().Size = resolution;
		getLine = f.GetLine();
		GD.Print(getLine);
		if(getLine == "False"){
			Global.enableVSync = false;
			this.enableVSync = false;
		}
		else if(getLine == "True"){
			Global.enableVSync = true;
			this.enableVSync = true;
		}
		//OS.SetBorderlessWindow(this.borderlessWindow);
		OS.WindowFullscreen = this.borderlessWindow;
		
		// if(borderlessWindow)
		// 	OS.SetWindowSize(OS.GetScreenSize());
		f.Close();
	}
	/*private void readFile2(string path)
	{
		File f = new File();
		f.Open(path,File.ModeFlags.Read);
		GD.Print("File Opened\n");
		int cnt =0;
		while(!f.EofReached()){
			GD.Print(cnt,"\n");
			cnt++;
			switch(countLine){
				case 0:
					getLine = f.GetLine();
					GD.Print(getLine);
					if(getLine == "True"){
						Global.enableShadows = true;
						this.enableShadows = true;
					}
					else if(getLine == "False"){
						Global.enableShadows = false;
						this.enableShadows = false;
					}
					break;

				case 1:
					getLine = f.GetLine();
					GD.Print(getLine);
					if(getLine == "0"){
						Global.antiAliasing = 0;
						this.antiAliasing = 0;
					}
					else if(getLine == "2"){
						Global.antiAliasing = 2;
						this.antiAliasing = 2;
					}
					else if(getLine == "4"){
						Global.antiAliasing = 4;
						this.antiAliasing = 4;
					}
					break;

				case 2:
					getLine = f.GetLine();
					GD.Print(getLine);
					if(getLine == "1"){
						Global.Multiplier = 1;
						this.Multiplier = 1;
					}
					else if(getLine == "2"){
						Global.Multiplier = 2;
						this.Multiplier = 2;
					}
					else if(getLine == "4"){
						Global.Multiplier = 4;
						this.Multiplier = 4;
					}
					break;

				case 3:
					getLine = f.GetLine();
					GD.Print(getLine);
					if(getLine == "True"){
						Global.borderlessWindow = true;
						this.borderlessWindow = true;
					}
					else if(getLine == "False"){
						Global.borderlessWindow = false;
						this.borderlessWindow = false;
					}
					break;

				case 4:
					getLine = f.GetLine();
					GD.Print(getLine);
					if(getLine == "(640, 480)"){
						this.resolution = new Vector2(640,480);
						Global.Resolution = this.resolution;
					}
					else if(getLine == "(1280, 720)"){
						this.resolution = new Vector2(1280,720);
						Global.Resolution = this.resolution;
					}
					else if(getLine == "(1920, 1080)"){
						this.resolution = new Vector2(1920,1080);
						Global.Resolution = this.resolution;
					}
					break;

				case 5:
					getLine = f.GetLine();
					GD.Print(getLine);
					if(getLine == "False"){
						Global.enableVSync = false;
						this.enableVSync = false;
					}
					else if(getLine == "True"){
						Global.enableVSync = true;
						this.enableVSync = true;
					}
					break;
				
				default:
					
					GD.Print("Error occured during settings scan\n");
					break;
			}
			countLine ++;
		}
		f.Close();	
	}*/

	private void _on_LoadSimButton_pressed()
	{
		GetNode<FileDialog>("FileDialog").Popup_();
		GetNode<Button>("NewSimButton").Visible = false;
		GetNode<Button>("LoadSimButton").Visible = false;
		GetNode<Button>("SettingButton").Visible = false;
		GetNode<Button>("ExitButton").Visible = false;
	}

	private void _on_FileDialog_popup_hide()
	{
		GetNode<Button>("NewSimButton").Visible = true;
		GetNode<Button>("LoadSimButton").Visible = true;
		GetNode<Button>("SettingButton").Visible = true;
		GetNode<Button>("ExitButton").Visible = true;
	}

}






