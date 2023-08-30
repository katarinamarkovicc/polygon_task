using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Zadatak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        PointCollection polygonPoints = new PointCollection();  
    
        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox_previewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(textBox.Text == "")
            {
                MessageBox.Show("Niste uneli broj!");
                return;
            }

            int n = Convert.ToInt32(textBox.Text);
            Point p = new Point();
            bool unutar;

            if(polygonPoints.Count < n)
            {
                polygonPoints.Add(e.GetPosition(GridCanvas));

            } else {
                p = Mouse.GetPosition(GridCanvas);

                unutar = IsPointInPolygon4(polygonPoints, p);

                if(unutar == false)
                {
                    label.Content = "Tačka se nalazi van mnogougla!";
                } else
                {
                    label.Content = "Tačka se nalazi unutar mnogougla!";
                }
            }

            Polygon pl = new Polygon();
            pl.Stroke = Brushes.Black;
            pl.Fill = Brushes.MistyRose;
            pl.HorizontalAlignment = HorizontalAlignment.Left;
            pl.VerticalAlignment = VerticalAlignment.Center;
            pl.StrokeThickness = 2;
            pl.Points = polygonPoints;
            

            GridCanvas.Children.Add(pl);

            if(p.X != 0 && p.Y != 0)
            {
                Ellipse myEllipse = new Ellipse();

                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                myEllipse.Fill = mySolidColorBrush;
                myEllipse.StrokeThickness = 2;
                myEllipse.Stroke = Brushes.Black;
                myEllipse.Width = 10;
                myEllipse.Height = 10;
                myEllipse.SetValue(Canvas.LeftProperty, p.X);
                myEllipse.SetValue(Canvas.TopProperty, p.Y);
    
                GridCanvas.Children.RemoveRange(4, GridCanvas.Children.Count);
                GridCanvas.Children.Add(myEllipse);
            }

        }

        public static bool IsPointInPolygon4(PointCollection polygon, Point testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

    }
}
