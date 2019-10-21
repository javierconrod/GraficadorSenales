using System;
using System.Collections.Generic;
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

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mostrarSegundaSeñal(false);

        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            Señal señal;
            Señal segundaSeñal = null;
            Señal señalResultante;

            switch (cbTipoSeñal.SelectedIndex)
            {
                case 0: //parabólica
                    señal = new SeñalParabolica();
                    break;
                case 1: //senoidal
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFrecuencia.Text);
                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 2: //función signo
                    señal = new FuncionSigno();
                    break;
                case 3: //exponencial alfa
                    double alfa = double.Parse(((ConfiguracionSeñalExponencialAlfa)(panelConfiguracion.Children[0])).txtAlfa.Text);
                    señal = new Exponencial_Alfa(alfa);
                    
                    break;
                case 4: //audio
                    string rutaArchivo = ((ConfiguracionAudio)(panelConfiguracion.Children[0])).txtRutaArchivo.Text;
                    señal = new SeñalAudio(rutaArchivo);
                    txtTiempoInicial.Text = señal.TiempoInicial.ToString();
                    txtTiempoFinal.Text = señal.TiempoFinal.ToString();
                    txtFrecuenciaMuestreo.Text = señal.FrecuenciaMuestreo.ToString();

                    break;
                default:
                    señal = null;

                    break;
            }

            if(cbTipoSeñal.SelectedIndex != 4 && señal != null)
            {
                señal.TiempoFinal = tiempoFinal;
                señal.TiempoInicial = tiempoInicial;
                señal.FrecuenciaMuestreo = frecuenciaMuestreo;

                señal.construirSeñal();
            }

            //construir segunda señal
            if (cbOperacion.SelectedIndex == 2)
            {
                switch (cbTipoSeñal2.SelectedIndex)
                {
                    case 0: //Parabolica
                        segundaSeñal = new SeñalParabolica();

                        break;
                    case 1: //senoidal
                        double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion2.Children[0])).txtAmplitud.Text);
                        double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion2.Children[0])).txtFase.Text);
                        double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion2.Children[0])).txtFrecuencia.Text);

                        segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);

                        break;
                    case 2: //función signo
                        segundaSeñal = new FuncionSigno();

                        break;
                    case 3: //Exponencial
                        double alfa = double.Parse(((ConfiguracionSeñalExponencialAlfa)(panelConfiguracion2.Children[0])).txtAlfa.Text);
                        segundaSeñal = new Exponencial_Alfa(alfa);

                        break;
                    case 4: //Audio
                        string rutaArchivo = ((ConfiguracionAudio)(panelConfiguracion2.Children[0])).txtRutaArchivo.Text;
                        segundaSeñal = new SeñalAudio(rutaArchivo);
                        txtTiempoInicial.Text = segundaSeñal.TiempoInicial.ToString();
                        txtTiempoFinal.Text = segundaSeñal.TiempoFinal.ToString();
                        txtFrecuenciaMuestreo.Text = segundaSeñal.FrecuenciaMuestreo.ToString();

                        break;
                    default:
                        segundaSeñal = null;

                        break;
                }
                if (cbTipoSeñal2.SelectedIndex != 4 && segundaSeñal != null)
                {
                    segundaSeñal.TiempoInicial = tiempoInicial;
                    segundaSeñal.TiempoFinal = tiempoFinal;
                    segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;
                    segundaSeñal.construirSeñal();
                }
            }
            switch (cbOperacion.SelectedIndex)
            {
                case 0: //escala de amplitud
                    double factorEscala = double.Parse(((ConfiguracionOperacionEscalaAmplitud)(panelConfiguracionOperacion.Children[0])).txtFactorEscala.Text);
                    señalResultante = Señal.escalarAmplitud(señal, factorEscala);

                    break;
                case 1: //desplazamiento
                    double cantidadDesplazamiento = double.Parse(((ConfiguracionOperacionDesplazamiento)(panelConfiguracionOperacion.Children[0])).txtCantidadDesplazamiento.Text);
                    señalResultante = Señal.desplazamientoAmplitud(señal, cantidadDesplazamiento);

                    break;
                case 2: //multiplicacion
                    señalResultante = Señal.multiplicarSeñales(señal, segundaSeñal);

                    break;
                case 3: //escala exponencial
                    double exponente = double.Parse(((ConfiguracionOperacionEscalaExponencial)(panelConfiguracionOperacion).Children[0]).txtExponente.Text);
                    señalResultante = Señal.escalaExponenecial(señal, exponente);

                    break;
                case 4: //transformada de Fourier
                    señalResultante = Señal.transformadaFourier(señal);


                    break;
               default:
                    señalResultante = null;

                    break;
            }

            //Elige entre la primera y la resultante
            double amplitudMaxima = (señal.AmplitudMaxima >= señalResultante.AmplitudMaxima) ?
                señal.AmplitudMaxima : señalResultante.AmplitudMaxima;
            if (segundaSeñal != null)
            {
                //elige entre la mas grande de la 1ra y resultante y la segunda
                amplitudMaxima = (amplitudMaxima > segundaSeñal.AmplitudMaxima) ? amplitudMaxima : segundaSeñal.AmplitudMaxima;
            }

            plnGrafica.Points.Clear();
            plnGraficaResultante.Points.Clear();
            plnGrafica2.Points.Clear();

            if (segundaSeñal != null)
            {
                foreach (var muestra in segundaSeñal.Muestras)
                {
                    plnGrafica2.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
                }
            }
            foreach (Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }
            foreach (Muestra muestra in señalResultante.Muestras)
            {
                plnGraficaResultante.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

               lblAmplitudSuperior.Text = amplitudMaxima.ToString("F");
               lblAmplitudInferior.Text = "-" + amplitudMaxima.ToString("F");

               lblAmplitudResultanteSuperior.Text = amplitudMaxima.ToString("F");
               lblAmplitudResultanteInferior.Text = "-" + amplitudMaxima.ToString("F");

               plnEjeX.Points.Clear();
               plnEjeX.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
               plnEjeX.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

               plnEjeY.Points.Clear();
               plnEjeY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
               plnEjeY.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));

               plnEjeXResultante.Points.Clear();
               plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
               plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

               plnEjeYResultante.Points.Clear();
               plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
               plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));

        }

        public Point adaptarCoordenadas(double x, double y, double tiempoInicial, double amplitudMaxima)
        {
            return new Point((x- tiempoInicial) * scrGrafica.Width, (-1 * (y * ((scrGrafica.Height / 2.0 -20) / amplitudMaxima)) + scrGrafica.Height/2));
        }

        private void CbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch(cbTipoSeñal.SelectedIndex)
            {
                case 0: //parabolica
                    break;
                case 1: //senoidal
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2:
                    break;
                case 3:
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencialAlfa());
                    break;
                case 4:
                    panelConfiguracion.Children.Add(new ConfiguracionAudio());
                    break;
                default:
                    break;
            }
        }

        private void CbOperacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracionOperacion.Children.Clear();
            mostrarSegundaSeñal(false);
            switch(cbOperacion.SelectedIndex)
            {
                case 0: //escala de amplitud
                    panelConfiguracionOperacion.Children.Add(new ConfiguracionOperacionEscalaAmplitud());
                    break;
                case 1: //desplazamiento de amplitud
                    panelConfiguracionOperacion.Children.Add(new ConfiguracionOperacionDesplazamiento());
                    break;
                case 2: //multiplicacion de señales
                    mostrarSegundaSeñal(true);
                    break;
                case 3: //escala exponencial
                    panelConfiguracionOperacion.Children.Add(new ConfiguracionOperacionEscalaExponencial());
                    break;
                case 4: //transformada de Fourier
                    break;
                default:
                    break;
            }
        }

        private void CbTipoSeñal2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion2.Children.Clear();
            switch (cbTipoSeñal2.SelectedIndex)
            {
                case 0: //parabolica
                    break;
                case 1: //senoidal
                    panelConfiguracion2.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2:
                    break;
                case 3:
                    panelConfiguracion2.Children.Add(new ConfiguracionSeñalExponencialAlfa());
                    break;
                case 4:
                    panelConfiguracion2.Children.Add(new ConfiguracionAudio());
                    break;
                default:
                    break;
            }
        }
        void mostrarSegundaSeñal(bool mostrar)
        {
            if (mostrar)
            {
                lblTipoSeñal2.Visibility = Visibility.Visible;
                cbTipoSeñal2.Visibility = Visibility.Visible;
                panelConfiguracion2.Visibility = Visibility.Visible;
            }
            else
            {
                lblTipoSeñal2.Visibility = Visibility.Hidden;
                cbTipoSeñal2.Visibility = Visibility.Hidden;
                panelConfiguracion2.Visibility = Visibility.Hidden;
            }
        }
    }
}
