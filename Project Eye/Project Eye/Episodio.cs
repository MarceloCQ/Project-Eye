using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public int Estado { get; set; } //0-> No ha salido -> 1 -> No descargado, 2-> Descargado, 3-> Visto
        public string Link { get; set; } //Link de donde se va a descargar
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
            Link = getMagnet();
            Porcentaje = 0;
            Archivo = "";
            Hash = "";
            
            
        }

        //Metodos locales
        private string getMagnet()
        {
            string temp = (Temporada < 10? "0" : "") + Temporada;
            string cap = (Capitulo < 10 ? "0" : "") + Capitulo;
            string pagina = "http://thepiratebay.se/search/" + NombreSerie.Replace(' ', '+') + "+S" + temp +  "E" + cap + "+" + Calidad + "/0/7/0";
            HttpDownloader fuente = new HttpDownloader(pagina, "thepiratebay.se", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:29.0) Gecko/20100101 Firefox/29.0");
            string codigo = fuente.GetPage();
            int primero = codigo.IndexOf("href=\"magnet");
            if (primero != -1)
            {
                int segundo = codigo.IndexOf("\" title", primero);
                string magnet = codigo.Substring(primero + 6, segundo - primero - 6);
                return magnet;
            }
            else
            {
                return "-1";
            }
        }



    }
}
