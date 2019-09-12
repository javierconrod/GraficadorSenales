using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class Exponencial_Alfa : Señal
    {
        public double Alfa { get; set; }
        public Exponencial_Alfa()
        {
            Alfa = 0.0;
            Muestras = new List<Muestra>();
        }
        public Exponencial_Alfa(double alfa)
        {
            AmplitudMaxima = 0.0;
            Alfa = alfa;
            Muestras = new List<Muestra>();
        }
        override public double evaluar(double tiempo)
        {
            double resultado = Math.Exp(Alfa * tiempo);
            return resultado;
        }

    }
}
