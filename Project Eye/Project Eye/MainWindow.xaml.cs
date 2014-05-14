using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
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

namespace Project_Eye
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Episodio> p = new List<Episodio>();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
          
                BackgroundWorker b = new BackgroundWorker();
                //b.WorkerReportsProgress = true;
                b.DoWork += (seneder, ee) =>
                    {
                        for (int i = 1; i < 23; i++)
                        {
                            Episodio k = new Episodio("Lalalal", "Modern Family", 3, i, 1, new DateTime(2048, 2, 2), "720p");
                            System.Diagnostics.Process.Start(k.Link);
                            p.Add(k);
                            labe.Dispatcher.Invoke(new Action(() =>
                            {
                               labe.Content = i.ToString();
                            }
                    ));
                        }
                    };

              
            

                b.RunWorkerAsync();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
