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

            s = new Serie(75760, 1, 1);
          
                BackgroundWorker b = new BackgroundWorker();
                //b.WorkerReportsProgress = true;
                b.DoWork += (seneder, ee) =>
                    {
                            
                        s.AddEpisodes();

                            
                    };
                s.PropertyChanged += (sen, eee) =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (ThreadStart)delegate { labe.Content = s.Descargando; });
                        
                    };
              
            

                b.RunWorkerAsync();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(s.Episodios[0][0].Temporada.ToString());
            /*
            foreach (List<Episodio> p in s.Episodios)
            {
                foreach (Episodio m in p)
                {
                    MessageBox.Show(m.Temporada.ToString() + " " + m.Capitulo.ToString());
                }
            }
*/
        }
    }
}
