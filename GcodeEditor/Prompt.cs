/*
 * Created by SharpDevelop.
 * User: linea
 * Date: 1/23/2021
 * Time: 9:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GcodeEditor
{
	/// <summary>
	/// Description of Prompt.
	/// </summary>
	public partial class Prompt : Form
	{
		public Prompt()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Label2Click(object sender, EventArgs e)
		{
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Prompt.ActiveForm.Close();
		}
		
		void RichTextBox1TextChanged(object sender, EventArgs e)
		{
			MainForm.datashare.toolprompt=richTextBox1.Text;
		}
	}
}
