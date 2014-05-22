using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using UTorrentAPI;

namespace Project_Eye
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Episodio> p = new List<Episodio>();
        UTorrentClient uClient;
        Serie s;
        public MainWindow()
        {
            InitializeComponent();
            uClient = new UTorrentClient(new Uri("http://127.0.0.1:8080/gui/"), "admin", "admin", 1000000);
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            BackgroundWorker c = new BackgroundWorker();
            c.DoWork += (selo, epo) =>
                {
                    s = new Serie(76290);
                    s.PropertyChanged += (sen, eee) =>
                    {
                        if (eee.PropertyName == "Descargando")
                            Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal,
                            (ThreadStart)delegate { labe.Content = s.Descargando; });
                    };
                };

            c.RunWorkerAsync();

            c.RunWorkerCompleted += (epos, lalas) =>
                {
                    BackgroundWorker b = new BackgroundWorker();
                    b.DoWork += (ma, it) =>
                        {
                            s.addSerie(9, 1);
                        };
                    b.RunWorkerAsync();
                    List<int> l = new List<int>();
                    for (int i = 1; i <= s.Episodios.Count; i++)
                    {
                        l.Add(i);
                    }

                    tempas.ItemsSource= l;
                    Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal,
                            (ThreadStart)delegate { load.Visibility = System.Windows.Visibility.Hidden; });
                    tempas.SelectionChanged += (rola, pola) =>
                        {
                            if (tempas.SelectedIndex != -1)
                            {
                                capos.Items.Clear();
                                for (int i = 1; i <= s.Episodios[tempas.SelectedIndex].Count; i++)
                                {
                                    
                                    capos.Items.Add(i);
                                }
                            }
                        };

                };
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            MessageBox.Show(s.Imprimir());


        }
    }
}
