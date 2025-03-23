using System;
using System.Windows.Forms;
using _7230_Tools.Services;
using Sunny.UI;

namespace _7230_Tools
{
    public partial class Form1 : UIForm
    {  // 2. 定义 PCI-7230 设备类型 (从文档或头文件获取)

        //设备类型 & 设备编号
        const ushort PCI_7230 = 0x20;  // 设备型号
        const ushort CARD_NUMBER = 0;   // 设备编号
        private short cardHandle = -1;  // 设备句柄
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }


        /// <summary>
        /// 连接 PCI-7230 设备
        /// </summary>
        public void ConnectPCI_7230()
        {
            cardHandle = PCI7230.Register_Card(PCI_7230, CARD_NUMBER);
            if (cardHandle < 0)
            {

                var message = $"PCI-7230 设备连接失败! 错误码: {cardHandle}";

                switchCotrol.Active = false;
                this.ShowErrorNotifier(message);
                ShowLog(message);
            }
            else
            {
                ledStatus.Color = System.Drawing.Color.Lime;
                ledStatus.Blink = true;
                switchCotrol.Active = true;
                ShowLog($"PCI-7230 设备连接成功! 句柄: {cardHandle}");

            }
        }
        /// <summary>
        /// Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.switchCotrol.Click += SwitchControl_Click;
            this.FormClosing += Form1_FormClosing;

        }
        /// <summary>
        /// 实时监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Switch_Monitor_in_Real_time_Click(object sender, EventArgs e)
        {
            if (cardHandle > 0)
            {
                //启用监控


                return;
            }
        }
        /// <summary>
        /// 连接7230
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchControl_Click(object sender, EventArgs e)
        {
            ConnectPCI_7230();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cardHandle >= 0)
            {
                PCI7230.Release_Card((ushort)cardHandle);
                Console.WriteLine("设备已断开连接!");
            }
        }


        /// <summary>
        /// 显示log
        /// </summary>
        /// <param name="message"></param>
        private void ShowLog(string message)
        {

            listLog.BeginInvoke(new Action(() =>
            {
                listLog.Items.Add(message);
                if (listLog.Items.Count > 30)
                {
                    listLog.Items.Clear();

                }

            }));
        }
    }
}
