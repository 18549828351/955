using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static WindowsFormsApp1.formTool;

namespace WindowsFormsApp1
{
    public partial class sysMenu : Form
    {
        private static Dictionary<string, string> blackMap = new Dictionary<string, string>();
        private static int waittime = 5;
        private int ts = waittime;
        string soft = "";
        public sysMenu()
        {
            blackMap.Add("idea64.exe","Idea");
            blackMap.Add("eclipse.exe", "Eclipse");
            blackMap.Add("studio64.exe", "Android Studio");
            blackMap.Add("navicat.exe", "Navicat");
            blackMap.Add("devenv.exe", "Visual Studio");
            blackMap.Add("Code.exe", "Visual Studio Code");
            blackMap.Add("HBuilderX.exe", "HBuilder");
            blackMap.Add("CloudClient.exe", "云桌面客户端");
            InitializeComponent();
            this.BackColor = Color.Fuchsia;
            this.TransparencyKey = Color.Fuchsia;
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
        }

        private void Form1_Move(object sender, EventArgs e)
        {           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            DateTime now = DateTime.Now;
            string type = "work";
            var day = now.DayOfWeek;
            var time = now.TimeOfDay;
            if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
                type = "周末不允许加班，";
            else if (time.Hours > 19 || time.Hours < 6)
                type = "不允许上夜班，";
            else if (time.Hours < 9)
                type = "来的太早了，赶紧回去睡个回笼觉，";
            else if (time.Hours > 16)
                type = "17点之后禁止工作，";
            if (!"work".Equals(type))
            {
                ts--;
                if (ts < 0)
                {                    
                    if (soft.Length > 1)//倒计时结束有软件运行，显示桌面,清空显示，倒计时重启
                    {
                        Type oleType = Type.GetTypeFromProgID("Shell.Application");
                        object oleObject = System.Activator.CreateInstance(oleType);
                        oleType.InvokeMember("ToggleDesktop", BindingFlags.InvokeMethod, null, oleObject, null);
                        soft = "";
                    }
                    else//倒计时结束无软件运行,清空显示，倒计时重启
                    {
                        soft = getSoft();
                    }
                    this.label1.Text = "";
                    this.label2.Text = "";
                    ts = waittime;
                }
                else
                {
                    if (soft.Length > 1)//倒计时没结束有软件运行，刷新运行清单，显示提示
                    {
                        soft = getSoft();
                        this.label1.Text = type + "请尽快最小化或关闭以下软件：倒计时：" + ts;
                        this.label2.Text = soft;
                    }
                    else//倒计时没结束无软件运行,清空显示，倒计时重启
                    {
                        this.label1.Text = "";
                        this.label2.Text = "";
                    }
                }
            }
            else
            {
                soft = "";
                ts = waittime;
            }
        }
        private string getSoft() {
            string soft = "";
            var openWindowProcesses = System.Diagnostics.Process.GetProcesses();
            for (int i = 0; i < openWindowProcesses.Length; i++)
            {
                try
                {
                    int jb = (int)openWindowProcesses[i].MainWindowHandle;
                    if (jb == 0)
                        continue;
                    string exename = openWindowProcesses[i].MainModule.ModuleName;
                    if (IsIconic(jb))
                        continue;
                    string softname = "";
                    if (blackMap.TryGetValue(exename, out softname))
                    {
                        soft += softname + ",";
                    }
                }
                catch (Exception err)
                {
                }
            }
            return soft;
        }
      
    }
}
