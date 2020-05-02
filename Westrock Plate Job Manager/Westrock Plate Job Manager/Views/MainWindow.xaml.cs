using System.Windows;

namespace Westrock_Plate_Job_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InventoryManager_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ScheduleManager_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    int index = int.Parse(((Button)e.Source).Uid);

        //    GridCursor.Margin = new Thickness(50 + (100 * index), 0, 0, 0);

        //    switch (index)
        //    {
        //        case 0:
        //            GridMain.Background = Brushes.Aquamarine;
        //            break;
        //        case 1:
        //            GridMain.Background = Brushes.Beige;
        //            break;
        //        case 2:
        //            GridMain.Background = Brushes.CadetBlue;

        //            break;
        //        case 3:
        //            //GridMain.Background = Brushes.DarkBlue;
        //            GridMain = InventoryManager_Loaded(object sender, RoutedEventArgs e);
        //            break;
        //        case 4:
        //            GridMain.Background = Brushes.Firebrick;
        //            break;
        //        case 5:
        //            GridMain.Background = Brushes.Gainsboro;
        //            break;
        //        case 6:
        //            GridMain.Background = Brushes.HotPink;
        //            break;
        //    }
        //}
    }
}
