using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using DronePulse;

#nullable enable

static class Program
{
    [STAThread]
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? portName = configuration.GetValue<string>("SerialPortSettings:PortName");
        int baudRate = configuration.GetValue<int>("SerialPortSettings:BaudRate");

        if (string.IsNullOrEmpty(portName))
        {
            MessageBox.Show("Error: PortName is not configured in appsettings.json", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var mainForm = new MainForm
        {
            PortName = portName,
            BaudRate = baudRate
        };

        Application.Run(mainForm);
    }
}