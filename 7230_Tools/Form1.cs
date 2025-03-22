using _7230_Tools.Services;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _7230_Tools
{
    public partial class Form1 : UIForm
    {

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int boardId = 0;

            // 初始化板卡
            if (PCI7230.APS_initial(ref boardId, 0) != 0)
            {
               this.ShowErrorDialog("初始化失敗，請檢查設備連接。");
                return;
            }

            this.ShowSuccessNotifier("PCI-7230 初始化成功！");

            // 寫入數字輸出 (例如 Port 0, 開啟第 1、3 位)
            PCI7230.APS_write_d_output(boardId, 0, 0b00000101);  // 0x05 => 第1、3位高電平
            Console.WriteLine("已設置數字輸出：0x05");

            // 讀取數字輸入
            PCI7230.APS_read_d_input(boardId, 0, out uint diValue);
            Console.WriteLine($"數字輸入狀態：0x{diValue:X2}");

            // 關閉 PCI-7230
            PCI7230.APS_close();
            Console.WriteLine("PCI-7230 已關閉。"); throw new NotImplementedException();
        }
    }
}
