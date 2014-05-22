using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace Project_Eye
{
    
    class Serie
    {
        //Atributos
        public string Nombre { get; set; } //Nombre de la serie
        public int Id { get; set; }       //Id de la serie
        private int temporada;  //Temporada de siguiente episodio a ver / descargar
        private int capitulo;   //Capitulo de siguiente episodio a ver / descargar
        public char Estado { get; set; } //Estado de la serie
        public List<List<Episodio>> Episodios { get; set; } //Matriz de episodios de la serie
        public int porVer { get; set; } //Cantidad de episodios por ver
        private int descargando;          //Cantidad de episodios por descargar
        public int Numserie = 60;             //Numero de la serie
        private XmlDocument doc;        //Documento de donde se extrae la informacion de la serie

        //Eventos
        public event PropertyChangedEventHandler PropertyChanged;

        //GetSet
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

        public int Temporada
        {
            get { return temporada; }
            set
            {
                if (value != temporada)
                {
                    temporada = value;
                    OnPropertyChanged("Temporada");
                }
            }
        }

        public int Capitulo
        {
            get { return capitulo; }
            set
            {
                if (value != capitulo)
                {
                    capitulo = value;
                    OnPropertyChanged("Capitulo");
                }
            }
        }

        public int PorVer
        {
            get { return porVer; }
            set
            {
                if (value != porVer)
                {
                    porVer = value;
                    OnPropertyChanged("PorVer");
                }
            }
        }



        /// <summary>
        /// Metodo constructor de la serie
        /// </summary>
        /// <param name="id">Id de la serie</param>
     
        public Serie(int id)
        {
            Id = id;

            //Se carga el documento de donde se saca la informacion
            doc = new XmlDocument();
            doc.Load(@"http://thetvdb.com/api/97AAE7796E3F60D2/series/"+ Id + "/all/en.xml");

            //Se pone su nombre y estado
            Nombre = doc.SelectSingleNode("/Data/Series/SeriesName").InnerText;
            Estado = doc.SelectSingleNode("/Data/Series/Status").InnerText == "Continuing" ? 'c' : 'e';
         
            PorVer = 0;
            Descargando = 0;

            AddEpisodes();



        }

        private void AddEpisodes()
        {
            //Se declaran las variables
            Episodios = new List<List<Episodio>>();
            XmlNodeList episodios = doc.SelectNodes("/Data/Episode");
            string nombreSerie, nombreEp, calidad;
            int tempo, capi, estado;
            DateTime Fecha;

            //Se ponen los parametros
            nombreSerie = Nombre;
            calidad = "720p";
            List<Episodio> aux = new List<Episodio>();
            int tempAct = 1;

            //Se ingresan los episodios
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
                    tempo = Convert.ToInt32(e.SelectSingleNode("SeasonNumber").InnerText);
                    capi = Convert.ToInt32(e.SelectSingleNode("EpisodeNumber").InnerText);
                    string fecha2 = e.SelectSingleNode("FirstAired").InnerText;
                    Fecha = new DateTime(Convert.ToInt32(fecha2.Substring(0, 4)), Convert.ToInt32(fecha2.Substring(5, 2)), Convert.ToInt32(fecha2.Substring(8, 2)));
                    Fecha = Fecha.AddDays(1.5);
                    estado = 3;

                    aux.Add(new Episodio(nombreEp, Nombre, tempo, capi, estado, Fecha, calidad));

                }


            }

            Episodios.Add(aux);

        }

        /// <summary>
        /// Metodo que sirve para agregar una serie una vez seleccionada
        /// </summary>
        /// <param name="t">Temporada del siguiente capitulo a descargar</param>
        /// <param name="c">Capitulo del siguiente capitulo a descargar</param>
        public void addSerie(int t, int c)
        {
            Temporada = t;
            Capitulo = c;

            //Se descarga la imagen de la serie
            WebClient wc = new WebClient();
            wc.DownloadFile("http://thetvdb.com/banners/" + doc.SelectSingleNode("/Data/Series/fanart").InnerText, @"C:\Users\Marcelo\Documents\Project Eye\Project-Eye\Interfaz\Fanart\" + Nombre + ".jpg");

            for (int i = Temporada - 1; i < Episodios.Count; i++)
            {
                int j = (i == Temporada - 1 ? capitulo - 1 : 0);
                while (j < Episodios[i].Count)
                {
                    
                    if (Episodios[i][j].Fecha < DateTime.Now)
                    {
                        Episodios[i][j].getMagnet();
                        Episodios[i][j].Estado = 1;
                        Descargando++;
                    }
                    else
                    {
                        Episodios[i][j].Estado = 0;
                    }
                    j++;

                }
            }
            CrearArchivo();


        }

        /// <summary>
        /// Metodo que sirve para crear la base de datos de cada serie
        /// </summary>

        private void CrearArchivo()
        {
            string se =  "";
            StreamWriter escribe = new StreamWriter(@"C:\Users\Marcelo\Documents\Project Eye\Project-Eye\Base de Datos\Series\"+ Nombre + ".txt");
            foreach (List<Episodio> lista in Episodios)
            {
                foreach (Episodio ep in lista)
                {
                    se += ep.NombreSerie + "*" + ep.Temporada + "*" + ep.Capitulo + "*" + ep.NombreEp + "*" + ep.Hash + "*" + ep.Fecha.ToShortDateString() + "*" + ep.Estado + "\r\n";
                }
                se += "-\r\n";
                escribe.Write(se);
                se = "";
            }

            escribe.Close();
            MessageBox.Show("LISTO");
        }


        /// <summary>
        /// Metodo que sirve para borrar un archivo de la clase
        /// </summary>
        public void BorrarArchivo()
        {
            File.Delete(@"C:\Users\Marcelo\Documents\Project Eye\Project-Eye\Base de Datos\Series\" + Nombre + ".txt");
        }


        /// <summary>
        /// Metodo que sirve para imprimir la informacion general de la serie
        /// </summary>
        /// <returns>El string con toda la informacion de la serie</returns>
        public string Imprimir()
        {
            return Nombre + "\r\n" + PorVer + "\r\n" + Descargando + "\r\n" + Temporada + " " + Capitulo + "\r\n-";

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
