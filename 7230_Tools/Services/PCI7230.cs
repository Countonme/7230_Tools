using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _7230_Tools.Services
{
    public static class PCI7230
    {
        // 引入 APS SDK 函數 (初始化 PCI-7230)
        [DllImport("APS168.dll")]
        public static extern int APS_initial(ref int boardId, int mode);

        // 設置數字輸出
        [DllImport("APS168.dll")]
        public static extern int APS_write_d_output(int BoardID, int Port, uint DO_Value);

        // 讀取數字輸入
        [DllImport("APS168.dll")]
        public static extern int APS_read_d_input(int BoardID, int Port, out uint DI_Value);

        // 關閉 PCI-7230
        [DllImport("APS168.dll")]
        public static extern int APS_close();
    }

}
