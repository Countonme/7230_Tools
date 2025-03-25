using System.Runtime.InteropServices;

namespace _7230_Tools.Services
{
    public static class PCI7230
    {
        // P/Invoke声明
        [DllImport("PCI-Dask.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern short Register_Card(ushort CardType, ushort card_num);
        [DllImport("PCI-Dask.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern short Release_Card(short CardNumber);


        [DllImport("PCI-Dask.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern short DI_ReadPort(short CardNumber, ushort port, out ushort value);


        [DllImport("PCI-Dask.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern short DO_WritePort(short CardNumber, ushort port, ushort value);
    }

}
