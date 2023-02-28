namespace Prueba.Models
{
    public class Domino
    {
        public string[] Ficha { get; set; } //resive las fichas por el json

        public string Mensaje = "";

        public bool aceptado = true;

        public int Fichas_Ingresadas = 0;

        public string Fichas_organizadas = "";

        public List<int> Lista = new List<int>();

        public List<int> Lista2 = new List<int>();

        public List<int> Lista3 = new List<int>();

        //Comparar posiciones de matriz - diagonal
        public int b = 0;

        public int w = 1; //aumentador para desplazarse por las filas

        public int v = 1; //posicion 1 del pivote

        public int c = 1; //pivote auxiliar

        public int guardart = 0; //guarda la fila 1, la t viene siendo i como en una matriz

        public int guardarr = 0; //guarda la fila 2, la r viene siendo j como una matriz

        public int h = 1; // Desplazador de filas, sirve para desplazarme por las filas de la matriz

        //Colocarlo en matriz con posiciones k y m
        public int k = 0;

        public int m = 0;

        // Variables para limpiar las fichas y poder trabajar con ellas, osea quitarle los corchetes y medio [2|3]
        public string data1;

        public string data2;

        public string data3;

        //Variables de interruptores para que ocurra solo una vez en la dimencion 2x4
        public bool interruptor = true;

        public bool interruptor2 = true;

    }

    
}
