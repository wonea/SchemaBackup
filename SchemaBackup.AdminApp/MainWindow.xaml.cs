using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SchemaBackup.Core;
using SchemaBackup.AdminApp.Properties;

namespace SchemaBackup.AdminApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsSerialisation SettingsSerialisation;

        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            SettingsSerialisation.Dispose();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsSerialisation = new SettingsSerialisation(Settings.Default.SettingFileLocation);
            SettingsSerialisation.Load();
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutwindow = new AboutWindow();
            aboutwindow.Show();
        }
    }
}