using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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

namespace Unity_Class_Tree
{
    public partial class MainWindow : Window
    {
        Grid TakeClassGrid = new Grid();
        Point mousePosCanvasOld = new Point(0, 0);
        public bool GetMouseElement = false;
        public MainWindow()
        {
            InitializeComponent();
            CreateNewClass();
        }

        public void CreateNewClass()
        {
            //___CREATE UIClass OBJECT___

            UIClass uIClass = new UIClass();

            // classGrid
            uIClass.classGrid = new Grid { Background = Brushes.Black, Width = 1000, Height = 600 };
            uIClass.classGrid.MouseLeftButtonDown += GridMouseLeftButtonDown;
            uIClass.classGrid.MouseLeftButtonUp += GridMouseLeftButtonUp;
            uIClass.classGrid.MouseMove += GridMouseMove;
            // classBorders
            uIClass.classBorderGray = new Border { Width = 400, Height = 109, Background = new SolidColorBrush(Color.FromRgb(108, 108, 109)), CornerRadius = new CornerRadius(6), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, BorderBrush = new SolidColorBrush(Color.FromRgb(89, 89, 89)), BorderThickness = new Thickness(2) };
            uIClass.classBorderWhite = new Border { Width = 300, Height = 50, Background = new SolidColorBrush(Colors.White), CornerRadius = new CornerRadius(6), VerticalAlignment = VerticalAlignment.Center, BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 22) };
            // classDescriptionButton
            uIClass.classDescriptionButton = new Button { Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), Foreground = new SolidColorBrush(Color.FromRgb(43, 175, 62)), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 88), Content = "???" };
            uIClass.classDescriptionButton.Click += OpenHideClassDescriptionTextBoxMethod;
            // 2 classTextboxes
            uIClass.classNameTextBox = new TextBox { Name = "classNameTextBox", Width = 200, FontSize = 14, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(43, 145, 175)), BorderThickness = new Thickness(0), Margin = new Thickness(0, 0, 0, 22) };
            uIClass.classDescriptionTextBox = new TextBox { Name = "classDescriptionTextBox", Width = 296, Height = 140, FontSize = 9, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(0, 0, 0, 252), Visibility = Visibility.Collapsed, Text = "123ABC" };

            // Right

            // Right A
            uIClass.rightACheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 88) };
            uIClass.rightAButton = new Button { Name = "rightA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(400, 0, 0, 88) };
            uIClass.rightAButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.rightARichTextBox = new RichTextBox { FontFamily = new FontFamily("Calibri"), Name = "rightARichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(730, 158, 0, 0), Visibility = Visibility.Collapsed, FontStyle = FontStyles.Normal };
            uIClass.rightARichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.rightARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            //FlowDocument flowDocument = new FlowDocument(); // FlowDocument, чтобы потом в него добавить Paragraph
            //Paragraph paragraph = new Paragraph(); // Paragraph, чтобы в него можно было добавить коллекцию Inline-ов
            //flowDocument.Blocks.Add(paragraph); // Добавление Paragraph-а в FlowDocument
            //uIClass.rightARichTextBox.Document = flowDocument; // Добавление FlowDocument-а в RichTextBox
            // right Static A
            uIClass.rightStaticACheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 56) };
            uIClass.rightStaticAButton = new Button { Name = "rightStaticA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(400, 0, 0, 56) };
            uIClass.rightStaticAButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.rightStaticARichTextBox = new RichTextBox { Name = "rightStaticARichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(730, 190, 0, 0), Visibility = Visibility.Collapsed };
            uIClass.rightStaticARichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.rightStaticARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            // right Constr
            uIClass.rightConstrCheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 18) };
            uIClass.rightConstrButton = new Button { Name = "rightConstr", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(400, 0, 0, 18) };
            uIClass.rightConstrButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.rightConstrRichTextBox = new RichTextBox { Name = "rightConstrRichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(730, 228, 0, 0), Visibility = Visibility.Collapsed };
            uIClass.rightConstrRichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.rightConstrRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            // right Meth
            uIClass.rightMethCheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(320, 20, 0, 0) };
            uIClass.rightMethButton = new Button { Name = "rightMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(400, 20, 0, 0) };
            uIClass.rightMethButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.rightMethRichTextBox = new RichTextBox { Name = "rightMethRichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(730, 266, 0, 0), Visibility = Visibility.Collapsed };
            uIClass.rightMethRichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.rightMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            // right StaticMeth
            uIClass.rightStaticMethCheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(320, 52, 0, 0) };
            uIClass.rightStaticMethButton = new Button { Name = "rightStaticMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(400, 52, 0, 0) };
            uIClass.rightStaticMethButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.rightStaticMethRichTextBox = new RichTextBox { Name = "rightStaticMethRichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(730, 298, 0, 0), Visibility = Visibility.Collapsed };
            uIClass.rightStaticMethRichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.rightStaticMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            // right Message
            uIClass.rightMessageCheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(320, 90, 0, 0) };
            uIClass.rightMessageButton = new Button { Name = "rightMessage", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(400, 90, 0, 0) };
            uIClass.rightMessageButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.rightMessageRichTextBox = new RichTextBox { Name = "rightMessageRichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(730, 336, 0, 0), Visibility = Visibility.Collapsed };
            uIClass.rightMessageRichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.rightMessageRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);

            // Left

            // Left A
            uIClass.leftACheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(0, 0, 320, 88) };
            uIClass.leftAButton = new Button { Name = "leftA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 400, 88) };
            uIClass.leftAButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.leftARichTextBox = new RichTextBox { Name = "leftARichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(0, 158, 730, 0), Visibility = Visibility.Collapsed };
            uIClass.leftARichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.leftARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            // RightACheckBox, RightAButton and RightARichTextBox
            uIClass.leftMethCheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(0, 20, 320, 0) };
            uIClass.leftMethButton = new Button { Name = "leftMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(0, 20, 400, 0) };
            uIClass.leftMethButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.leftMethRichTextBox = new RichTextBox { Name = "leftMethRichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(0, 266, 730, 0), Visibility = Visibility.Collapsed };
            uIClass.leftMethRichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.leftMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
            // RightACheckBox, RightAButton and RightARichTextBox
            uIClass.leftStaticMethCheckBox = new CheckBox { Width = 16, Height = 16, Margin = new Thickness(0, 52, 320, 0) };
            uIClass.leftStaticMethButton = new Button { Name = "leftStaticMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(0, 52, 400, 0) };
            uIClass.leftStaticMethButton.Click += OpenHideRichTextBoxesMethod;
            uIClass.leftStaticMethRichTextBox = new RichTextBox { Name = "leftStaticMethRichTextBox", Width = 270, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(0, 298, 730, 0), Visibility = Visibility.Collapsed };
            uIClass.leftStaticMethRichTextBox.LostFocus += RichTextBox_LostFocus;
            uIClass.leftStaticMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);

            // Adding elements to classGrid
            uIClass.classGrid.Children.Add(uIClass.classBorderGray);
            uIClass.classGrid.Children.Add(uIClass.classBorderWhite);
            uIClass.classGrid.Children.Add(uIClass.classDescriptionButton);
            uIClass.classGrid.Children.Add(uIClass.classNameTextBox);
            uIClass.classGrid.Children.Add(uIClass.classDescriptionTextBox);
            uIClass.classGrid.Children.Add(uIClass.rightACheckBox);
            uIClass.classGrid.Children.Add(uIClass.rightAButton);
            uIClass.classGrid.Children.Add(uIClass.rightARichTextBox);
            uIClass.classGrid.Children.Add(uIClass.rightStaticACheckBox);
            uIClass.classGrid.Children.Add(uIClass.rightStaticAButton);
            uIClass.classGrid.Children.Add(uIClass.rightStaticARichTextBox);
            uIClass.classGrid.Children.Add(uIClass.rightConstrCheckBox);
            uIClass.classGrid.Children.Add(uIClass.rightConstrButton);
            uIClass.classGrid.Children.Add(uIClass.rightConstrRichTextBox);
            uIClass.classGrid.Children.Add(uIClass.rightMethCheckBox);
            uIClass.classGrid.Children.Add(uIClass.rightMethButton);
            uIClass.classGrid.Children.Add(uIClass.rightMethRichTextBox);
            uIClass.classGrid.Children.Add(uIClass.rightStaticMethCheckBox);
            uIClass.classGrid.Children.Add(uIClass.rightStaticMethButton);
            uIClass.classGrid.Children.Add(uIClass.rightStaticMethRichTextBox);
            uIClass.classGrid.Children.Add(uIClass.rightMessageCheckBox);
            uIClass.classGrid.Children.Add(uIClass.rightMessageButton);
            uIClass.classGrid.Children.Add(uIClass.rightMessageRichTextBox);
            uIClass.classGrid.Children.Add(uIClass.leftACheckBox);
            uIClass.classGrid.Children.Add(uIClass.leftAButton);
            uIClass.classGrid.Children.Add(uIClass.leftARichTextBox);
            uIClass.classGrid.Children.Add(uIClass.leftMethCheckBox);
            uIClass.classGrid.Children.Add(uIClass.leftMethButton);
            uIClass.classGrid.Children.Add(uIClass.leftMethRichTextBox);
            uIClass.classGrid.Children.Add(uIClass.leftStaticMethCheckBox);
            uIClass.classGrid.Children.Add(uIClass.leftStaticMethButton);
            uIClass.classGrid.Children.Add(uIClass.leftStaticMethRichTextBox);
            canvas.Children.Add(uIClass.classGrid);

            //
            //
            //

            //___CREATE DataClass OBJECT___



        }

        private void OpenHideClassDescriptionTextBoxMethod(object sender, RoutedEventArgs e) // Показывает (делает видимым) или скрывает ClassDescriptionTextBox при нажатии на classDescriptionButton
        {
            TextBox[] textBoxes = ((sender as Button).Parent as Grid).Children.OfType<TextBox>().ToArray();
            foreach (TextBox t in textBoxes)
            {
                if (t.Name == "classDescriptionTextBox" && t.Visibility == Visibility.Collapsed)
                { t.Visibility = Visibility.Visible; break; }
                else if (t.Name == "classDescriptionTextBox" && t.Visibility == Visibility.Visible)
                { t.Visibility = Visibility.Collapsed; break; }
            }
        }

        private void OpenHideRichTextBoxesMethod(object sender, RoutedEventArgs e) // Показывает (делает видимым) или скрывает RichTextBox-ы при нажатии на соответствующие кнопки
        {
            int a = 0;
            string ButtonName = (sender as Button).Name + "RichTextBox";
            RichTextBox[] richTextBoxes = ((sender as Button).Parent as Grid).Children.OfType<RichTextBox>().ToArray();
            //richTextBoxes[0].Visibility = Visibility.Visible;
            foreach (RichTextBox r in richTextBoxes)
            {
                if (r.Name == ButtonName && richTextBoxes[a].Visibility == Visibility.Collapsed) { richTextBoxes[a].Visibility = Visibility.Visible; }
                else if (r.Name == ButtonName && richTextBoxes[a].Visibility == Visibility.Visible) { richTextBoxes[a].Visibility = Visibility.Collapsed; }
                a++;
            }
        }


        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e) //
        {
            //rtb = richTextBox;
            Paragraph paragraph = (sender as RichTextBox).Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов выделенного RichTextBox-а
            foreach (Inline i in paragraph.Inlines)
            {
                MessageBox.Show(i.ToString() + "___" + new TextRange(i.ContentStart, i.ContentEnd).Text + " " + i.FontWeight + " " + i.Foreground);
            }
        }


        // Реализация перемещения мышкой UIClass-а
        private void GridMouseLeftButtonDown(object sender, MouseButtonEventArgs e) // Когда кнопка мыши нажата вниз, может работать событие GridMouseMove (перемещение классов по холсту)
        {
            //TakeClassGrid = sender as Grid;
            //MessageBox.Show(TakeClassGrid.Name);
            //GetMouseElement = true;
            //TakeClassGrid.CaptureMouse();
            //mousePosCanvasOld = e.GetPosition(canvas);
        }
        private void GridMouseLeftButtonUp(object sender, MouseButtonEventArgs e) // Когда кнопка мыши отпущена, перемещение прекращается
        {
            //GetMouseElement = false;
            //TakeClassGrid.ReleaseMouseCapture();
            //mousePosCanvasOld = new Point(0, 0);
            //TakeClassGrid = null;
        }
        private void GridMouseMove(object sender, MouseEventArgs e) // Само перемещение классов по холсту
        {
            //Point mousePosCanvasNew = e.GetPosition(canvas);
            //Point CanvasElement = new Point((double)TakeClassGrid.GetValue(Canvas.LeftProperty), (double)TakeClassGrid.GetValue(Canvas.TopProperty));

            //if (GetMouseElement)
            //{
            //    Canvas.SetLeft(TakeClassGrid, CanvasElement.X + (mousePosCanvasNew.X - mousePosCanvasOld.X));
            //    Canvas.SetTop(TakeClassGrid, CanvasElement.Y + (mousePosCanvasNew.Y - mousePosCanvasOld.Y));
            //    mousePosCanvasOld.X = mousePosCanvasNew.X;
            //    mousePosCanvasOld.Y = mousePosCanvasNew.Y;
            //    //MessageBox.Show(CanvasElement.X.ToString() + " " + mousePosCanvasNew.X.ToString());
            //}
        }






        private void ChangeTextColorButton(object sender, RoutedEventArgs e)
        {
            //if (rtb != null && rtb.Selection.Text != " " && rtb.Selection.Text.Length > 0) // Если выделена хоть одна буква - то открывает ColorDialog для выбора цвета выделенного текста
            //{
            //    System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            //    colorDialog.ShowDialog();
            //    rtb.Selection.ApplyPropertyValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
            //    changeColor = false;
            //}
            //Paragraph paragraph = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов выделенного RichTextBox-а
            //foreach (Inline i in paragraph.Inlines)
            //{
            //    MessageBox.Show(i.ToString() + "___" + new TextRange(i.ContentStart, i.ContentEnd).Text + " " + i.FontWeight + " " + i.Foreground);
            //}

            //using (FileStream fs = new FileStream("D:\\Test.json", FileMode.OpenOrCreate))
            //{
            //    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(string));
            //    jsonSerializer.WriteObject(fs, inlines);
            //}
        }

        private void ChangeTextSizeButton(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (rtb != null && rtb.Selection.Text != " " && rtb.Selection.Text.Length > 0) // Происходит увеличение шрифта выделенного фрагмента текста
            //    {
            //        int b = Convert.ToInt32(rtb.Selection.GetPropertyValue(TextElement.FontSizeProperty));
            //        rtb.Selection.ApplyPropertyValue(FontSizeProperty, (b + 5).ToString());
            //    }
            //}
            //catch (Exception) { }
        }










        private void Button_Click(object sender, RoutedEventArgs e)  //Добавление нового класса и запись в файл
        {
            //PropertyClass propertyClass = new PropertyClass();
            //propertyClass.textBoxSave = "Hello World!";
            //propertyClass.ClassCount++;
            //PropertyClassList.Add(propertyClass);

            //UIClass uiClass = new UIClass();
            //uiClass.grid = new Grid {Margin = new Thickness(100), Background = Brushes.Black, Name = "Grid1"};
            //uiClass.grid.MouseLeftButtonDown += GridMouseLeftButtonDown;
            //uiClass.grid.MouseLeftButtonUp += GridMouseLeftButtonUp;
            //uiClass.grid.MouseMove += GridMouseMove;
            //uiClass.textBox = new TextBox { Background = Brushes.Green, Foreground = Brushes.Red, Text = propertyClass.textBoxSave, Width = 100, Height = 50, Margin = new Thickness(20, 10, 10, 10) };
            //uiClass.grid.Children.Add(uiClass.textBox);

            //canvas.Children.Add(uiClass.grid);

            //using (FileStream fs = new FileStream("D:\\Test.json", FileMode.OpenOrCreate))
            //{
            //    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<PropertyClass>));
            //    jsonSerializer.WriteObject(fs, PropertyClassList);
            //}
            //MessageBox.Show(PropertyClassList.Count.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)  //Загрузка из файла всех классов
        {
            //PropertyClassList = new List<PropertyClass>();
            //using (FileStream fs = new FileStream("D:\\Test.json", FileMode.Open))
            //{
            //    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<PropertyClass>));
            //    PropertyClassList = (List<PropertyClass>)jsonSerializer.ReadObject(fs);
            //}
            //foreach (PropertyClass p in PropertyClassList)
            //{
            //    UIClass uiClass = new UIClass();
            //    uiClass.textBox = new TextBox { Background = Brushes.Green, Foreground = Brushes.Red, Text = PropertyClassList[0].textBoxSave, Width = 100, Height = 50, Margin = new Thickness(5, 5, 50, 10) };
            //    canvas.Children.Add(uiClass.textBox);
            //}
            //MessageBox.Show(PropertyClassList.Count.ToString());
        }




    }
}
