using System;
using _7230_Tools.Services;
using Sunny.UI;

namespace _7230_Tools
{
    public partial class Form1 : UIForm
    {  // 2. 定义 PCI-7230 设备类型 (从文档或头文件获取)
        const ushort PCI_7230 = 0x20;  // **设备型号编号**
        const ushort CARD_NUMBER = 0;  // **设备编号（通常是 0）**

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 3. 设备注册
            short cardHandle = PCI7230.Register_Card(PCI_7230, CARD_NUMBER);
            if (cardHandle < 0)
            {
                Console.WriteLine($"设备注册失败, 错误代码: {cardHandle}");
                return;
            }
            Console.WriteLine("PCI-7230 设备注册成功!");

            // 4. 读取 DI 端口
            uint diValue;
            short diStatus = PCI7230.DI_ReadPort((ushort)cardHandle, 0, out diValue);
            if (diStatus == 0)
            {
                Console.WriteLine($"DI 端口数据: {Convert.ToString(diValue, 2).PadLeft(16, '0')}");
            }
            else
            {
                Console.WriteLine($"读取 DI 失败, 错误代码: {diStatus}");
            }

            // 5. 写入 DO 端口
            uint doValue = 0x000F; // 低 4 位输出高电平 (0000 0000 0000 1111)
            short doStatus = PCI7230.DO_WritePort((ushort)cardHandle, 0, doValue);
            if (doStatus == 0)
            {
                Console.WriteLine("DO 端口写入成功!");
            }
            else
            {
                Console.WriteLine($"写入 DO 失败, 错误代码: {doStatus}");
            }

            // 6. 释放设备
            PCI7230.Release_Card((ushort)cardHandle);
            Console.WriteLine("设备释放完成.");
        }
    }
}
