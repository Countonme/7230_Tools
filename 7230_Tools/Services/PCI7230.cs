using System.Runtime.InteropServices;

namespace _7230_Tools.Services
{
    public static class PCI7230
    {
        // 引入 APS SDK 函數 (初始化 PCI-7230)
        [DllImport("PCI-Dask.dll")]
        public static extern short Register_Card(ushort cardType, ushort cardNum);

        [DllImport("PCI-Dask.dll")]
        public static extern short Release_Card(ushort cardHandle);

        [DllImport("PCI-Dask.dll")]
        public static extern short DI_ReadPort(ushort cardHandle, ushort port, out uint value);

        [DllImport("PCI-Dask.dll")]
        public static extern short DO_WritePort(ushort cardHandle, ushort port, uint value);
    }

}
