using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using testWorkForSirius.Methods;
using WpfApp1.Models;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CanvasStruct> _canvases;
        Stack<Shape> _shapes;
        public MainWindow()
        {
            InitializeComponent();
            butDelete.IsEnabled = false;
            _canvases = new List<CanvasStruct>();
            _shapes = new Stack<Shape>();
        }

        private void butAddShape_Click(object sender, RoutedEventArgs e)
        {
            addCanvas();
        }

        private void butDel_Click(object sender, RoutedEventArgs e)
        {
            delCanavs();

            butDelete.IsEnabled = false;
        }

        private void butDelete_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var but = sender as Button;
            if (but.IsEnabled)
            {
                but.Foreground = Brushes.Red;
                but.BorderBrush = Brushes.Red;
            }
            else
            {
                but.Foreground = Brushes.Gray;
                but.BorderBrush = Brushes.Gray;
            }
        }
        private void addCanvas()
        {
            int id = 1;
            if (_canvases.Count > 0)
                id = _canvases.Max(a => a.ID) + 1;
            var text = GetUI.GetText(true, id);
            var canvas = new Canvas()
            {
                Width = 130,
                Height = 30,
                Uid = "0" + id,
                Children =
                {
                    GetUI.GetBorder(true),
                    text
                }
            };
            var rnd = new Random();
            var height = rnd.Next(5, (int)mainWindow.Width - 150);
            var width = rnd.Next(40, (int)mainWindow.Height - 100);
            Canvas.SetLeft(canvas, Convert.ToDouble(height));
            Canvas.SetTop(canvas, Convert.ToDouble(width));
            canvas.MouseUp += Canvas_MouseUp;
            mainCanvas.Children.Add(canvas);
            _canvases.Add(new CanvasStruct()
            {
                ID = id,
                _canvas = canvas
            });
            addPen(new Point(height + 65, width));
        }
        private void delCanavs()
        {
            var timeList = new List<CanvasStruct>();
            foreach (var item in  _canvases)
            {
                if (item._canvas.Uid.StartsWith('1'))
                    mainCanvas.Children.Remove(item._canvas);
                else
                    timeList.Add(item);
            }

            _canvases = timeList;
            reprintPen();
        }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var canvas = (Canvas)sender;
            var del = canvas.Uid.StartsWith("0");
            canvas.Children.Clear();
            canvas.Children.Add(GetUI.GetBorder(!del));
            canvas.Children.Add(GetUI.GetText(!del, int.Parse(canvas.Uid.Remove(0, 1))));
            canvas.Uid = canvas.Uid.Remove(0, 1).Insert(0, del ? "1": "0");

            checkDel();
        }
        private void checkDel()
        {
            butDelete.IsEnabled = false;
            foreach (var item in _canvases)
            {
                if (item._canvas.Uid.StartsWith("1"))
                {
                    butDelete.IsEnabled = true;
                    break;
                }
            }
        }
        private void addPen(Point p2)
        {
            if (_canvases.Count <= 1)
                return;

            var start = _canvases.SkipLast(1).Last()._canvas;
            var p1 = start.TranslatePoint(new Point(65, 0), mainCanvas);
            printPen(p1, p2);
        }
        private void reprintPen()
        {
            while (_shapes.Count > 0) 
            { 
                mainCanvas.Children.Remove(_shapes.Peek());
                _shapes.Pop();
            }
            if (_canvases.Count <= 1)
                return;

            for (int i = 0; i < _canvases.Count - 1; i++)
            {
                var start = _canvases[i]._canvas;
                var end = _canvases[i + 1]._canvas;
                var p1 = start.TranslatePoint(new Point(65, 0), mainCanvas);
                var p2 = end.TranslatePoint(new Point(65, 0), mainCanvas);
                printPen(p1, p2);
            }
        }
        private void printPen (Point p1, Point p2)
        {
            if (p1.Y < p2.Y)
            {
                p1.Y += 30;
            }
            if (p1.Y > p2.Y)
            {
                p2.Y += 30;
            }

            var shape = GetUI.DrawLinkArrow(p1, p2);
            mainCanvas.Children.Add(shape);
            _shapes.Push(shape);
        }
    }
}
