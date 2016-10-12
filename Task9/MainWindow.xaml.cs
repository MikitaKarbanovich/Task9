using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;


namespace Task9
{
    public partial class MainWindow : Window
    {
        public enum ShowMode
        {
            All = 0,
            Methods,
            Properties,
            Fields,
        }
        ShowMode showMode = ShowMode.All;
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void ChooseDirectoryClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lblSelectedFolder.Text = fbd.SelectedPath;
                listViewOfDll.Items.Clear();
                listViewOfTypes.Items.Clear();
                ClearMethodsPropertiesFields();
                Selectfolders(fbd.SelectedPath);
            }
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Selectfolders(string filename)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(filename);
                FileInfo[] info = dirInfo.GetFiles("*.dll*", SearchOption.AllDirectories);
                foreach (var fileName in info)
                {
                    listViewOfDll.Items.Add(fileName);
                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                System.Windows.Forms.MessageBox.Show("You have not access to this folder", "No access", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListViewOfDllSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewOfTypes.Items.Clear();
            ClearMethodsPropertiesFields();
            var selectedItem = listViewOfDll.SelectedItem;
            try
            {
                {
                    var types = WorkWithFile.GetTypes(lblSelectedFolder.Text + "\\" + selectedItem.ToString());
                    foreach (var type in types)
                    {
                        listViewOfTypes.Items.Add(type);
                    }
                }
            }
            catch (System.BadImageFormatException e1)
            {
                System.Windows.Forms.MessageBox.Show("No access", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.IO.FileNotFoundException e2)
            {
                System.Windows.Forms.MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.NullReferenceException e3)
            {
                System.Windows.Forms.MessageBox.Show("Directory was chanched", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void ListViewOfTypesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewOfTypes.SelectedItem != null)
            {
                var methods = WorkWithFile.GetMethods((Type)listViewOfTypes.SelectedItem);
                var properties = WorkWithFile.GetProperties((Type)listViewOfTypes.SelectedItem);
                var fields = WorkWithFile.GetFields((Type)listViewOfTypes.SelectedItem);
                ClearMethodsPropertiesFields();
                if (showMode == ShowMode.Methods || showMode == ShowMode.All)
                {
                    foreach (var method in methods)
                    {
                        listViewOfMethods.Items.Add(method.ToString());
                    }
                }
                if (showMode == ShowMode.Properties || showMode == ShowMode.All)
                {
                    foreach (var property in properties)
                    {
                        listViewOfProperties.Items.Add(property.ToString());
                    }
                }
                if (showMode == ShowMode.Fields || showMode == ShowMode.All)
                {
                    foreach (var field in fields)
                    {
                        listViewOfFields.Items.Add(field.ToString());
                    }
                }
            }
        }

        private void RbPropertiesChecked(object sender, RoutedEventArgs e)
        {

            showMode = ShowMode.Properties;
            listViewOfMethods.Items.Clear();
            listViewOfFields.Items.Clear();
        }

        private void RbFieldsChecked(object sender, RoutedEventArgs e)
        {
            showMode = ShowMode.Fields;
            listViewOfMethods.Items.Clear();
            listViewOfProperties.Items.Clear();
        }

        private void RbMethodsChecked(object sender, RoutedEventArgs e)
        {
            showMode = ShowMode.Methods;
            listViewOfProperties.Items.Clear();
            listViewOfFields.Items.Clear();
        }

        private void RbShowAllChecked(object sender, RoutedEventArgs e)
        {
            showMode = ShowMode.All;
        }
        public void ClearMethodsPropertiesFields()
        {
            listViewOfMethods.Items.Clear();
            listViewOfProperties.Items.Clear();
            listViewOfFields.Items.Clear();
        }
    }
}
