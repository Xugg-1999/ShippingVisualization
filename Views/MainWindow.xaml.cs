using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace ShippingVisualization
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
        }
        /// <summary>
        /// 全屏加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseImageButton_MouseDown(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShippingDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRow = ShippingDataGrid.SelectedItem as DeliveryNotice;
            if (selectedRow != null)
            {
                var viewModel = DataContext as MainWindowViewModel;
                viewModel.HandleRowClick(selectedRow);  
            }
        }
        /// <summary>
        /// 处理下拉框值改变事件
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedValue = selectedItem.Content.ToString();
                if (int.TryParse(selectedValue, out int refreshInterval))
                {
                    var viewModel = DataContext as MainWindowViewModel;
                    if (viewModel != null)
                    {
                        viewModel.UpdateRefreshInterval(refreshInterval);
                    }
                }
            }
        }


    }
}

