﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet;
using HslCommunication;
using HslCommunication.Secs;
using HslCommunication.Secs.Types;
using System.Threading;
using System.IO.Ports;
using System.Xml.Linq;
using HslCommunication.Secs.Helper;

namespace HslCommunicationDemo
{
	public partial class FormSecsGem : HslFormContent
	{
		public FormSecsGem( )
		{
			InitializeComponent( );
		}

		private SecsHsms secs = null;

		private void FormSiemens_Load( object sender, EventArgs e )
		{
			panel2.Enabled = false;
			Language( Program.Language );

			StringBuilder stringBuilder = new StringBuilder( "Example：" );
			stringBuilder.Append( new SecsValue( (sbyte)1 ) );
			stringBuilder.Append( new SecsValue( (byte)2 ) );
			stringBuilder.Append( new SecsValue( (short)3 ) );
			stringBuilder.Append( new SecsValue( (ushort)4 ) );
			stringBuilder.Append( new SecsValue( (int)5 ) );
			stringBuilder.Append( new SecsValue( (uint)6 ) );
			stringBuilder.Append( new SecsValue( (long)7 ) );
			stringBuilder.Append( new SecsValue( (ulong)8 ) );
			stringBuilder.Append( new SecsValue( (float)9 ) );
			stringBuilder.Append( new SecsValue( (double)10 ) );
			stringBuilder.Append( new SecsValue( "ABC" ) );
			stringBuilder.Append( new SecsValue( new byte[] { 0x01, 0x02, 0x03 } ) );
			stringBuilder.Append( new SecsValue( true ) );
			//stringBuilder.Append( new SecsValue( new object[] { (short)3, "ABC" } ) );
			textBox_example.Text = stringBuilder.ToString();

			TreeNode s1Node = new TreeNode( "S1" );
			AddTree( s1Node, new SecsTreeItem( 1, 1,  true,  null, "Are You Online" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 3,  true,  new SecsValue(new object[] { (uint)1, (uint)2 } ), "Selected Equipment Status" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 5,  true,  new SecsValue( new byte[] { 0x01 } ), "Formatted Status" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 7,  false, new SecsValue( new byte[] { 0x01 } ), "Fixed Form Request" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 9,  true,  null, "Material Transfer Status" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 11, true,  new SecsValue(new object[] { (uint)1, (uint)2 } ), "Status Variable Namelist" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 13, true,  null, "Establish Communications" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 15, true,  null, "Request OFF-LINE" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 17, true,  null, "Request ON-LINE" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 19, true,  new SecsValue( new object[] { "object class name", new object[] { "Job0001", "Job0002" }, new object[] { "attribute1", "attribute2" } } ), "Get Attribute" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 21, true,  new SecsValue( new object[] { "Variable ID1", "Variable ID2" } ), "Data Variable Namelist" ) );
			AddTree( s1Node, new SecsTreeItem( 1, 23, true,  new SecsValue( new object[] { (uint)1, (uint)2 } ), "Collection Event Namelist" ) );
			treeView1.Nodes.Add( s1Node );

			TreeNode s2Node = new TreeNode( "S2" );
			AddTree( s2Node, new SecsTreeItem( 2, 1, false, new SecsValue( new byte[] { 0x01, 0x02, 0x03 } ), "Service Program Load Grant" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 3, false, new SecsValue( new byte[] { 0x01, 0x02, 0x03 } ), "Service Program Send" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 5, false, new SecsValue( "bin00" ), "Service Program Load" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 7, false, new SecsValue( "bin00" ), "Service Program Run Send" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 9, false, new SecsValue( "bin00" ), "Service Program Results" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 11, false, null, "Service Program Directory" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 13, true, new SecsValue( new object[] { (uint)1, (uint)2 } ), "Equipment Constant" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 15, true, new SecsValue( new object[] { new object[] { (uint)1, "1" }, new object[] { (uint)2, "2" } } ), "New Equipment Constant Send" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 17, true, null, "Date and Time" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 19, true, new SecsValue((byte)1), "Reset/Initialize Send" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 21, true, new SecsValue( "pause" ), "Remote Command Send" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 23, true, new SecsValue( new object[] { "TRID", "000500", (uint)4, (uint)5, new object[] { (uint)1, (uint)2 } } ), "Trace Initialize Send" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 25, true, new SecsValue( "00 01 03 03 0a 0d 1b 5d 18 18 1a 04 13 7f 80 fe ff".ToHexBytes( ) ), "Loopback Diagnostic" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 27, true, new SecsValue( new object[] { new byte[] { 0x01 }, "banana", new object[] { "ee052793.1" } } ), "Initiate Processing Request" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 29, true, new SecsValue( new object[] { (uint)220 } ), "Equipment Constant Namelist" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 31, true, new SecsValue( DateTime.Now.ToString("yyyyMMddHHmmss") ), "Date and Time Set" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 33, true, new SecsValue( new object[] { (uint)1, new object[] { new object[] { (uint)4, new object[] { "810" } } } } ), "Define Report" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 35, true, new SecsValue( new object[] { (uint)1, new object[] { new object[] { (uint)4050, new object[] { (uint)1 } } } } ), "Link Event Report" ) );
			AddTree( s2Node, new SecsTreeItem( 2, 37, true, new SecsValue( new object[] { (uint)1, new object[] { new object[] { (uint)4050, new object[] { (uint)1 } } } } ), "Enable/Disable Event Report" ) );
			treeView1.Nodes.Add( s2Node );


			TreeNode s3Node = new TreeNode( "S3" );
			AddTree( s3Node, new SecsTreeItem( 3, 1, true, null, "Material Status" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 3, true, null, "Time to Completion Data" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 5, true, new SecsValue( new object[] { new byte[] { 0x01 }, new byte[] { 0x18 } } ), "Material Found Send" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 7, true, new SecsValue( new object[] { new byte[] { 0x01 }, new byte[] { 0x18 }, "ee052793.1" } ), "Material Lost Send" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 9, true, new SecsValue( new object[] { "ee052793.1", "1" } ), "Matl ID Equate Send" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 11, true, new SecsValue( (byte)1 ), "Matl ID Request" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 13, true, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Matl ID Send" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 17, true, new SecsValue( new object[] { (uint)1, "ProceedWithCarrier", "CSX 52078", (byte)1, new object[] { new object[] { "Usage", "product" } } } ), "Carrier Action Request" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 19, true, null, "Cancel All Carrier Out Req" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 21, true, new SecsValue( new object[] { "buffer1", (byte)1, new object[] { (byte)1 } } ), "Port Group Defn" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 23, true, new SecsValue( new object[] { "CancelReservationAtPort", "buffer1", new object[] { "ServiceStatus", (byte)1 } } ), "Port Group Action Req" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 25, true, new SecsValue( new object[] { " ChangeServiceStatus", (byte)1, new object[] { new object[] { "ServiceStatus",( byte)1 } } } ), "Port Action Req" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 27, true, new SecsValue( new object[] { (byte)0, new object[] { (byte)1 } } ), "Change Access" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 29, true, new SecsValue( new object[] { "logical ID", "Carrier:CSX 52078", "S01", (uint)649 } ), "Carrier Tag Read Req" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 31, true, new SecsValue( new object[] { "logical ID", "Carrier:CSX 52078", "S01", (uint)649, "unformatted data" } ), "Carrier Tag Write Data" ) );
			AddTree( s3Node, new SecsTreeItem( 3, 33, true, null, "Cancel All Pod Out Req" ) );
			treeView1.Nodes.Add( s3Node );


			TreeNode s4Node = new TreeNode( "S4" );
			AddTree( s4Node, new SecsTreeItem( 4, 1, true, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Ready to Send Materials" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 3, false, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Send Material" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 5, false, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Handshake Complete" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 9, false, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Stuck in Sender" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 11, false, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Stuck in Receiver" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 13, false, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Send Incomplete Timeout" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 15, false, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Material Received" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 17, true, new SecsValue( new object[] { (byte)1, "ee052793.1" } ), "Request to Receive" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 19, true, new SecsValue( new object[] { (uint)1, new object[] { "TJH_U_M_E1086", new object[] { (uint)3, (uint)1, "c000678", (uint)7, (byte)1, 
				"standard exchange", "AGV0001", (uint)1, (byte)1,(byte)1, (uint)1, "TF:1 0" } } } ), "Transfer Job Create" ) );
			AddTree( s4Node, new SecsTreeItem( 4, 21, true, new SecsValue( new object[]{ new byte[] { 0x60 }, "PAUSE", new object[] { new object[] { "ppexecname", "cmos168-zl0EC3" } } } ), "Transfer Job Command" ) );
			treeView1.Nodes.Add( s4Node );

			TreeNode s5Node = new TreeNode( "S5" );;
			AddTree( s5Node, new SecsTreeItem( 5, 3, true, new SecsValue( new object[] { new byte[] { 0x00 }, (uint)1000 } ), "Enable/Disable Alarm Send" ) );
			AddTree( s5Node, new SecsTreeItem( 5, 5, true, new SecsValue( (uint)0 ), "List Alarms Request" ) );
			AddTree( s5Node, new SecsTreeItem( 5, 7, true, null, "List Enabled Alarm Request" ) );
			AddTree( s5Node, new SecsTreeItem( 5, 9, true, new SecsValue( new object[] { "YYMMDDHHMMSS", "out of ink", "ALARM", "ink not sensed at nozzle inlet", new object[] { "manually insert new ink cartridge" } } ), "Exception Post Notify" ) );
			AddTree( s5Node, new SecsTreeItem( 5, 11, true, new SecsValue( new object[] { "YYMMDDHHMMSS", "out of ink", "ALARM", "ink not sensed at nozzle inlet" } ), "Exception Clear Notify" ) );
			AddTree( s5Node, new SecsTreeItem( 5, 13, true, new SecsValue( new object[] { "out of ink", "manually insert new ink cartridge" } ), "Exception Recover Request" ) );
			AddTree( s5Node, new SecsTreeItem( 5, 17, true, new SecsValue( "out of ink" ), "Exception Recovery Abort Request" ) );
			treeView1.Nodes.Add( s5Node );

			TreeNode s6Node = new TreeNode( "S6" );
			AddTree( s6Node, new SecsTreeItem( 6, 1, true, new SecsValue( new object[] { "1", (uint)10, "YYMMDDHHMMSS", new object[] { (byte)65 } } ), "Trace Data Send" ) );
			AddTree( s6Node, new SecsTreeItem( 6, 3, true, new SecsValue( new object[] { (uint)1, (uint)4050, new object[] { new object[] { "12", new object[] { new object[] { (uint)10, "54" } } } } } ), "Discrete Variable Data Send" ) );
			AddTree( s6Node, new SecsTreeItem( 6, 5, true, new SecsValue( new object[] { (uint)1, (uint)649 } ), "Multi-block Data Send Inquire" ) );
			AddTree( s6Node, new SecsTreeItem( 6, 7, true, new SecsValue( (uint)1 ), "Data Transfer Request" ) );
			AddTree( s6Node, new SecsTreeItem( 6, 9, true, new SecsValue( new object[] { new byte[] { 0x02 }, (uint)1, (uint)4050, new object[] { new object[] { "12", new object[] { "54" } } } } ), "Formatted Variable Send" ) );
			treeView1.Nodes.Add( s6Node );

			treeView1.AfterSelect += TreeView1_AfterSelect;
		}

		private void TreeView1_AfterSelect( object sender, TreeViewEventArgs e )
		{
			if (e.Node.Tag is SecsTreeItem treeItem)
			{
				textBox_stream.Text = treeItem.S.ToString( );
				textBox_function.Text = treeItem.F.ToString( );
				checkBox_back.Checked = treeItem.W;

				if (treeItem.Value != null)
					textBox_data.Text = treeItem.Value.ToXElement( ).ToString( );
				else
					textBox_data.Text = string.Empty;
			}
		}

		public void AddTree( TreeNode treeNode, SecsTreeItem treeItem )
		{
			TreeNode child = new TreeNode( $"S{treeItem.S}F{treeItem.F}{(treeItem.W ? "W" : "")} {treeItem.Description}" );
			child.Tag = treeItem;
			treeNode.Nodes.Add( child );
		}

		public class SecsTreeItem
		{
			public SecsTreeItem( byte s, byte f, bool w, SecsValue value, string decs )
			{
				this.S = s;
				this.F = f;
				this.W = w;
				this.Value = value;
				this.Description = decs;
			}

			public byte S { get; set; }
			public byte F { get; set; }
			public bool W { get; set; }
			public SecsValue Value { get; set; }
			public string Description { get; set; }
		}

		private void Language( int language )
		{
			if (language == 2)
			{
				Text = "SECS Read Demo";

				label1.Text = "Com:";
				label3.Text = "baudRate:";
				label21.Text = "station";
				button1.Text = "Connect";
				button2.Text = "Disconnect";
				button3.Text = "Active";

				label11.Text = "Address:";
				label12.Text = "length:";
				button25.Text = "Bulk Read";
				label13.Text = "Results:";

				groupBox3.Text = "Log";
			}
		}

		private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
		{

		}
		

		#region Connect And Close



		private void button1_Click( object sender, EventArgs e )
		{
			if(!int.TryParse(textBox2.Text,out int port ))
			{
				MessageBox.Show( DemoUtils.PortInputWrong );
				return;
			}

			secs?.ConnectClose( );
			secs = new SecsHsms( textBox3.Text, port );
			secs.DeviceID = ushort.Parse( textBox_deviceID.Text );
			secs.LogNet = new HslCommunication.LogNet.LogNetSingle( "" );
			secs.LogNet.BeforeSaveToFile += LogNet_BeforeSaveToFile;
			secs.OnSecsMessageReceived += Secs_OnSecsMessageReceived;
			secs.LogNet = LogNet;
			try
			{
				OperateResult connect = secs.ConnectServer( );
				if (connect.IsSuccess)
				{
					MessageBox.Show( HslCommunication.StringResources.Language.ConnectedSuccess );
					button2.Enabled = true;
					button1.Enabled = false;
					panel2.Enabled = true;

				}
				else
				{
					MessageBox.Show( HslCommunication.StringResources.Language.ConnectedFailed + connect.Message + Environment.NewLine +
						"Error: " + connect.ErrorCode );
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show( ex.Message );
			}
		}

		private void Secs_OnSecsMessageReceived( object sender, SecsMessage secsMessage )
		{
			// 当接收到数据返回的非应答数据时，需要处理的方法代码
			secs.LogNet?.WriteDebug( secs.ToString( ), secsMessage.ToString( ) );

		}

		private void LogNet_BeforeSaveToFile( object sender, HslCommunication.LogNet.HslEventArgs e )
		{
			if (InvokeRequired)
			{
				Invoke( new Action( ( ) => LogNet_BeforeSaveToFile( sender, e ) ) );
				return;
			}

			textBox10.AppendText( e.HslMessage.ToString( ) + Environment.NewLine );
		}


		private void button2_Click( object sender, EventArgs e )
		{
			// 断开连接
			secs.ConnectClose( );
			button2.Enabled = false;
			button1.Enabled = true;
			panel2.Enabled = false;
		}

		#endregion

		#region 批量读取测试

		private void button25_Click( object sender, EventArgs e )
		{
			OperateResult<SecsMessage> read = secs.ReadSecsMessage( byte.Parse( textBox_stream.Text ), byte.Parse( textBox_function.Text ), 
				string.IsNullOrEmpty(textBox_data.Text) ? new SecsValue() : new SecsValue(XElement.Parse( textBox_data.Text ) ), checkBox_back.Checked );
			if (read.IsSuccess)
			{
				textBox_receive.Text = DateTime.Now.ToString( "HH:mm:ss" ) + ": " + Environment.NewLine + read.Content.GetItemValues()?.ToString( );
			}
			else
			{
				MessageBox.Show( "读取失败！" + read.ToMessageShowString( ) );
			}
		}

		private void button3_Click( object sender, EventArgs e )
		{
			OperateResult send = secs.SendByCommand( byte.Parse( textBox_stream.Text ), byte.Parse( textBox_function.Text ),
				string.IsNullOrEmpty( textBox_data.Text ) ? new byte[0] : new SecsValue( XElement.Parse( textBox_data.Text ) ).ToSourceBytes( ), checkBox_back.Checked );
			if (send.IsSuccess)
			{
			   MessageBox.Show( "发送成功！" );
			}
			else
			{
				MessageBox.Show( "发送失败！" + send.ToMessageShowString( ) );
			}
		}
		private void button4_Click( object sender, EventArgs e )
		{

		}
		#endregion


		public override void SaveXmlParameter( XElement element )
		{
			element.SetAttributeValue( DemoDeviceList.XmlBaudRate, textBox2.Text );
			element.SetAttributeValue( DemoDeviceList.XmlStation, textBox_deviceID.Text );
		}

		public override void LoadXmlParameter( XElement element )
		{
			base.LoadXmlParameter( element );
			textBox2.Text = element.Attribute( DemoDeviceList.XmlBaudRate ).Value;
			textBox_deviceID.Text = element.Attribute( DemoDeviceList.XmlStation ).Value;
		}

		private void userControlHead1_SaveConnectEvent_1( object sender, EventArgs e )
		{
			userControlHead1_SaveConnectEvent( sender, e );
		}

		private void button_S1F1_Click( object sender, EventArgs e )
		{
			OperateResult<OnlineData> read = this.secs.Gem.S1F1_AreYouThere( );
			if (read.IsSuccess)
			{
				textBox_s1.Text = read.Content.ToJsonString( );
			}
			else
			{
				MessageBox.Show( "Read failed: " + read.Message );
			}
		}

		private void button_s1f11_Click( object sender, EventArgs e )
		{
			OperateResult<VariableName[]> read = null;
			if (string.IsNullOrEmpty( textBox4.Text ))
				read = this.secs.Gem.S1F11_StatusVariableNamelist( );
			else
				read = this.secs.Gem.S1F11_StatusVariableNamelist( textBox4.Text.ToStringArray<int>( ) );
			if (read.IsSuccess)
			{
				textBox_s1.Text = read.Content.ToJsonString( );
			}
			else
			{
				MessageBox.Show( "Read failed: " + read.Message );
			}
		}

		private void button_s1f13_Click( object sender, EventArgs e )
		{
			OperateResult<OnlineData> read = this.secs.Gem.S1F13_EstablishCommunications( );
			if (read.IsSuccess)
			{
				textBox_s1.Text = read.Content.ToJsonString( );
			}
			else
			{
				MessageBox.Show( "Read failed: " + read.Message );
			}
		}

		private void button_s1f15_Click( object sender, EventArgs e )
		{
			OperateResult<byte> read = this.secs.Gem.S1F15_OfflineRequest( );
			if (read.IsSuccess)
			{
				textBox_s1.Text = read.Content.ToString( ) + Environment.NewLine + "返回值说明，0: ok, 1: refused, 2: already online";
			}
			else
			{
				MessageBox.Show( "Read failed: " + read.Message );
			}
		}

		private void button_s1f17_Click( object sender, EventArgs e )
		{
			OperateResult<byte> read = this.secs.Gem.S1F17_OnlineRequest( );
			if (read.IsSuccess)
			{
				textBox_s1.Text = read.Content.ToString( ) + Environment.NewLine + "返回值说明，0: ok, 1: refused, 2: already online";
			}
			else
			{
				MessageBox.Show( "Read failed: " + read.Message );
			}
		}

		private void button_s2f13_Click( object sender, EventArgs e )
		{
			OperateResult<SecsValue> read = this.secs.Gem.S2F13_EquipmentConstantRequest( );
			if (read.IsSuccess)
			{
				textBox_s1.Text = read.Content.ToString( );
			}
			else
			{
				MessageBox.Show( "Read failed: " + read.Message );
			}
		}
	}
}
