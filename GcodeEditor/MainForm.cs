/*
 * Created by SharpDevelop.
 * User: linea
 * Date: 10/26/2020
 * Time: 6:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace GcodeEditor
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			
			InitializeComponent();
			groupBox8.Visible=false;
			
			g=panel1.CreateGraphics();
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			pen =new Pen(Color.Blue,5);
			pen.StartCap=pen.EndCap=System.Drawing.Drawing2D.LineCap.Round;
			
			
			try{
				HeaderA=File.ReadAllText(@"C:\Program Files\GcodeEditor\headerA.txt");
			}catch{MessageBox.Show("Error reading headerA file!");}
			try{
				HeaderB=File.ReadAllText(@"C:\Program Files\GcodeEditor\headerB.txt");
			}catch{MessageBox.Show("Error reading headerB file!");}
			try{
				Footer=File.ReadAllText(@"C:\Program Files\GcodeEditor\footer.txt");
			}catch{MessageBox.Show("Error reading footer file!");}
			try{
				hTool1=File.ReadAllText(@"C:\Program Files\GcodeEditor\tool1.txt");
			}catch{MessageBox.Show("Error reading tool 1 file!");}
			try{
				hTool2=File.ReadAllText(@"C:\Program Files\GcodeEditor\tool2.txt");
			}catch{MessageBox.Show("Error reading tool 2 file!");}
			try{
				hTool3=File.ReadAllText(@"C:\Program Files\GcodeEditor\tool3.txt");
			}catch{MessageBox.Show("Error reading tool 3 file!");}
			try{
				hTool4=File.ReadAllText(@"C:\Program Files\GcodeEditor\tool4.txt");
			}catch{MessageBox.Show("Error reading tool 4 file!");}
			
			openSettings("settings");
			
			
			table=File.ReadAllLines(@"C:\Program Files\GcodeEditor\table.txt");
			tableX=Convert.ToDouble(table[0]);
			tableY=Convert.ToDouble(table[1]);
			toolStripTextBox25.Text=table[0];
			toolStripTextBox26.Text=table[1];
			
			webBrowser1.ScriptErrorsSuppressed = true;
			
			timeHold=DateTime.Now.Minute;
			autosave.Start();
			
			if(panel1.Size.Height > (tableY*zoom))zoom=panel1.Size.Height/(tableX+offset)-3;
			if(panel1.Size.Width > (tableX*zoom))zoom=panel1.Size.Width/(tableX+offset)-3;
			
			//FormBorderStyle = FormBorderStyle.None;
			WindowState = FormWindowState.Maximized;
			process.Start();
			
		}
		
		public static class datashare
		{
			public static string toolprompt="";
		}
		
		
		Graphics g;
		Pen pen;
		int colorOrder=0;
		void Button1Click(object sender, EventArgs e)
		{
			
			
		}
		
		
		string HeaderA;
		string HeaderB;
		string hTool1;
		string hTool2;
		string hTool3;
		string hTool4;
		string Footer;
		string[] tool=new string[11];
		string[] toolsignal=new string[20];
		string[] row;
		string[] signal=new string[10];
		
		string[] t=new string[2];
		string[] x=new string[2];
		string[] y=new string[2];
		string[] z=new string[2];
		string[] i=new string[2];
		string[] j=new string[2];
		string[] k=new string[2];
		string[] f=new string[2];
		
		int[] available=new int[10];
		int[] planeXY=new int[2];
		
		int[] ToolOrder=new int[10] {0,3,2,1,4,0,0,0,0,0};

		double x1=0, y1=0, z1=0, j1=0, i1=0, k1=0, h=0, ang1=0, ang2=0, x2=0, y2=0, z2=0, i2=0, j2=0, k2=0;
		double[] fd1=new double[10]{0,0,0,0,0,0,0,0,0,0};
		double[] fm1=new double[10]{0,0,0,0,0,0,0,0,0,0};
		double f2=0;
		
		
		double xhold=0, yhold=0, zhold=0;
		double xlate=0,ylate=0,zlate=0,ilate=0,jlate=0;
		double xlater=0, ylater=0;
		double lastFeed=0;
		int lastTool=0;
		int T=0;
		
		double[] posAngle= new double[10];
		double[] posX= new double[10];
		double[] posY= new double[10];
		
		int[] instX= new int[10]{1,1,1,1,1,1,1,1,1,1};
		int[] instY= new int[10]{1,1,1,1,1,1,1,1,1,1};
		double[] dX= new double[10];
		double[] dY= new double[10];
		
		string[] table=new string[2];
		
		int passEnd=1;
		
		double offset=2;
		double zoom=1;
		
		double maxX=0;
		double maxY=0;
		double maxZ=0;
		
		string outputGcode;
		
		int humanInput=0;
		void Button2Click(object sender, EventArgs e)
		{
			
			try{
				sel=0;
				humanInput=1;
				process.Start();
				
				//if(textBox8.Text!="")
				//{
				//textBox8.BackColor=Color.White;
				//if(Convert.ToDouble(textBox8.Text)!=(-1*maxZ))MessageBox.Show("WARNING!\nGcode Z depth: "+Convert.ToString(maxZ)+" does not match ZSHIFT: "+textBox8.Text);
				//File.WriteAllText(readSaveDialog(),outputGcode);
				
				//}
				//else textBox8.BackColor=Color.Red;
			}catch{}
		}
		
		
		
		
		void Panel1Paint(object sender, PaintEventArgs e)
		{
			/*Pen blackpen = new Pen(Color.Black, 1);

			Graphics g = e.Graphics;

			g.DrawLine(blackpen, Convert.ToSingle(x1), Convert.ToSingle(y1), Convert.ToSingle(x2), Convert.ToSingle(y2));

			g.Dispose();*/
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			
			
			
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			
			
			
		}
		
		void error()
		{
			//MessageBox.Show("error file");
		}
		
		
		
		void RadioButton1CheckedChanged(object sender, EventArgs e)
		{
			
		}
		
		
		
		
		
		void Button5Click(object sender, EventArgs e)
		{
			
			if(button5.BackColor!=Color.Green)
			{
				button5.BackColor=Color.Green;
				sel=1;
			}
			else button5.BackColor=Color.Yellow;
			
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			if(button6.BackColor!=Color.Green){
				button6.BackColor=Color.Green;
				sel=2;
			}
			else button6.BackColor=Color.Yellow;
			
		}
		
		void Button7Click(object sender, EventArgs e)
		{
			if(button7.BackColor!=Color.Green){
				button7.BackColor=Color.Green;
				sel=3;
			}
			else button7.BackColor=Color.Yellow;
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			if(button8.BackColor!=Color.Green){
				button8.BackColor=Color.Green;
				sel=4;
			}
			else button8.BackColor=Color.Yellow;
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			if(button9.BackColor!=Color.Green){
				button9.BackColor=Color.Green;
				sel=5;
			}
			else button9.BackColor=Color.Yellow;
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			panel1.Refresh();
		}
		void graphicsRefresh(double xl, double yl, double zl, double il, double jl, double xx, double yy, double zz, double ii, double jj)
		{
			Color p1=Color.Blue;
			Color p2=Color.Yellow;
			
			if(sel==colorOrder)
			{
				p1=Color.Red;
				p2=Color.Orange;
			}
			if(zl<=0)
			{
				pen =new Pen(p1,Convert.ToSingle(0.05*zoom));
			}
			if(zl>0)
			{
				pen =new Pen(p2,Convert.ToSingle(0.025*zoom));
			}
			
			
			pen.StartCap=pen.EndCap=System.Drawing.Drawing2D.LineCap.Round;
			
			if(zz!=0)zlate=zz;
			if(xx!=0 && yy!=0)
			{
				
				g.DrawLine(pen,Convert.ToSingle(((xl+offset)*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-((yl+offset)*zoom)+viewOffsetY),Convert.ToSingle(((xx+offset)*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-((yy+offset)*zoom)+viewOffsetY));
				
			}
			
			
			//if(ii!=0&&jj!=0)
		//	{
				
				//double radii=Math.Sqrt((il*il)+(jl*jl));
				//g.DrawArc(pen,Convert.ToSingle(((xl+offset)*zoom)+viewOffsetX+il-radii),Convert.ToSingle(panel1.Size.Height-((yl+offset)*zoom)+viewOffsetY+jl-radii), Convert.ToSingle(2*radii*zoom), Convert.ToSingle(2*radii*zoom),Convert.ToSingle(Math.Tan(((panel1.Size.Height-((yl+offset)*zoom)+viewOffsetY+jl)-(yl*zoom))/(((xl+offset)*zoom)+viewOffsetX+il)-(xl*zoom))),Convert.ToSingle(Math.Tan(((panel1.Size.Height-((yl+offset)*zoom)+viewOffsetY+jl)-(yy*zoom))/(((xl+offset)*zoom)+viewOffsetX+il)-(xx*zoom))));
			//}
			
			
			
			if(ii!=0)il=ii;
			if(jj!=0)jl=jj;
			
			
		}
		
		string alterGcode(string gcode, int num, int xi, int yi, double xd, double yd, double zShift)
		{
			int c=0;
			string post="";
			
			row=gcode.Split('\n');
			
			double progX=0;
			double progY=0;
			xlate=0;
			ylate=0;
			zlate=0;
			ilate=0;
			jlate=0;
			
			
			for(int ym=0;ym<yi;ym++)
			{
				
				for(int xm=0;xm<xi;xm++)
				{
					
					
					
					for(int n=0;n<row.Length;n++)
					{
						signal=row[n].Split(' ');
						
						if(row[n].Contains("M")&&row[n].Contains("T"))
						{
							try{
								t=signal[c].Split('T');
								lastTool=Convert.ToInt16(t[1]);
							}catch{}
						}
						
						if((row[n].Contains("(")||row[n].Contains(")")||row[n].Contains("M")||row[n].Contains("T"))||row[n].Contains("g0")||(row[n].Contains("G")&&!(signal[0].Contains("G0")||signal[0].Contains("G00")||signal[0].Contains("G1")||signal[0].Contains("G01")||signal[0].Contains("G2")||signal[0].Contains("G02")||signal[0].Contains("G3")||signal[0].Contains("G03"))))
						{
							post+=row[n]+"\n";
							passEnd=1;
							c++;
						}
						else
						{
							if(signal[0].Contains("G0")||signal[0].Contains("G00")||signal[0].Contains("G1")||signal[0].Contains("G01")||signal[0].Contains("G2")||signal[0].Contains("G02")||signal[0].Contains("G3")||signal[0].Contains("G03"))
							{
								post+=signal[c]+" ";
								c++;
							}
							
							//insert code to test continuity-textBox1.Text+=Convert.ToString(c)+"|";
							if(row[n].Contains("X"))
							{

								try{
									x=signal[c].Split('X');
									x1=Convert.ToDouble(x[1]);
									xhold=x1;
									available[1]=1;
									c++;
									
								}catch{
									error();}
							}
							
							if(row[n].Contains("Y"))
							{
								
								try{
									y=signal[c].Split('Y');
									y1=Convert.ToDouble(y[1]);
									yhold=y1;
									available[2]=1;
									c++;
								}catch{
									error();}
							}
							
							if(((available[1]==0 && available[2]==1)||(available[1]==1 && available[2]==0)))//&&preserveLinear==1)
							{
								if(available[1]==0 && available[2]==1)
								{
									x1=xhold;
									available[1]=1;
								}
								
								if(available[1]==1 && available[2]==0)
								{
									y1=yhold;
									available[2]=1;
									
								}
								
							}
							
							
							
							if(row[n].Contains("Z"))
							{
								try{
									z=signal[c].Split('Z');
									z1=Convert.ToDouble(z[1]);
									available[3]=1;
									c++;
								}catch{error();}
								
							}
							if(row[n].Contains("I"))
							{
								
								try{
									i=signal[c].Split('I');
									i1=Convert.ToDouble(i[1]);
									available[4]=1;
									c++;
								}catch{error();}
							}
							if(row[n].Contains("J"))
							{
								try{
									j=signal[c].Split('J');
									j1=Convert.ToDouble(j[1]);
									available[5]=1;
									c++;
								}catch{error();}
								
							}
							if(row[n].Contains("K"))
							{
								try{
									k=signal[c].Split('K');
									k1=Convert.ToDouble(k[1]);
									available[6]=1;
									c++;
								}catch{error();}
							}
							if(row[n].Contains("F"))
							{
								try{
									f=signal[c].Split('F');
									lastFeed=Convert.ToDouble(f[1]);
									available[7]=1;
									c++;
								}catch{error();}
							}
							
						}
						
						//post process math HERE**************************

						
						if(available[1]==1||available[2]==1)
						{
							
							h=Math.Sqrt((x1*x1)+(y1*y1));
							
							ang1=Math.Atan2(y1,x1);
							ang2=ang1;
							if(ym==0)ang2+=(posAngle[num]*Math.PI/180);
							
							x2=Math.Cos(ang2)*h;
							y2=Math.Sin(ang2)*h;
							
							
							h=Math.Sqrt((i1*i1)+(j1*j1));
							
							ang1=Math.Atan2(j1,i1);
							ang2=ang1;
							if(ym==0)ang2+=(posAngle[num]*Math.PI/180);
							i2=Math.Cos(ang2)*h;
							j2=Math.Sin(ang2)*h;
							
							xhold=x1;
							yhold=y1;
						}
						
						
						
						
						if(ym==0){
							x2=Math.Round(x2+posX[num]+progX,5);
							y2=Math.Round(y2+posY[num]+progY,5);
						}
						else{
							x2=Math.Round(x2,5);
							y2=Math.Round(y2+progY,5);
							
						}
						
						z2=z1+zShift;
						k2=k1;
						
						z2=Math.Round(z2,5);
						i2=Math.Round(i2,5);
						j2=Math.Round(j2,5);
						k2=Math.Round(k2,5);
						
						
						if(available[7]==1 && available[3]==1 &&(z2-zhold)<(-0.0001) && fd1[lastTool]>0)
						{
							f2=fd1[lastTool];
							
						}
						else if(available[7]==1 && fm1[lastTool]>0 )
						{
							f2=fm1[lastTool];
							
						}
						else f2=lastFeed;
						
						if(available[3]==1)zhold=z2;
						
						if(x2>maxX) maxX=x2;
						if(y2>maxY) maxY=y2;
						if(z2<maxZ) maxZ=z2;
						
						//graphics handling
						if(available[1]==1) xlater=x2;
						if(available[2]==1) ylater=y2;
						
						pen.StartCap=pen.EndCap=System.Drawing.Drawing2D.LineCap.Round;
						//if((xlate!=0&&ylate!=0&&xlater!=0&&ylater!=0)&&!post.Contains("X "+Convert.ToString(xlater)+" Y "+Convert.ToString(ylater)))graphicsRefresh(xlate,ylate,zlate,ilate,jlate,xlater,ylater,z2,i2,j2);
						if(xlate!=0&&ylate!=0&&xlater!=0&&ylater!=0)graphicsRefresh(xlate,ylate,zlate,ilate,jlate,xlater,ylater,z2,i2,j2);
						
						//g.DrawLine(pen,Convert.ToSingle((xlate*zoom)+offset),Convert.ToSingle((ylate*zoom)+offset),Convert.ToSingle((xlater*zoom)+offset),Convert.ToSingle((ylater*zoom)+offset));
						
						if(available[1]==1)xlate=x2;
						if(available[2]==1)ylate=y2;
						
						
						
						
						/*richTextBox18.Text+=row[n]+"\nxl"+Convert.ToString(xlate)+"yl"+Convert.ToString(ylate)+"zl"+Convert.ToString(zlate)+"\n";
						richTextBox18.Text+="x"+Convert.ToString(x1)+"xx"+Convert.ToString(x2)+"|";
						richTextBox18.Text+="y"+Convert.ToString(y1)+"yy"+Convert.ToString(y2)+"\n_________________________\n";
						 */
						
						
						if(available[1]==1)post+="X"+Convert.ToString(x2)+" ";
						if(available[2]==1)post+="Y"+Convert.ToString(y2)+" ";
						if(available[3]==1)post+="Z"+Convert.ToString(z2)+" ";
						if(available[4]==1)post+="I"+Convert.ToString(i2)+" ";
						if(available[5]==1)post+="J"+Convert.ToString(j2)+" ";
						if(available[6]==1)post+="K"+Convert.ToString(k2)+" ";
						if(available[7]==1)post+="F"+Convert.ToString(f2)+" ";
						
						
						x1=0;
						y1=0;
						z1=0;
						j1=0;
						i1=0;
						k1=0;
						h=0;
						ang1=0;
						ang2=0;
						x2=0;
						y2=0;
						z2=0;
						i2=0;
						j2=0;
						k2=0;
						
						//finish line
						if(passEnd!=1){
							for(int p=0;c<signal.Length;c++)
							{
								try{
									post+=signal[c];
								}
								catch{}
							}
							post+="\n";
						}
						
						c=0;
						passEnd=0;
						Array.Clear(available,0,available.Length);
						Array.Clear(signal,0,signal.Length);
					}//end for loopN
					
					progX+=xd;
					if(ym!=0) xm=xi;
					
				}//end for loopX
				
				if(ym==0)
				{
					Array.Clear(row,0,row.Length);
					row=post.Split('\n');
					//richTextBox18.Text+="!";
				}
				progY+=yd;
				progX=0;
				
			}//end for loopY
			//richTextBox18.Text+="#";//" \n\n_______________________________________________________\n\n"+post;
			return post;
		}
		
		void GroupBox1Enter(object sender, EventArgs e)
		{
			
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			
		}
		string labelhold;
		string[] label= new string[10];
		void TextBox1MouseClick(object sender, MouseEventArgs e)
		{
			labelhold=readOpenDialog();
			if(labelhold!=""){
				label=labelhold.Split(Convert.ToChar(92));
				textBox1.Text=label[label.Length-1];
				paths[1]=textBox1.Text;
			}
			try{
				richTextBox1.Text=File.ReadAllText(labelhold);
				button5.BackColor=Color.Green;
				button11.PerformClick();
				
			}catch{}
		}
		
		void TextBox2MouseClick(object sender, MouseEventArgs e)
		{
			labelhold=readOpenDialog();
			if(labelhold!=""){
				label=labelhold.Split(Convert.ToChar(92));
				textBox2.Text=label[label.Length-1];
				paths[2]=textBox2.Text;
			}
			try{
				richTextBox2.Text=File.ReadAllText(labelhold);
				button6.BackColor=Color.Green;
				button12.PerformClick();
			}catch{}
			
		}
		
		void TextBox3MouseClick(object sender, MouseEventArgs e)
		{
			labelhold=readOpenDialog();
			if(labelhold!=""){
				label=labelhold.Split(Convert.ToChar(92));
				textBox3.Text=label[label.Length-1];
				paths[3]=textBox3.Text;
			}
			try{
				richTextBox3.Text=File.ReadAllText(labelhold);
				button7.BackColor=Color.Green;
				button13.PerformClick();
			}catch{}
			
		}
		
		void TextBox4MouseClick(object sender, MouseEventArgs e)
		{
			labelhold=readOpenDialog();
			if(labelhold!=""){
				label=labelhold.Split(Convert.ToChar(92));
				textBox4.Text=label[label.Length-1];
				paths[4]=textBox4.Text;
			}
			try{
				richTextBox4.Text=File.ReadAllText(labelhold);
				button8.BackColor=Color.Green;
				button14.PerformClick();
			}catch{}
			
		}
		
		void TextBox5MouseClick(object sender, MouseEventArgs e)
		{
			labelhold=readOpenDialog();
			if(labelhold!=""){
				label=labelhold.Split(Convert.ToChar(92));
				textBox5.Text=label[label.Length-1];
				paths[5]=textBox5.Text;
			}
			try{
				richTextBox5.Text=File.ReadAllText(labelhold);
				button9.BackColor=Color.Green;
				button15.PerformClick();
			}catch{}
		}
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void NumericUpDown1ValueChanged(object sender, EventArgs e)
		{
			posX[1]=Convert.ToDouble(numericUpDown1.Value);
			if(ignore==0)	processCode();
		}

		void NumericUpDown2ValueChanged(object sender, EventArgs e)
		{
			posY[1]=Convert.ToDouble(numericUpDown2.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown3ValueChanged(object sender, EventArgs e)
		{
			posAngle[1]=Convert.ToDouble(numericUpDown3.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown4ValueChanged(object sender, EventArgs e)
		{
			instX[1]=Convert.ToInt16(numericUpDown4.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown5ValueChanged(object sender, EventArgs e)
		{
			instY[1]=Convert.ToInt16(numericUpDown5.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown6ValueChanged(object sender, EventArgs e)
		{
			dX[1]=Convert.ToDouble(numericUpDown6.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown7ValueChanged(object sender, EventArgs e)
		{
			dY[1]=Convert.ToDouble(numericUpDown7.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown14ValueChanged(object sender, EventArgs e)
		{
			posX[2]=Convert.ToDouble(numericUpDown14.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown13ValueChanged(object sender, EventArgs e)
		{
			posY[2]=Convert.ToDouble(numericUpDown13.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown12ValueChanged(object sender, EventArgs e)
		{
			posAngle[2]=Convert.ToDouble(numericUpDown12.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown11ValueChanged(object sender, EventArgs e)
		{
			instX[2]=Convert.ToInt16(numericUpDown11.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown10ValueChanged(object sender, EventArgs e)
		{
			instY[2]=Convert.ToInt16(numericUpDown10.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown9ValueChanged(object sender, EventArgs e)
		{
			dX[2]=Convert.ToDouble(numericUpDown9.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown8ValueChanged(object sender, EventArgs e)
		{
			dY[2]=Convert.ToDouble(numericUpDown8.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown21ValueChanged(object sender, EventArgs e)
		{
			posX[3]=Convert.ToDouble(numericUpDown21.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown20ValueChanged(object sender, EventArgs e)
		{
			posY[3]=Convert.ToDouble(numericUpDown20.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown19ValueChanged(object sender, EventArgs e)
		{
			posAngle[3]=Convert.ToDouble(numericUpDown19.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown18ValueChanged(object sender, EventArgs e)
		{
			instX[3]=Convert.ToInt16(numericUpDown18.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown17ValueChanged(object sender, EventArgs e)
		{
			instY[3]=Convert.ToInt16(numericUpDown17.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown16ValueChanged(object sender, EventArgs e)
		{
			dX[3]=Convert.ToDouble(numericUpDown16.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown15ValueChanged(object sender, EventArgs e)
		{
			dY[3]=Convert.ToDouble(numericUpDown15.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown28ValueChanged(object sender, EventArgs e)
		{
			posX[4]=Convert.ToDouble(numericUpDown28.Value);
			if(ignore==0)	processCode();
		}
		void NumericUpDown27ValueChanged(object sender, EventArgs e)
		{
			posY[4]=Convert.ToDouble(numericUpDown27.Value);
			if(ignore==0)	processCode();
		}
		
		
		void NumericUpDown26ValueChanged(object sender, EventArgs e)
		{
			posAngle[4]=Convert.ToDouble(numericUpDown26.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown25ValueChanged(object sender, EventArgs e)
		{
			instX[4]=Convert.ToInt16(numericUpDown25.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown24ValueChanged(object sender, EventArgs e)
		{
			instY[4]=Convert.ToInt16(numericUpDown24.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown23ValueChanged(object sender, EventArgs e)
		{
			dX[4]=Convert.ToDouble(numericUpDown23.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown22ValueChanged(object sender, EventArgs e)
		{
			dY[4]=Convert.ToDouble(numericUpDown22.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown35ValueChanged(object sender, EventArgs e)
		{
			posX[5]=Convert.ToDouble(numericUpDown35.Value);
			if(ignore==0)	processCode();
		}
		
		void NumericUpDown34ValueChanged(object sender, EventArgs e)
		{
			posY[5]=Convert.ToDouble(numericUpDown34.Value);
			if(ignore==0)	processCode();
			
		}
		
		
		void NumericUpDown33ValueChanged(object sender, EventArgs e)
		{
			posAngle[5]=Convert.ToDouble(numericUpDown33.Value);
			if(ignore==0)processCode();
		}
		
		void NumericUpDown32ValueChanged(object sender, EventArgs e)
		{
			instX[5]=Convert.ToInt16(numericUpDown32.Value);
			if(ignore==0)processCode();
		}
		
		void NumericUpDown31ValueChanged(object sender, EventArgs e)
		{
			instY[5]=Convert.ToInt16(numericUpDown31.Value);
			if(ignore==0)processCode();
		}
		
		void NumericUpDown30ValueChanged(object sender, EventArgs e)
		{
			dX[5]=Convert.ToDouble(numericUpDown30.Value);
			if(ignore==0)processCode();
		}
		
		void NumericUpDown29ValueChanged(object sender, EventArgs e)
		{
			dY[5]=Convert.ToDouble(numericUpDown29.Value);
			if(ignore==0)processCode();
		}
		
		int sel=0;
		
		void Button11Click(object sender, EventArgs e)
		{
			richTextBox13.BringToFront();
			richTextBox1.BringToFront();
			sel=1;
			processCode();
			
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			richTextBox14.BringToFront();
			richTextBox2.BringToFront();
			sel=2;
			processCode();
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			richTextBox15.BringToFront();
			richTextBox3.BringToFront();
			sel=3;
			processCode();
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			richTextBox16.BringToFront();
			richTextBox4.BringToFront();
			sel=4;
			processCode();
		}
		
		void Button15Click(object sender, EventArgs e)
		{
			richTextBox17.BringToFront();
			richTextBox5.BringToFront();
			sel=5;
			processCode();
		}
		
		void RichTextBox9TextChanged(object sender, EventArgs e)
		{
			
		}
		
		void GroupBox2Enter(object sender, EventArgs e)
		{
			
		}
		
		void SaveFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}
		string readSaveDialog()
		{
			if(saveFileDialog1.ShowDialog()==DialogResult.OK)
			{
				try{
					return saveFileDialog1.FileName;
				}catch{
					MessageBox.Show("Error saving to this location");
					return "";
				}
			}
			return "";
		}
		string WorkSaveDialog()
		{
			saveFileDialog2.FileName=DateTime.Now.Hour+"."+DateTime.Now.Minute+"."+DateTime.Now.Second+"_"+DateTime.Now.Year+"."+DateTime.Now.Month+"."+DateTime.Now.Day;
			
			if(saveFileDialog2.ShowDialog()==DialogResult.OK)
			{
				
				try{
					return saveFileDialog2.FileName;
				}catch{
					MessageBox.Show("Error saving to this location");
					return "";
				}
			}
			return "";
		}
		void OpenFileDialog1FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}
		
		string readOpenDialog()
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					return openFileDialog1.FileName;
					
				}
				catch
				{
					MessageBox.Show("Error reading this file");
					return "";
				}
				
			}
			return "";
		}
		
		
		string WorkOpenDialog()
		{
			if (openFileDialog2.ShowDialog() == DialogResult.OK)
			{
				try
				{
					string[] inspect=new string[100];
					inspect=openFileDialog2.FileName.Split(Convert.ToChar(92));
					string fileExport="";
					
					for(int i=0;i<inspect.Length-1;i++)
					{
						if(i!=0)fileExport+=@"\";
						fileExport+=inspect[i];
					}
					
					
					return fileExport;
					
				}
				catch
				{
					MessageBox.Show("Error reading this file");
					return "";
				}
				
			}
			return "";
		}
		
		int eX=0;
		int eY=0;
		
		double viewOffsetX=0;
		double viewOffsetY=0;
		
		int mouseSel=0;
		
		void Panel1MouseDown(object sender, MouseEventArgs e)
		{
			eX=e.X;
			eY=e.Y;
			
			switch (e.Button) {

				case MouseButtons.Left:
					mouseSel=1;
					break;

				case MouseButtons.Right:
					mouseSel=2;
					break;}

			
		}
		
		void Panel1MouseHover(object sender, EventArgs e)
		{
			
		}
		
		int ignore=0;
		void Panel1MouseUp(object sender, MouseEventArgs e)
		{
			if(mouseSel==2)
			{
				viewOffsetX+=((e.X-eX));
				viewOffsetY+=((e.Y-eY));
				// richTextBox2.Text="X "+Convert.ToString(viewOffsetX)+" Y "+Convert.ToString(viewOffsetY);
			}
			
			if(mouseSel==1)
			{
				posX[sel]=posX[sel]+((e.X-eX)/zoom);
				posY[sel]=posY[sel]-((e.Y-eY)/zoom);
				ignore=1;
				if(sel==1)
				{
					try{
						numericUpDown1.Value=Convert.ToDecimal(posX[sel]);
					}catch{}
					try{
						numericUpDown2.Value=Convert.ToDecimal(posY[sel]);
					}catch{}
				}
				if(sel==2)
				{
					try{
						numericUpDown14.Value=Convert.ToDecimal(posX[sel]);
					}catch{}
					try{
						numericUpDown13.Value=Convert.ToDecimal(posY[sel]);
					}catch{}
				}
				if(sel==3)
				{
					try{
						numericUpDown21.Value=Convert.ToDecimal(posX[sel]);
					}catch{}
					try{
						numericUpDown20.Value=Convert.ToDecimal(posY[sel]);
					}catch{}
				}
				if(sel==4)
				{
					try{
						numericUpDown28.Value=Convert.ToDecimal(posX[sel]);
					}catch{}
					try{
						numericUpDown27.Value=Convert.ToDecimal(posY[sel]);
					}catch{}
				}
				if(sel==5)
				{
					try{
						numericUpDown35.Value=Convert.ToDecimal(posX[sel]);
					}catch{}
					try{
						numericUpDown34.Value=Convert.ToDecimal(posY[sel]);
					}catch{}
				}
				ignore=0;
				
				textBox6.Text=Convert.ToString(posX[sel]);
				textBox7.Text=Convert.ToString(posY[sel]);
				
			}
			processCode();
		}
		
		void Button17Click(object sender, EventArgs e)
		{
			if(zoom>5)
			{
				zoom-=1;
			}
			Thread.Sleep(50);
			processCode();
		}
		
		void Button16Click(object sender, EventArgs e)
		{
			zoom+=1;
			Thread.Sleep(50);
			processCode();
		}
		
		void Button18Click(object sender, EventArgs e)
		{
			
		}
		
		
		
		void ToolStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			
		}
		
		string savedPos="";
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			
			Save(save);
			
			
		}
		
		string[] savedRows= new string[10];
		string[] posData= new string [10];
		
		void ToolStripMenuItem3Click(object sender, EventArgs e)
		{
			
		}
		
		double tableX=60;
		double tableY=48;
		void defaultGraphics()
		{
			
			/*Pen pen = new Pen(Color.Red, Convert.ToSingle(0.1*zoom));
			g.DrawLine(pen,Convert.ToSingle(offset*zoom),Convert.ToSingle(offset*zoom),Convert.ToSingle((tableX*zoom)+(offset*zoom)),Convert.ToSingle(offset*zoom));
			g.DrawLine(pen,Convert.ToSingle((tableX*zoom)+(offset*zoom)),Convert.ToSingle(offset*zoom),Convert.ToSingle((tableX*zoom)+(offset*zoom)),Convert.ToSingle((tableY*zoom)+(offset*zoom)));
			Pen pen1 = new Pen(Color.Orange, Convert.ToSingle(0.1*zoom));
			g.DrawLine(pen1,Convert.ToSingle(offset*zoom),Convert.ToSingle(offset*zoom),Convert.ToSingle(offset*zoom),Convert.ToSingle((tableY*zoom)+(offset*zoom)));
			g.DrawLine(pen1,Convert.ToSingle(offset*zoom),Convert.ToSingle((tableY*zoom)+(offset*zoom)),Convert.ToSingle((tableX*zoom)+(offset*zoom)),Convert.ToSingle((tableY*zoom)+(offset*zoom)));
			 */
			
			// Create font and brush.
			Font drawFont = new Font("Arial", Convert.ToSingle(zoom*1));

			StringFormat drawFormat = new StringFormat();
			
			SolidBrush drawBrushY = new SolidBrush(Color.DarkGreen);
			g.DrawString("Y", drawFont, drawBrushY,Convert.ToSingle(2+viewOffsetX), panel1.Size.Height-Convert.ToSingle((zoom*10)-viewOffsetY), drawFormat);
			
			SolidBrush drawBrushX = new SolidBrush(Color.Red);
			g.DrawString("X", drawFont, drawBrushX,Convert.ToSingle((zoom*10)+viewOffsetX), panel1.Size.Height-Convert.ToSingle((offset*zoom)-viewOffsetY), drawFormat);

			Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), Convert.ToSingle(0.1*zoom));
			g.DrawRectangle(blackPen, Convert.ToInt32((offset*zoom)+viewOffsetX), Convert.ToInt32(panel1.Size.Height-(offset*zoom)-5-(tableY*zoom)+viewOffsetY), Convert.ToSingle(tableX*zoom), Convert.ToSingle(tableY*zoom));
			
			//g.DrawArc(pen, 100, 100, 50, 50, 20, 190);
		}
		
		void Button20Click(object sender, EventArgs e)
		{
			try{
				button1.PerformClick();
				if(sel==1)File.WriteAllText(readSaveDialog(),richTextBox13.Text);
				if(sel==2)File.WriteAllText(readSaveDialog(),richTextBox14.Text);
				if(sel==3)File.WriteAllText(readSaveDialog(),richTextBox15.Text);
				if(sel==4)File.WriteAllText(readSaveDialog(),richTextBox16.Text);
				if(sel==5)File.WriteAllText(readSaveDialog(),richTextBox17.Text);
				
			}catch{}
		}
		
		string[] labels=new string[10];
		string[] paths=new string[10];
		
		void TextBox9TextChanged(object sender, EventArgs e)
		{
			labels[1]=textBox9.Text;
		}
		
		void TextBox10TextChanged(object sender, EventArgs e)
		{
			labels[2]=textBox10.Text;
		}
		
		void TextBox11TextChanged(object sender, EventArgs e)
		{
			labels[3]=textBox11.Text;
		}
		
		void TextBox12TextChanged(object sender, EventArgs e)
		{
			labels[4]=textBox12.Text;
		}
		
		void TextBox13TextChanged(object sender, EventArgs e)
		{
			labels[5]=textBox13.Text;
		}
		
		string save="";
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			try{
				save=WorkSaveDialog();
				Save(save);
				toolStripMenuItem1.Enabled=true;
				
			}catch{}
			
			
		}
		void drawMarkers()
		{
			Font dFont = new Font("Arial",Convert.ToSingle(zoom));
			SolidBrush dBrush = new SolidBrush(Color.Black);
			
			
				try{
				int e=0;
					if(button5.BackColor==Color.Green)g.DrawString(labels[e+1],dFont, dBrush,Convert.ToSingle((posX[e+1]*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-(posY[e+1]*zoom)-50+viewOffsetY));
					e++;
					if(button6.BackColor==Color.Green)g.DrawString(labels[e+1],dFont, dBrush,Convert.ToSingle((posX[e+1]*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-(posY[e+1]*zoom)-50+viewOffsetY));
					e++;
					if(button7.BackColor==Color.Green)g.DrawString(labels[e+1],dFont, dBrush,Convert.ToSingle((posX[e+1]*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-(posY[e+1]*zoom)-50+viewOffsetY));
					e++;
					if(button8.BackColor==Color.Green)g.DrawString(labels[e+1],dFont, dBrush,Convert.ToSingle((posX[e+1]*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-(posY[e+1]*zoom)-50+viewOffsetY));
					e++;
					if(button9.BackColor==Color.Green)g.DrawString(labels[e+1],dFont, dBrush,Convert.ToSingle((posX[e+1]*zoom)+viewOffsetX),Convert.ToSingle(panel1.Size.Height-(posY[e+1]*zoom)-50+viewOffsetY));
					
				}catch{}
			
			
		}
		Bitmap bitmap;
		void PrintDocument1PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			e.Graphics.DrawImage(bitmap, 0, 0);
		}
		
		void Button19Click(object sender, EventArgs e)
		{
			
				
		}
		
		
		void RichTextBox7TextChanged(object sender, EventArgs e)
		{
			if(richTextBox7.Text!="") richTextBox7.BackColor=Color.Yellow;
			else richTextBox7.BackColor=Color.White;
		}
		
		void ToolStripDropDownButton2Click(object sender, EventArgs e)
		{
			
		}
		
		double ts1=0, ts2=0, ts3=0,ts4=0;
		
		void ToolStripTextBox4Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox4TextChanged(object sender, EventArgs e)
		{
			try{
				ts1=Convert.ToDouble(toolStripTextBox4.Text);
			}catch{
				ts1=0;
				toolStripTextBox4.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox2TextChanged(object sender, EventArgs e)
		{
			try{
				ts2=Convert.ToDouble(toolStripTextBox2.Text);
			}catch{
				ts2=0;
				toolStripTextBox2.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox6TextChanged(object sender, EventArgs e)
		{
			try{
				ts3=Convert.ToDouble(toolStripTextBox6.Text);
			}catch{
				ts3=0;
				toolStripTextBox6.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox8TextChanged(object sender, EventArgs e)
		{
			try{
				ts4=Convert.ToDouble(toolStripTextBox8.Text);
			}catch{
				ts4=0;
				toolStripTextBox8.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox10Click(object sender, EventArgs e)
		{
			
		}
		void ToolStripTextBox14Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox10TextChanged(object sender, EventArgs e)
		{
			try{
				fd1[1]=Convert.ToDouble(toolStripTextBox10.Text);
			}catch{
				fd1[1]=0;
				toolStripTextBox10.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox12TextChanged(object sender, EventArgs e)
		{
			try{
				fm1[1]=Convert.ToDouble(toolStripTextBox12.Text);
			}catch{
				fm1[1]=0;
				toolStripTextBox12.Text="";
			}
			saveSettings("settings");
		}
		
		
		void ToolStripTextBox14TextChanged(object sender, EventArgs e)
		{
			try{
				fd1[2]=Convert.ToDouble(toolStripTextBox14.Text);
			}
			catch{
				fd1[2]=0;
				toolStripTextBox14.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox16TextChanged(object sender, EventArgs e)
		{
			try{
				fm1[2]=Convert.ToDouble(toolStripTextBox16.Text);
			}catch{
				fm1[2]=0;
				toolStripTextBox16.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox17TextChanged(object sender, EventArgs e)
		{
			try{
				fd1[3]=Convert.ToDouble(toolStripTextBox17.Text);
			}
			catch{
				fd1[3]=0;
				toolStripTextBox17.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox18TextChanged(object sender, EventArgs e)
		{
			try{
				fm1[3]=Convert.ToDouble(toolStripTextBox18.Text);
			}catch{
				fm1[3]=0;
				toolStripTextBox18.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox19TextChanged(object sender, EventArgs e)
		{
			try{
				fd1[4]=Convert.ToDouble(toolStripTextBox19.Text);
			}catch{
				fd1[4]=0;
				toolStripTextBox19.Text="";
			}
			saveSettings("settings");
		}
		
		void ToolStripTextBox20TextChanged(object sender, EventArgs e)
		{
			try{
				fm1[4]=Convert.ToDouble(toolStripTextBox20.Text);
			}catch{
				fm1[4]=0;
				toolStripTextBox20.Text="";
			}
			saveSettings("settings");
		}
		
		string saved="";
		
		void saveSettings(string settings)
		{
			
			saved=Convert.ToString(ts1)+"|"+Convert.ToString(ts2)+"|"+Convert.ToString(ts3)+"|"+Convert.ToString(ts4)+"\n";
			saved+=Convert.ToString(ToolOrder[1])+"|"+Convert.ToString(ToolOrder[2])+"|"+Convert.ToString(ToolOrder[3])+"|"+Convert.ToString(ToolOrder[4])+"\n";
			
			for(int g=1; g<=4;g++)
			{
				try{
					saved+=fd1[g]+"|";
				}catch{saved+="0.0|";}
				try{
					saved+=fm1[g]+"\n";
				}catch{}
			}
			
			try{
				File.WriteAllText(@"C:\Program Files\GcodeEditor\"+settings+@"\settings.txt", saved);
			}catch{error();}
		}
		
		string[] opened= new string[10];
		string[] read= new string[5];
		void openSettings(string settings)
		{
			try{
				opened=File.ReadAllLines(@"C:\Program Files\GcodeEditor\"+settings+@"\settings.txt");
				
				
				read=opened[0].Split('|');
				ts1=Convert.ToInt16(read[0]);
				toolStripTextBox4.Text=read[0];
				ts2=Convert.ToInt16(read[1]);
				toolStripTextBox2.Text=read[1];
				ts3=Convert.ToInt16(read[2]);
				toolStripTextBox6.Text=read[2];
				ts4=Convert.ToInt16(read[3]);
				toolStripTextBox8.Text=read[3];
				
				read=opened[1].Split('|');
				ToolOrder[1]=Convert.ToInt16(read[0]);
				toolStripComboBox1.Text=read[0];
				ToolOrder[2]=Convert.ToInt16(read[1]);
				toolStripComboBox2.Text=read[1];
				ToolOrder[3]=Convert.ToInt16(read[2]);
				toolStripComboBox3.Text=read[2];
				ToolOrder[4]=Convert.ToInt16(read[3]);
				toolStripComboBox4.Text=read[3];
				
				read=opened[2].Split('|');
				fd1[1]=Convert.ToDouble(read[0]);
				toolStripTextBox10.Text=read[0];
				fm1[1]=Convert.ToDouble(read[1]);
				toolStripTextBox12.Text=read[1];
				
				read=opened[3].Split('|');
				fd1[2]=Convert.ToDouble(read[0]);
				toolStripTextBox14.Text=read[0];
				fm1[2]=Convert.ToDouble(read[1]);
				toolStripTextBox16.Text=read[1];
				
				read=opened[4].Split('|');
				fd1[3]=Convert.ToDouble(read[0]);
				toolStripTextBox17.Text=read[0];
				fm1[3]=Convert.ToDouble(read[1]);
				toolStripTextBox18.Text=read[1];
				
				read=opened[5].Split('|');
				fd1[4]=Convert.ToDouble(read[0]);
				toolStripTextBox19.Text=read[0];
				fm1[4]=Convert.ToDouble(read[1]);
				toolStripTextBox20.Text=read[1];
			}catch{error();}
			
		}
		
		
		void ToolStripTextBox21Click(object sender, EventArgs e)
		{
			
			
		}
		
		void ToolStripTextBox23Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox25Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox27Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox29Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox31Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox33Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox22Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox24Click(object sender, EventArgs e)
		{
			
			
		}
		
		void ToolStripTextBox26Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox28Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox30Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox32Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolStripTextBox34Click(object sender, EventArgs e)
		{
			
		}
		
		void CallToolStripMenuItemClick(object sender, EventArgs e)
		{
			openSettings("PC");
		}
		
		void SaveToolStripMenuItemClick(object sender, EventArgs e)
		{
			//saveSettings("PC");
		}
		
		void CallToolStripMenuItem1Click(object sender, EventArgs e)
		{
			openSettings("PVC");
		}
		
		void SaveToolStripMenuItem1Click(object sender, EventArgs e)
		{
			//saveSettings("PVC");
		}
		
		void CallToolStripMenuItem2Click(object sender, EventArgs e)
		{
			openSettings("Acrylic");
		}
		
		void SaveToolStripMenuItem2Click(object sender, EventArgs e)
		{
			//saveSettings("Acrylic");
		}
		
		void CallToolStripMenuItem3Click(object sender, EventArgs e)
		{
			openSettings("Aluminum");
		}
		
		void SaveToolStripMenuItem3Click(object sender, EventArgs e)
		{
			//saveSettings("Aluminum");
		}
		
		void CallToolStripMenuItem4Click(object sender, EventArgs e)
		{
			openSettings("Preset1");
		}
		
		void SaveToolStripMenuItem4Click(object sender, EventArgs e)
		{
			//saveSettings("Preset1");
		}
		
		void CallToolStripMenuItem5Click(object sender, EventArgs e)
		{
			openSettings("Preset2");
		}
		
		void SaveToolStripMenuItem5Click(object sender, EventArgs e)
		{
			//saveSettings("Preset2");
		}
		
		void CallToolStripMenuItem6Click(object sender, EventArgs e)
		{
			openSettings("Preset3");
		}
		
		void SaveToolStripMenuItem6Click(object sender, EventArgs e)
		{
			//saveSettings("Preset3");
		}
		
		void ToolStripComboBox1Click(object sender, EventArgs e)
		{
			
		}
		
		
		
		void ToolStripComboBox1TextChanged(object sender, EventArgs e)
		{
			ToolOrder[1]=Convert.ToInt16(toolStripComboBox1.Text);
			saveSettings("settings");
		}
		
		void ToolStripComboBox2TextChanged(object sender, EventArgs e)
		{
			ToolOrder[2]=Convert.ToInt16(toolStripComboBox2.Text);
			saveSettings("settings");
		}
		
		void ToolStripComboBox3TextChanged(object sender, EventArgs e)
		{
			ToolOrder[3]=Convert.ToInt16(toolStripComboBox3.Text);
			saveSettings("settings");
		}
		
		void ToolStripComboBox4TextChanged(object sender, EventArgs e)
		{
			ToolOrder[4]=Convert.ToInt16(toolStripComboBox4.Text);
			saveSettings("settings");
		}
		
		void PrintToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			WindowState = FormWindowState.Maximized;
			
			//Add a Panel control.
			Panel panel = new Panel();
			this.Controls.Add(panel);
			
			//Create a Bitmap of size same as that of the Form.
			Graphics grp = panel.CreateGraphics();
			Size formSize = this.ClientSize;
			bitmap = new Bitmap(formSize.Width, formSize.Height, grp);
			grp = Graphics.FromImage(bitmap);
			
			Size printArea= new Size(panel1.Size.Width-10,panel1.Size.Height-5);
			
			//Copy screen area that that the Panel covers.
			Point panelLocation = PointToScreen(panel.Location);
			grp.CopyFromScreen(panelLocation.X+groupBox1.Width+20, panelLocation.Y+50,5, 5, printArea );
			
			//Show the Print Preview Dialog.
			printPreviewDialog1.Document = printDocument1;
			printPreviewDialog1.Document.DefaultPageSettings.Landscape=true;
			//pd.DefaultPageSettings.Landscape = true;
			printPreviewDialog1.PrintPreviewControl.Zoom = 1;
			//
			if(DialogResult.OK==printDialog1.ShowDialog())
			{
				printPreviewDialog1.ShowDialog();
			}
			
		}
		
		
		void Button21Click(object sender, EventArgs e)
		{
			processCode();
		}
		
		void NCViewerToolStripMenuItemClick(object sender, EventArgs e)
		{
			groupBox9.BringToFront();
			groupBox9.Visible=true;
		}
		
		void GCodeEditorToolStripMenuItemClick(object sender, EventArgs e)
		{
			groupBox9.SendToBack();
			groupBox9.Visible=false;
		}
		
		int timeHold=0;
		
		void AutosaveTick(object sender, EventArgs e)
		{
			if(Convert.ToInt16(DateTime.Now.Minute)!=timeHold)
			{
				
				try{
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\1.txt",richTextBox1.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\2.txt",richTextBox2.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\3.txt",richTextBox3.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\4.txt",richTextBox4.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\5.txt",richTextBox5.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\1mod.txt",richTextBox13.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\2mod.txt",richTextBox14.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\3mod.txt",richTextBox15.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\4mod.txt",richTextBox16.Text);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\5mod.txt",richTextBox17.Text);
					File.WriteAllLines(@"C:\Program Files\GcodeEditor\lastsave\paths.txt", paths);
					File.WriteAllLines(@"C:\Program Files\GcodeEditor\lastsave\labels.txt",labels);
					
					savedPos="";
					for(int r=1;r<=5;r++)
					{
						savedPos+=Convert.ToString(posX[r])+"|";
						savedPos+=Convert.ToString(posY[r])+"|";
						savedPos+=Convert.ToString(posAngle[r])+"|";
						savedPos+=Convert.ToString(instX[r])+"|";
						savedPos+=Convert.ToString(instY[r])+"|";
						savedPos+=Convert.ToString(dX[r])+"|";
						savedPos+=Convert.ToString(dY[r])+"|";
						savedPos+=Convert.ToString(zCH[r])+"\n";
					}
					
					
					File.WriteAllText(@"C:\Program Files\GcodeEditor\lastsave\savedPos.txt",savedPos);
				}catch{}
			}
		}
		
		void ToolStripTextBox26TextChanged(object sender, EventArgs e)
		{
			panel1.Refresh();
			try{
				if(Convert.ToDouble(toolStripTextBox26.Text)>0)
					
				{
					tableY=Convert.ToDouble(toolStripTextBox26.Text);
					defaultGraphics();
					string table=Convert.ToString(tableX)+"\n"+Convert.ToString(tableY);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\table.txt",table);
				}
			}catch{}
		}
		
		void ToolStripTextBox25TextChanged(object sender, EventArgs e)
		{
			panel1.Refresh();
			try{
				if(Convert.ToDouble(toolStripTextBox25.Text)>0)
				{
					tableX=Convert.ToDouble(toolStripTextBox25.Text);
					defaultGraphics();
					string table=Convert.ToString(tableX)+"\n"+Convert.ToString(tableY);
					File.WriteAllText(@"C:\Program Files\GcodeEditor\table.txt",table);
				}
			}catch{}
		}
		
		
		
		
		
		void Open(string file)
		{
			richTextBox1.Text=File.ReadAllText(file+@"\1.txt");
			richTextBox2.Text=File.ReadAllText(file+@"\2.txt");
			richTextBox3.Text=File.ReadAllText(file+@"\3.txt");
			richTextBox4.Text=File.ReadAllText(file+@"\4.txt");
			richTextBox5.Text=File.ReadAllText(file+@"\5.txt");
			richTextBox13.Text=File.ReadAllText(file+@"\1mod.txt");
			richTextBox14.Text=File.ReadAllText(file+@"\2mod.txt");
			richTextBox15.Text=File.ReadAllText(file+@"\3mod.txt");
			richTextBox16.Text=File.ReadAllText(file+@"\4mod.txt");
			richTextBox17.Text=File.ReadAllText(file+@"\5mod.txt");
			labels=File.ReadAllLines(file+@"\labels.txt");
			paths=File.ReadAllLines(file+@"\paths.txt");
			savedRows=File.ReadAllLines(file+@"\savedPos.txt");
			
			for(int u=0;u<5;u++)
			{
				posData=savedRows[u].Split('|');
				posX[u+1]=Convert.ToDouble(posData[0]);
				posY[u+1]=Convert.ToDouble(posData[1]);
				posAngle[u+1]=Convert.ToDouble(posData[2]);
				instX[u+1]=Convert.ToInt16(posData[3]);
				instY[u+1]=Convert.ToInt16(posData[4]);
				dX[u+1]=Convert.ToDouble(posData[5]);
				dY[u+1]=Convert.ToDouble(posData[6]);
				zCH[u+1]=Convert.ToDouble(posData[7]);
				
			}
			
			//1-------------------------------------------------------------
			numericUpDown1.Value=Convert.ToDecimal(posX[1]);
			numericUpDown2.Value=Convert.ToDecimal(posY[1]);
			numericUpDown3.Value=Convert.ToDecimal(posAngle[1]);
			numericUpDown4.Value=Convert.ToDecimal(instX[1]);
			numericUpDown5.Value=Convert.ToDecimal(instY[1]);
			numericUpDown6.Value=Convert.ToDecimal(dX[1]);
			numericUpDown7.Value=Convert.ToDecimal(dY[1]);
			numericUpDown36.Value=Convert.ToDecimal(zCH[1]);
			
			//2-------------------------------------------------------------
			numericUpDown14.Value=Convert.ToDecimal(posX[2]);
			numericUpDown13.Value=Convert.ToDecimal(posY[2]);
			numericUpDown12.Value=Convert.ToDecimal(posAngle[2]);
			numericUpDown11.Value=Convert.ToDecimal(instX[2]);
			numericUpDown10.Value=Convert.ToDecimal(instY[2]);
			numericUpDown9.Value=Convert.ToDecimal(dX[2]);
			numericUpDown8.Value=Convert.ToDecimal(dY[2]);
			numericUpDown37.Value=Convert.ToDecimal(zCH[2]);
			
			//3-------------------------------------------------------------
			numericUpDown21.Value=Convert.ToDecimal(posX[3]);
			numericUpDown20.Value=Convert.ToDecimal(posY[3]);
			numericUpDown19.Value=Convert.ToDecimal(posAngle[3]);
			numericUpDown18.Value=Convert.ToDecimal(instX[3]);
			numericUpDown17.Value=Convert.ToDecimal(instY[3]);
			numericUpDown16.Value=Convert.ToDecimal(dX[3]);
			numericUpDown15.Value=Convert.ToDecimal(dY[3]);
			numericUpDown38.Value=Convert.ToDecimal(zCH[3]);
			
			//4-------------------------------------------------------------
			numericUpDown28.Value=Convert.ToDecimal(posX[4]);
			numericUpDown27.Value=Convert.ToDecimal(posY[4]);
			numericUpDown26.Value=Convert.ToDecimal(posAngle[4]);
			numericUpDown25.Value=Convert.ToDecimal(instX[4]);
			numericUpDown24.Value=Convert.ToDecimal(instY[4]);
			numericUpDown23.Value=Convert.ToDecimal(dX[4]);
			numericUpDown22.Value=Convert.ToDecimal(dY[4]);
			numericUpDown39.Value=Convert.ToDecimal(zCH[4]);
			
			//5-------------------------------------------------------------
			numericUpDown35.Value=Convert.ToDecimal(posX[5]);
			numericUpDown34.Value=Convert.ToDecimal(posY[5]);
			numericUpDown33.Value=Convert.ToDecimal(posAngle[5]);
			numericUpDown32.Value=Convert.ToDecimal(instX[5]);
			numericUpDown31.Value=Convert.ToDecimal(instY[5]);
			numericUpDown30.Value=Convert.ToDecimal(dX[5]);
			numericUpDown29.Value=Convert.ToDecimal(dY[5]);
			numericUpDown40.Value=Convert.ToDecimal(zCH[5]);
			
			
			textBox9.Text=labels[1];
			textBox10.Text=labels[2];
			textBox11.Text=labels[3];
			textBox12.Text=labels[4];
			textBox13.Text=labels[5];
			
			if(paths[1]!="")
			{
				textBox1.Text=paths[1];
				button5.BackColor=Color.Green;
			}
			if(paths[2]!="")
			{
				textBox2.Text=paths[2];
				button6.BackColor=Color.Green;
			}
			if(paths[3]!="")
			{
				textBox3.Text=paths[3];
				button7.BackColor=Color.Green;
			}
			if(paths[4]!="")
			{
				textBox4.Text=paths[4];
				button8.BackColor=Color.Green;
			}
			if(paths[5]!="")
			{
				textBox5.Text=paths[5];
				button9.BackColor=Color.Green;
			}
			processCode();
			
		}
		
		void Save(string save)
		{
			
			try{
				Directory.CreateDirectory(saveFileDialog2.FileName);
				File.WriteAllText(save+@"\1.txt",richTextBox1.Text);
				File.WriteAllText(save+@"\2.txt",richTextBox2.Text);
				File.WriteAllText(save+@"\3.txt",richTextBox3.Text);
				File.WriteAllText(save+@"\4.txt",richTextBox4.Text);
				File.WriteAllText(save+@"\5.txt",richTextBox5.Text);
				File.WriteAllText(save+@"\1mod.txt",richTextBox13.Text);
				File.WriteAllText(save+@"\2mod.txt",richTextBox14.Text);
				File.WriteAllText(save+@"\3mod.txt",richTextBox15.Text);
				File.WriteAllText(save+@"\4mod.txt",richTextBox16.Text);
				File.WriteAllText(save+@"\5mod.txt",richTextBox17.Text);
				File.WriteAllLines(save+@"\paths.txt", paths);
				File.WriteAllLines(save+@"\labels.txt",labels);
				
				savedPos="";
				for(int r=1;r<=5;r++)
				{
					savedPos+=Convert.ToString(posX[r])+"|";
					savedPos+=Convert.ToString(posY[r])+"|";
					savedPos+=Convert.ToString(posAngle[r])+"|";
					savedPos+=Convert.ToString(instX[r])+"|";
					savedPos+=Convert.ToString(instY[r])+"|";
					savedPos+=Convert.ToString(dX[r])+"|";
					savedPos+=Convert.ToString(dY[r])+"|";
					savedPos+=Convert.ToString(zCH[r])+"\n";
				}
				
				
				File.WriteAllText(save+@"\savedPos.txt",savedPos);
				MessageBox.Show("Saved!");
			}catch{MessageBox.Show(saveFileDialog1.FileName);}
		}
		
		
		void MainFormActivated(object sender, EventArgs e)
		{
			autosave.Start();
		}
		
		void MainFormDeactivate(object sender, EventArgs e)
		{
			autosave.Stop();
		}
		
		void ProcessTick(object sender, EventArgs e)
		{
			processCode();
			process.Stop();
		}
		
		void processCode()
		{
			
			process.Stop();
			runCode();
		}
		
		void runCode()
		{
			panel1.Refresh();
			
			defaultGraphics();
			drawMarkers();
			
			maxX=0;
			maxY=0;
			maxZ=0;
			
			if(button5.BackColor==Color.Green)
			{
				colorOrder=1;
				richTextBox13.Text=alterGcode(richTextBox1.Text,1,instX[1],instY[1],dX[1],dY[1],zCH[1]);
			}
			
			if(button6.BackColor==Color.Green){
				colorOrder=2;
				richTextBox14.Text=alterGcode(richTextBox2.Text,2,instX[2],instY[2],dX[2],dY[2],zCH[2]);
			}
			if(button7.BackColor==Color.Green)
			{
				colorOrder=3;
				richTextBox15.Text=alterGcode(richTextBox3.Text,3,instX[3],instY[3],dX[3],dY[3],zCH[3]);
			}
			if(button8.BackColor==Color.Green)
			{
				colorOrder=4;
				richTextBox16.Text=alterGcode(richTextBox4.Text,4,instX[4],instY[4],dX[4],dY[4],zCH[4]);
			}
			if(button9.BackColor==Color.Green)
			{
				colorOrder=5;
				richTextBox17.Text=alterGcode(richTextBox5.Text,5,instX[5],instY[5],dX[5],dY[5],zCH[5]);
			}
			
			textBox8.Text=Convert.ToString(-1*maxZ);
			
			richTextBox10.Text="";
			if(button5.BackColor==Color.Green)richTextBox10.Text+=richTextBox13.Text;
			if(button6.BackColor==Color.Green)richTextBox10.Text+=richTextBox14.Text;
			if(button7.BackColor==Color.Green)	richTextBox10.Text+=richTextBox15.Text;
			if(button8.BackColor==Color.Green)richTextBox10.Text+=richTextBox16.Text;
			if(button9.BackColor==Color.Green)	richTextBox10.Text+=richTextBox17.Text;
			
			row=richTextBox10.Text.Split('\n');
			
			tool[1]="";
			tool[2]="";
			tool[3]="";
			tool[4]="";
			
			for(int n=0;n<row.Length;n++)
			{
				if(row[n].Contains("G990"))T=0;
				if(row[n].Contains("T1")&&row[n-1].Contains("S"))T=1;
				if(row[n].Contains("T2")&&row[n-1].Contains("S"))T=2;
				if(row[n].Contains("T3")&&row[n-1].Contains("S"))T=3;
				if(row[n].Contains("T4")&&row[n-1].Contains("S"))T=4;
				
				if(T==1 && !(row[n].Contains("T")||row[n].Contains("S")))tool[1]+=row[n]+"\n";
				if(T==2&& !(row[n].Contains("T")||row[n].Contains("S")))tool[2]+=row[n]+"\n";
				if(T==3&& !(row[n].Contains("T")||row[n].Contains("S")))tool[3]+=row[n]+"\n";
				if(T==4&& !(row[n].Contains("T")||row[n].Contains("S")))tool[4]+=row[n]+"\n";
			}
			
			richTextBox8.Text=tool[1];
			richTextBox6.Text=tool[2];
			richTextBox9.Text=tool[3];
			richTextBox7.Text=tool[4];
			
			richTextBox10.Text="M999\n(GcodeEditer.exe-Proceed with caution)\n";
			
			if(tool[4]!=""&& humanInput==1)
			{
				datashare.toolprompt="";
				Prompt form2= new Prompt();
				if( DialogResult.OK==form2.ShowDialog())
					richTextBox10.Text+="(Tool 4 in program, follow directions)\n"+"("+datashare.toolprompt+")\n";
			}
			
			richTextBox10.Text+="(Stock Z: "+Convert.ToString(maxZ)+" X: "+Convert.ToString(maxX)+" Y: "+Convert.ToString(maxY)+")\n";
			richTextBox10.Text+=HeaderA+textBox8.Text+HeaderB;
			
			for(int p=0;p<=4;p++)
			{
				if(tool[3]!=""&&ToolOrder[3]==p) richTextBox10.Text+="\n\nS"+Convert.ToString(ts3)+"\n"+hTool3+"\n"+tool[3];
				else if(tool[2]!=""&&ToolOrder[2]==p) richTextBox10.Text+="\n\nS"+Convert.ToString(ts2)+"\n"+hTool2+"\n"+tool[2];
				else if(tool[1]!=""&&ToolOrder[1]==p) richTextBox10.Text+="\n\nS"+Convert.ToString(ts1)+"\n"+hTool1+"\n"+tool[1];
				else if(tool[4]!=""&&ToolOrder[4]==p) richTextBox10.Text+="\n\nS"+Convert.ToString(ts4)+"\n"+hTool4+"\n"+tool[4];
			}
			
			richTextBox10.Text+=Footer;
			if(humanInput==1)
			{
				humanInput=0;
				outputGcode=richTextBox10.Text;
				File.WriteAllText(readSaveDialog(),outputGcode);
			}
		}
		
		void SaveFileDialog2FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}
		
		void OpenToolStripMenuItemClick(object sender, EventArgs e)
		{
			string open=WorkOpenDialog();
			Open(open);
			toolStripMenuItem1.Enabled=true;
		}
		
		void OKToolStripMenuItemClick(object sender, EventArgs e)
		{
			saveSettings("PC");
		}
		
		void OKToolStripMenuItem1Click(object sender, EventArgs e)
		{
			saveSettings("PVC");
		}
		
		void OKToolStripMenuItem2Click(object sender, EventArgs e)
		{
			saveSettings("Acrylic");
		}
		
		void OKToolStripMenuItem3Click(object sender, EventArgs e)
		{
			saveSettings("Aluminum");
		}
		
		void OKToolStripMenuItem4Click(object sender, EventArgs e)
		{
			saveSettings("Preset1");
		}
		
		void OKToolStripMenuItem5Click(object sender, EventArgs e)
		{
			saveSettings("Preset2");
		}
		
		void OKToolStripMenuItem6Click(object sender, EventArgs e)
		{
			saveSettings("Preset3");
		}
		
		
		void OKToolStripMenuItem7Click(object sender, EventArgs e)
		{
			Open(@"C:\Program Files\GcodeEditor\lastsave");
		}
		
		void ToolStripTextBox12Click(object sender, EventArgs e)
		{
			
		}
		
		double[] zCH=new double[6] {0,0,0,0,0,0};
		
		void NumericUpDown36ValueChanged(object sender, EventArgs e)
		{
			zCH[1]=Convert.ToDouble(numericUpDown36.Value);
		}
		
		void NumericUpDown37ValueChanged(object sender, EventArgs e)
		{
			zCH[2]=Convert.ToDouble(numericUpDown37.Value);
		}
		
		void NumericUpDown38ValueChanged(object sender, EventArgs e)
		{
			zCH[3]=Convert.ToDouble(numericUpDown38.Value);
		}
		
		void NumericUpDown39ValueChanged(object sender, EventArgs e)
		{
			zCH[4]=Convert.ToDouble(numericUpDown39.Value);
		}
		
		void NumericUpDown40ValueChanged(object sender, EventArgs e)
		{
			zCH[5]=Convert.ToDouble(numericUpDown40.Value);
		}
		
		void Button22Click(object sender, EventArgs e)
		{
			if(sel==1)
			{
				textBox1.Clear();
				richTextBox1.Clear();
				richTextBox13.Clear();
				button5.BackColor=Color.Yellow;
				processCode();
			}
			if(sel==2)
			{
				textBox2.Clear();
				richTextBox2.Clear();
				richTextBox14.Clear();
				button6.BackColor=Color.Yellow;
				processCode();
			}
			if(sel==3)
			{
				textBox3.Clear();
				richTextBox3.Clear();
				richTextBox15.Clear();
				button7.BackColor=Color.Yellow;
				processCode();
			}
			if(sel==4)
			{
				textBox4.Clear();
				richTextBox4.Clear();
				richTextBox16.Clear();
				button8.BackColor=Color.Yellow;
				processCode();
			}
			if(sel==5)
			{
				textBox5.Clear();
				richTextBox5.Clear();
				richTextBox17.Clear();
				button9.BackColor=Color.Yellow;
				processCode();
			}
		}
		
		void Button23Click(object sender, EventArgs e)
		{
			viewOffsetX=0;
			viewOffsetY=0;
			processCode();
		}
		
		void HeaderToolStripMenuItemClick(object sender, EventArgs e)
		{
			richTextBox11.BringToFront();
			richTextBox11.Text=HeaderA+textBox8.Text+HeaderB;
		}
		
		void FooterToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			richTextBox12.BringToFront();
			richTextBox12.Text=Footer;
		}
		
		void PrintPreviewDialog1Load(object sender, EventArgs e)
		{
			
		}
	}
	
}
