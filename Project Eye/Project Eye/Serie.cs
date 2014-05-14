using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int Descargando { get; set; } //Cantidad de episodios por descargar

        //Constructor
        public Serie(int id, int temporada, int capitulo)
        {
            Id = id;
            SigEp = new int[2];
            SigEp[0] = temporada;
            SigEp[1] = capitulo;


        }
    }
}
