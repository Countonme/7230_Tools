using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using _7230_Tools.Services;
using Sunny.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _7230_Tools
{
    public partial class Form1 : UIForm
    {
        // 定义 PCI-7230 设备类型 (从文档或头文件获取)
        const ushort PCI_7230 = 6;  // 设备型号
        private ushort CARD_NUMBER = 0;   // 设备编号
        private short cardHandle = -1;  // 设备句柄
        private Timer timer;
        private ushort input = 0, output = 0;
        private ushort oldinput = 0;



        private Dictionary<int, UIRadioButton> portToRadioButtonMap;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.switchCotrol.Click += SwitchControl_Click;
            InitializePortMapping();
            this.cb_do_0.Click += Cb_do_Click;
            this.cb_do_1.Click += Cb_do_Click;
            this.cb_do_2.Click += Cb_do_Click;
            this.cb_do_3.Click += Cb_do_Click;
            this.cb_do_4.Click += Cb_do_Click;
            this.cb_do_5.Click += Cb_do_Click;
            this.cb_do_6.Click += Cb_do_Click;
            this.cb_do_7.Click += Cb_do_Click;
            this.cb_do_8.Click += Cb_do_Click;
            this.cb_do_9.Click += Cb_do_Click;
            this.cb_do_10.Click += Cb_do_Click;
            this.cb_do_11.Click += Cb_do_Click;
            this.cb_do_12.Click += Cb_do_Click;
            this.cb_do_13.Click += Cb_do_Click;
            this.cb_do_14.Click += Cb_do_Click;
            this.cb_do_15.Click += Cb_do_Click;
        }

        /// <summary>
        /// IO 點擊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_do_Click(object sender, EventArgs e)
        {
            var cb= (UICheckBox)sender;
            var sb = new StringBuilder();
            //sb.Append("0b");
            sb.Append($"{cb_do_15.Checked}");
            sb.Append($"{cb_do_14.Checked}");
            sb.Append($"{cb_do_13.Checked}");
            sb.Append($"{cb_do_12.Checked}");
            sb.Append($"{cb_do_11.Checked}");
            sb.Append($"{cb_do_10.Checked}");
            sb.Append($"{cb_do_9.Checked}");
            sb.Append($"{cb_do_8.Checked}");
            sb.Append($"{cb_do_7.Checked}");
            sb.Append($"{cb_do_6.Checked}");
            sb.Append($"{cb_do_5.Checked}");
            sb.Append($"{cb_do_4.Checked}");
            sb.Append($"{cb_do_3.Checked}");
            sb.Append($"{cb_do_2.Checked}");
            sb.Append($"{cb_do_1.Checked}");
            sb.Append($"{cb_do_0.Checked}");
            var value = sb.ToString().Replace("True", "1").Replace("False", "0");
           // MessageBox.Show(value);

            DO_WritePort(cb, Convert.ToUInt16(value, 2));
        }

        private void InitializePortMapping()
        {
            // 初始化端口号和 RadioButton 的映射关系
            portToRadioButtonMap = new Dictionary<int, UIRadioButton>
        {
            { 0, radio_di_0 },
            { 1, radio_di_1 },
             { 2, radio_di_2 },
            { 3, radio_di_3 },
             { 4, radio_di_4 },
            { 5, radio_di_5 },
             { 6, radio_di_6 },
            { 7, radio_di_7 },
             { 8, radio_di_8 },
            { 9, radio_di_9 },
             { 10, radio_di_10 },
            { 11, radio_di_11 },
             { 12, radio_di_12 },
            { 13, radio_di_13 },
             { 14, radio_di_14 },
            { 15, radio_di_15 },
            // 如果有更多端口，可以在这里继续添加
        };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化定时器
            timer = new Timer();
            timer.Interval = 50; // 每50毫秒触发一次
            timer.Tick += Timer_Tick;
        }

        private void ConnectPCI_7230()
        {
            CARD_NUMBER = ushort.Parse(comboBox1.Text);
            cardHandle = PCI7230.Register_Card(PCI_7230, CARD_NUMBER);
            if (cardHandle < 0)
            {
                var message = $"PCI-7230 设备连接失败! 错误码: {cardHandle}";
                switchCotrol.Active = false;
                this.ShowErrorNotifier(message);
                ShowLog(message);
                ShowLog("请检查设备安装是否正确，驱动程序是否安装，设备编号是否正确。");
            }
            else
            {
                ledStatus.Color = System.Drawing.Color.Lime;
                ledStatus.Blink = true;
                switchCotrol.Active = true;
                ShowLog($"PCI-7230 设备连接成功! 句柄: {cardHandle}");
                timer.Start(); // 开始定时器
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (cardHandle >= 0)
            {
                ushort input;
                PCI7230.DI_ReadPort(cardHandle, 0, out input);
                var binString = input.ToBinString();
                // 读取数字输入
                // 遍历所有端口并读取数据
                for (int i = 0; i < binString.Length; i++)
                {
                    
                        var value = int.Parse(binString[i].ToString());
                        // 更新对应的 RadioButton 状态
                        if (portToRadioButtonMap.TryGetValue((binString.Length-1)-i, out var radioButton))
                        {
                            radioButton.Checked = value > 0;
                            radioButton.Refresh();

                            // 记录日志
                            ShowLog($" 数据: {input.ToBinString()} ");
                        }
                    
                }

            }
        }

        private void SwitchControl_Click(object sender, EventArgs e)
        {
            ConnectPCI_7230();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cardHandle >= 0)
            {
                PCI7230.Release_Card(cardHandle);
                Console.WriteLine("设备已断开连接!");
                timer.Stop(); // 停止定时器
            }
        }

        private void radio_do_0_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// 顯示日志
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



        /// <summary>
        /// 控制IO 輸出
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="port"></param>
        private void DO_WritePort(UICheckBox btn, ushort value)
        {

            if (cardHandle >= 0)
            {
                try
                {
                    // 获取当前 CheckBox 的状态
                    bool isChecked = btn.Checked;

                    // 显示调试信息
                    //MessageBox.Show($"btnStatus={isChecked}, Port={port}");

                    // 根据 CheckBox 状态设置输出值
                    ushort outputValue = isChecked ? (ushort)1 : (ushort)0;

                    // 调用 DLL 函数写入端口
                    short result = PCI7230.DO_WritePort(cardHandle, 0,value );

                    // 检查返回值
                    if (result < 0)
                    {
                        // 写入失败，显示错误信息
                        ShowLog($"写入 {value} 失败！错误码: {result}");
                        btn.Checked = !isChecked; // 恢复 CheckBox 状态
                        this.ShowErrorNotifier($"写入 {value} 失败！错误码: {result}");
                    }
                    else
                    {
                        // 写入成功，记录日志
                        ShowLog($"写入 {value} 成功！输出值: {outputValue}");
                        this.ShowSuccessNotifier($"写入 {value} 成功！输出值: {outputValue}");
                    }
                }
                catch (Exception ex)
                {
                    // 捕获异常并记录日志
                    ShowLog($"发生异常: {ex.Message}");
                    this.ShowErrorDialog(ex.Message);
                }
            }
            else
            {
                btn.Checked = false;
                this.ShowErrorNotifier("未鏈接");
            }

        }
    }
}