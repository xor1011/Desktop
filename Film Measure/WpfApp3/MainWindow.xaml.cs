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
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.IO;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point start, end, StartT, EndT;
        double dx, dy, length, conversionFactor = 1;
        string path;
        bool c = false, l = false, draw = true;
        double calLength = 1;
        string annotation;
        

        public MainWindow()
        {
            InitializeComponent();
            myCanvas.Cursor = Cursors.Cross;
            // pic.Height = 100;
            // pic.Width = 100;



        }
        private void Create_Label(Point lxy, string label)
        {
            Label a = new Label()
            {
                Foreground = Brushes.Turquoise,
                Content = label,
                FontSize = 11,

            };
            Canvas.SetLeft(a, lxy.X);
            Canvas.SetTop(a, lxy.Y);
            myCanvas.Children.Add(a);
        }
        private void Cal_Stuff()
        {
            calLength = length;
            if (c == true)
            {
                try
                {
                    string val = Microsoft.VisualBasic.Interaction.InputBox("Enter Calibration length", "Calibrate");
                    
                    conversionFactor = double.Parse(val) / calLength;
                    //MessageBox.Show(conversionFactor.ToString());

                }
                catch
                {
                    MessageBox.Show("Enter a valid number");
                }
                
                c = false;

            }
        }

        private void Draw_Line(int color, double px, double py, double pxx, double pyy)
        {
            // Point p, p2;
         
            Line myLine = new Line()
            {

                // Stroke = Brushes.Red,
                StrokeThickness = 1,
                X1 = px,
                Y1 = py,
                X2 = pxx,
                Y2 = pyy,

            };
            
            switch (color)
            {
                case 0:
                    myLine.Stroke = Brushes.Red;
                    break;
                case 1:
                    myLine.Stroke = Brushes.AliceBlue;
                    break;
                default:
                    myLine.Stroke = Brushes.Red;
                    break;
            }
            myCanvas.Children.Add(myLine);

        }



        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var st = (ScaleTransform)myCanvas.RenderTransform;
            double zoom = e.Delta > 0 ? .1 : -.1;
            st.ScaleX += zoom;
            st.ScaleY += zoom;
        }


        private void Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(myCanvas);
          
                
                draw = true;
                if (l == true)
                {
                    Create_Label(start, annotation);
                    l = false;
                    myCanvas.Cursor = Cursors.Cross;
                    draw = false;

                }
               
            
          
           


        }

        private void Mouse_Move(object sender, MouseEventArgs e)
        {
            end = e.GetPosition(myCanvas);
       
       

        }
    

      

        private void Mouse_Up(object sender, MouseButtonEventArgs e)
        {


            end.Y = end.Y ;
            start.Y = start.Y ;
            if (draw == true)
            {
                Draw_Line(0, start.X, start.Y, end.X, end.Y);
            }

            dx = start.X - end.X;
            dy = start.Y - end.Y;
            length = Math.Sqrt((Math.Pow(dx, 2) + Math.Pow(dy, 2)))*conversionFactor;
           
             
                length = Math.Round(length, 3);
         
            Cal_Stuff();

            Label m = new Label()
                {
                    Foreground = Brushes.White,

                    Content = length.ToString(),
                    FontSize = 11,

                };
                if (length == 0)
            {
                m.Content = "";
            }
                Canvas.SetLeft(m, end.X - 4);
                Canvas.SetTop(m, end.Y - 8);
            if (conversionFactor<=1 && l==false)
            {
                myCanvas.Children.Add(m);
            }


        }
       
        

        private void open_Click(object sender, RoutedEventArgs e)
        {
           
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (file.ShowDialog() == true)
                {
                    path = file.FileName;
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(path);
                    image.EndInit();
                    pic.Source = image;



                }
           
             


        }

        private void cal_Click(object sender, RoutedEventArgs e)
        {
            c = true;
            
            if (calLength != 1)
            {
                
            }
            else
            {
                MessageBox.Show("Measure your standard");
            }
            
            
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Show();


        }


        private void Save_Canvas(Canvas canvas, string filename)
        {
            
           
            tools.Visibility = Visibility.Hidden;
            pic.HorizontalAlignment = HorizontalAlignment.Center;
            
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int)mybox.Width+300,
                (int)mybox.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
          
          //  mybox.Measure(new Size((int)myCanvas.Width, (int)myCanvas.Height));
           // mybox.Arrange(new Rect(new Size((int)myCanvas.Width, (int)myCanvas.Height)));
            renderBitmap.Render(myCanvas);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            using (FileStream fs = File.Create(filename))
            {
                encoder.Save(fs);
            }
            
            tools.Visibility = Visibility.Visible;
        }

        /*private static void Save_Bitmap(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }*/
            private void Save_Click(object sender, RoutedEventArgs e)
        {
           // try
            //{
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
             {
                path = saveFile.FileName;

               // Bitmap bmp = (Bitmap)pic;

                //var file = new Uri(path);
                //string file = new Uri(path)


                Save_Canvas(myCanvas, path);
             }
           }
       
        private void Annotate_Click(object sender, RoutedEventArgs e)
        {
            l = true;
            annotation = Microsoft.VisualBasic.Interaction.InputBox("Enter your annotation", "Annotate");
            //MessageBox.Show("Click where you would like to place your label");
            myCanvas.Cursor = Cursors.Pen;




            
        }

    }







}

   

