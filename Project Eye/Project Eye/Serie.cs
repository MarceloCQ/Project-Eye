using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Project_Eye
{
    
    class Serie
    {
        //Atributos
        public string Nombre { get; set; } //Nombre de la serie
        public int Id { get; set; }       //Id de la serie
        public int[] SigEp { get; set; }  //Siguiente episodio a ver / descargar
        public char Estado { get; set; } //Estado de la serie
        public List<List<Episodio>> Episodios { get; set; } //Matriz de episodios de la serie
        public int PorVer { get; set; } //Cantidad de episodios por ver
        public int descargando;          //Cantidad de episodios por descargar
        private XmlDocument doc;

        //Eventos
        public event PropertyChangedEventHandler PropertyChanged;

        public int Descargando
        {
            get { return descargando; }
            set
            {
                if (value != descargando)
                {
                    descargando = value;
                    OnPropertyChanged("Descargando");
                }
            }
        }

        //Constructor
        public Serie(int id, int temporada, int capitulo)
        {
            Id = id;
            SigEp = new int[2];
            SigEp[0] = temporada;
            SigEp[1] = capitulo;

            //Se carga el documento de donde se saca la informacion
            doc = new XmlDocument();
            doc.Load(@"http://thetvdb.com/api/97AAE7796E3F60D2/series/"+ Id + "/all/en.xml");

            Nombre = doc.SelectSingleNode("/Data/Series/SeriesName").InnerText;
            Estado = doc.SelectSingleNode("/Data/Series/Status").InnerText == "Continuing" ? 'c' : 'e';
            
            WebClient wc = new WebClient();
            wc.DownloadFile("http://thetvdb.com/banners/" + doc.SelectSingleNode("/Data/Series/fanart").InnerText, @"C:\Users\Marcelo\Documents\Project Eye\Project-Eye\Interfaz\Fanart\" + Nombre + ".jpg");

            PorVer = 0;
            Descargando = 0;



        }

        public void AddEpisodes()
        {
            Episodios = new List<List<Episodio>>();
            XmlNodeList episodios = doc.SelectNodes("/Data/Episode");
            string nombreSerie, nombreEp, calidad;
            int temporada, capitulo, estado;
            DateTime Fecha;
            nombreSerie = Nombre;
            calidad = "720p";
            List<Episodio> aux = new List<Episodio>();
            int tempAct = 1;
            
            foreach (XmlNode e in episodios)
            {
                if (e.SelectSingleNode("SeasonNumber").InnerText != "" && e.SelectSingleNode("SeasonNumber").InnerText != "0")
                {
                    if (tempAct < Convert.ToInt32(e.SelectSingleNode("SeasonNumber").InnerText))
                    {
                        Episodios.Add(aux);
                        aux = new List<Episodio>();
                        tempAct++;
                    }

                    nombreEp = e.SelectSingleNode("EpisodeName").InnerText;
                    temporada = Convert.ToInt32(e.SelectSingleNode("SeasonNumber").InnerText);
                    capitulo = Convert.ToInt32(e.SelectSingleNode("EpisodeNumber").InnerText);
                    string fecha2 = e.SelectSingleNode("FirstAired").InnerText;
                    Fecha = new DateTime(Convert.ToInt32(fecha2.Substring(0, 4)), Convert.ToInt32(fecha2.Substring(5, 2)), Convert.ToInt32(fecha2.Substring(8, 2)));
                    Fecha = Fecha.AddDays(1.5);

                    if (Fecha < DateTime.Now)
                    {
                        estado = 1;
                    }
                    else
                    {
                        estado = 0;
                    }

                    aux.Add(new Episodio(nombreEp, Nombre, temporada, capitulo, estado, Fecha, calidad));
                    Descargando++;

                    

                }

                
            }

            Episodios.Add(aux);

            
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
