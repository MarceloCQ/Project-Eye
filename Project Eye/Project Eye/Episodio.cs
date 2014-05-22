using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project_Eye
{
    /*Clase episodio que representa un episodio de la serie
     */
    class Episodio
    {
        //Atributos
        public string NombreSerie { get; set; } //Nombre de la serie a la que pertenece
        public string NombreEp { get; set; } //Nombre del episodio
        public int Temporada { get; set; } //Numero de Temporada
        public int Capitulo { get; set; } //Numero de Capitulo
        public int Estado { get; set; } //0-> No ha salido, 1-> Descargando, 2-> Descargado, 3-> Visto
        public DateTime Fecha { get; set; } //Fecha de cuando salio el episodio
        public string Hash { get; set; } //Hash del torrent
        public int Porcentaje { get; set; } // Porcentaje de descarga
        public string Archivo { get; set; } //Archivo del episodio
        public string Calidad { get; set; } //Calidad 720p/1080p

        //Constructor
        public Episodio(string nombrep, string nombrese, int tempo, int capi, int estado, DateTime fecha, string calidad)
        {
            NombreEp = nombrep;
            NombreSerie = nombrese;
            Temporada = tempo;
            Capitulo = capi;
            Estado = estado;
            Fecha = fecha;
            Calidad = calidad;
            Hash = "-1";
            Porcentaje = 0;
            Archivo = "";
            
            
        }

        public void getMagnet()
        {
            string link;
            string temp = (Temporada < 10? "0" : "") + Temporada;
            string cap = (Capitulo < 10 ? "0" : "") + Capitulo;
            string codigo;
            int primero, segundo;

            do
            {
                string pagina = "http://thepiratebay.se/search/" + NombreSerie.Replace(' ', '+') + "+S" + temp +  "E" + cap + "+" + Calidad + "/0/7/0";
                HttpDownloader fuente = new HttpDownloader(pagina, "thepiratebay.se", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:29.0) Gecko/20100101 Firefox/29.0");
                codigo = fuente.GetPage();
                primero = codigo.IndexOf("href=\"magnet");
                if (primero == -1)
                {
                    if (Calidad == "")
                    {
                        link = "-1";
                        break;
                    }
                    Calidad = (Calidad == "1080p" ? "720p" : "");
                }

            }
            while(primero == -1);

            if (primero != -1)
            {
                //Saca el link de la pagina
                segundo = codigo.IndexOf("\" title", primero);
                link = codigo.Substring(primero + 6, segundo - primero - 6);
                /* AGREGAR MAGNET */
                //Saca el HASH del link
                Regex r2 = new Regex(".+xt=urn:btih:(.+?)&dn=.+");
                Hash = r2.Match(link).Groups[1].Value.ToUpper();

            }
            else
            {
                throw new Exception("No se encontro el torrent");
            }
           
        }



    }
}
