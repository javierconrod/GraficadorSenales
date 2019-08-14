﻿using System;
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

            plnGrafica.Points.Add(new Point(0,10));
            plnGrafica.Points.Add(new Point(20, 15));
            plnGrafica.Points.Add(new Point(30, 5));
            plnGrafica.Points.Add(new Point(40, 0));
            plnGrafica.Points.Add(new Point(60, 20));
            plnGrafica.Points.Add(new Point(100, 45));
            plnGrafica.Points.Add(new Point(750, 0));

        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e)
        {
            double amplitud = double.Parse(txtAmplitud.Text);
            double fase = double.Parse(txtFase.Text);
            double frecuencia = double.Parse(txtFrecuencia.Text);
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);

            SeñalSenoidal señal = new SeñalSenoidal(amplitud, fase, frecuencia);

            double periodoMuestreo = 1.0 / frecuenciaMuestreo;

            plnGrafica.Points.Clear();

            for(double i = tiempoInicial; i <= tiempoFinal; i+= periodoMuestreo)
            {

            }


        }
    }
}
