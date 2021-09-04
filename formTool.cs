using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFormsApp1
{
    public class formTool
    {
        private const int WS_VISIBLE = 268435456;//窗体可见
        private const int WS_MINIMIZEBOX = 131072;//有最小化按钮
        private const int WS_MAXIMIZEBOX = 65536;//有最大化按钮
        private const int WS_BORDER = 8388608;//窗体有边框
        private const int GWL_STYLE = (-16);//窗体样式
        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        [DllImport("User32.dll")]
        private extern static int GetWindow(int hWnd, int wCmd);
        [DllImport("User32.dll")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);
        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool IsIconic(int hwnd);

        //获得窗体句柄和标题
        //List<FromInfo> fromInfo = GetHandleList(this.Handle.ToInt32());
        ////隐藏窗体
        //ShowWindow(this.Handle.ToInt32(), SW_HIDE);
        ////显示窗体
        //ShowWindow(this.Handle.ToInt32(), SW_SHOW);

        //获得包含窗体可见、有边框、有最大化按钮的窗体的句柄和标题(窗体的属性出这几种外还有很多种)
        public static List<FromInfo> GetHandleListShow(int Handle)
        {
            List<FromInfo> fromInfo = new List<FromInfo>();
            int handle = GetWindow(Handle, GW_HWNDFIRST);
            while (handle > 0)
            {
                int IsTask = WS_VISIBLE;//| WS_BORDER | WS_MAXIMIZEBOX;//窗体可见、有边框、有最大化按钮
                int lngStyle = GetWindowLongA(handle, GWL_STYLE);
                bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                
                if (TaskWindow&& !IsIconic(handle))
                {
                    int length = GetWindowTextLength(new IntPtr(handle));
                    StringBuilder stringBuilder = new StringBuilder(2 * length + 1);
                    GetWindowText(handle, stringBuilder, stringBuilder.Capacity);
                    
                    string strTitle = stringBuilder.ToString();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
                        fromInfo.Add(new FromInfo(strTitle, handle));
                    }
                    //else
                    //{
                    //    fromInfo.Add(new FromInfo("", handle));
                    //}
                }
                handle = GetWindow(handle, GW_HWNDNEXT);
            }
            return fromInfo;
        }
        //获得所有窗体的句柄和标题
        public static List<FromInfo> GetHandleList(int Handle)
        {
            List<FromInfo> fromInfo = new List<FromInfo>();
            int handle = GetWindow(Handle, GW_HWNDFIRST);
            while (handle > 0)
            {
                int length = GetWindowTextLength(new IntPtr(handle));
                StringBuilder stringBuilder = new StringBuilder(2 * length + 1);
                GetWindowText(handle, stringBuilder, stringBuilder.Capacity);
                string strTitle = stringBuilder.ToString();
                if (!string.IsNullOrEmpty(strTitle))
                {
                    fromInfo.Add(new FromInfo(strTitle, handle));
                }
                else
                {
                    fromInfo.Add(new FromInfo("", handle));
                }
                handle = GetWindow(handle, GW_HWNDNEXT);
            }
            return fromInfo;
        }
        public class FromInfo
        {
            public FromInfo(string title, int handle)
            {
                this.title = title;
                this.handle = handle;
            }
            private string title;
            private int handle;
            public string Title
            {
                get { return title; }
                set { title = value; }
            }
            public int Handle
            {
                get { return handle; }
                set { handle = value; }
            }
        }        
    }
}
