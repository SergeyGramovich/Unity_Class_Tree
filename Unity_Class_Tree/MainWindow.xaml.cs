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
        RichTextBox rtb;
        Grid TakeClassGrid;
        Point mousePosClassCanvasOld = new Point(0, 0);
        Point mousePosCanvasOld = new Point(0, 0);
        Point currentCanvasPointClass = new Point(0, 0);
        public string currentClassName = "";
        bool GetMouseClass = false;
        bool GetMouseCanvas = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateNewClassButtonClick(object sender, RoutedEventArgs e) // Создание нового класса
        {
            // НУЖНО ДОПИСАТЬ СЮДА КОД ПРОВЕРКИ, КОТОРЫЙ БУДЕТ ПРОВЕРЯТЬ ИМЯ НОВОГО КЛАССА, Т,Е, СУЩЕСТВУЕТ УЖЕ ТАКОЙ КЛАСС ИЛИ НЕТ. ЕСЛИ СУЩЕСТВУЕТ, ЧТОБЫ ВЫВОДИЛОСЬ ПРЕДУПРЕЖДЕНИЕ ОБ ЭТОМ

            if (File.Exists("D:\\" + NewClassNameTextBox.Text + ".json")) { MessageBox.Show("Класс с таким именем уже создан."); }
            else
            {
                //___CREATE DataClass OBJECT___
                DataClass dataClass = new DataClass();
                dataClass.className = NewClassNameTextBox.Text;

                // Создание файла (в папке) для этого класса
                using (FileStream fs = new FileStream("D:\\" + dataClass.className + ".json", FileMode.Create))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                    jsonSerializer.WriteObject(fs, dataClass);
                }

                //
                //
                //

                //___CREATE UIClass OBJECT___

                UIClass uIClass = new UIClass();

                // classGrid
                uIClass.classGrid = new Grid { Name = NewClassNameTextBox.Text, Width = 400, Height = 109, Background = new SolidColorBrush(Color.FromRgb(108, 108, 109)), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
                uIClass.classGrid.MouseLeftButtonDown += GridMouseLeftButtonDown;
                uIClass.classGrid.MouseLeftButtonUp += GridMouseLeftButtonUp;
                uIClass.classGrid.MouseMove += GridMouseMove;
                // classBorders
                uIClass.classBorderWhite = new Border { Name = "classBorderWhite", Width = 300, Height = 50, Background = new SolidColorBrush(Colors.White), CornerRadius = new CornerRadius(6), VerticalAlignment = VerticalAlignment.Center, BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 22) };
                // classDescriptionButton
                uIClass.classDescriptionButton = new Button { Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), Foreground = new SolidColorBrush(Color.FromRgb(43, 175, 62)), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 88), Content = "???" };
                uIClass.classDescriptionButton.Click += OpenHideClassDescriptionTextBoxMethod;
                // 2 classTextboxes
                uIClass.classNameTextBox = new TextBox { Name = "classNameTextBox", Width = 296, FontSize = 19, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(43, 145, 175)), BorderThickness = new Thickness(0), Margin = new Thickness(0, 0, 0, 22), Text = dataClass.className };
                uIClass.classNameTextBox.GotFocus += ChildrenGotFocus;
                uIClass.classNameTextBox.LostFocus += ChangeClassNameLostFocus;
                uIClass.classNameTextBox.LostFocus += LostFocusClassSaving;
                uIClass.classDescriptionTextBox = new TextBox { Name = "classDescriptionTextBox", FontFamily = new FontFamily("Calibri"), Width = 296, Height = 140, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(52, -140, 52, 110), Visibility = Visibility.Collapsed, Text = dataClass.classDescription };
                uIClass.classDescriptionTextBox.GotFocus += ChildrenGotFocus;
                uIClass.classDescriptionTextBox.LostFocus += LostFocusClassSaving;

                // Right

                // right A
                uIClass.rightACheckBox = new CheckBox { Name = "rightA", Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 88), IsChecked = true, Opacity = 0.4 };
                uIClass.rightACheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightACheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightACheckBox.Click += LostFocusClassSaving;
                Grid rightAButtonContentGrid = new Grid();
                rightAButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 10, 0), Text = "int" });
                rightAButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "a" });
                uIClass.rightAButton = new Button { Name = "rightA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 3, -30, 91), Content = rightAButtonContentGrid };
                uIClass.rightAButton.GotFocus += ChildrenGotFocus;
                uIClass.rightAButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightARichTextBox = new RichTextBox { FontFamily = new FontFamily("Calibri"), Name = "rightARichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -120, -430, -278), Visibility = Visibility.Collapsed, FontStyle = FontStyles.Normal };
                uIClass.rightARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightARichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightARichTextBox.LostFocus += LostFocusClassSaving;
                // right Static A
                uIClass.rightStaticACheckBox = new CheckBox { Name = "rightStaticA", Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 56), IsChecked = true, Opacity = 0.4 };
                uIClass.rightStaticACheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticACheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightStaticACheckBox.Click += LostFocusClassSaving;
                Grid rightStaticAButtonContentGrid = new Grid();
                rightStaticAButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 10, 0), Text = "static int" });
                rightStaticAButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "a" });
                uIClass.rightStaticAButton = new Button { Name = "rightStaticA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 19, -30, 75), Content = rightStaticAButtonContentGrid };
                uIClass.rightStaticAButton.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticAButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightStaticARichTextBox = new RichTextBox { Name = "rightStaticARichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -110, -430, -300), Visibility = Visibility.Collapsed };
                uIClass.rightStaticARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightStaticARichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticARichTextBox.LostFocus += LostFocusClassSaving;
                // right Constr
                uIClass.rightConstrCheckBox = new CheckBox { Name = "rightConstr", Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 18), IsChecked = true, Opacity = 0.4 };
                uIClass.rightConstrCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightConstrCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightConstrCheckBox.Click += LostFocusClassSaving;
                Grid rightConstrButtonContentGrid = new Grid();
                rightConstrButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Constr()" });
                uIClass.rightConstrButton = new Button { Name = "rightConstr", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 38, -30, 56), Content = rightConstrButtonContentGrid };
                uIClass.rightConstrButton.GotFocus += ChildrenGotFocus;
                uIClass.rightConstrButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightConstrRichTextBox = new RichTextBox { Name = "rightConstrRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -90, -430, -318), Visibility = Visibility.Collapsed };
                uIClass.rightConstrRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightConstrRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightConstrRichTextBox.LostFocus += LostFocusClassSaving;
                // right Meth
                uIClass.rightMethCheckBox = new CheckBox { Name = "rightMeth", Width = 16, Height = 16, Margin = new Thickness(320, 20, 0, 0), IsChecked = true, Opacity = 0.4 };
                uIClass.rightMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightMethCheckBox.Click += LostFocusClassSaving;
                Grid rightMethButtonContentGrid = new Grid();
                rightMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.rightMethButton = new Button { Name = "rightMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 57, -30, 37), Content = rightMethButtonContentGrid };
                uIClass.rightMethButton.GotFocus += ChildrenGotFocus;
                uIClass.rightMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightMethRichTextBox = new RichTextBox { Name = "rightMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -66, -430, -332), Visibility = Visibility.Collapsed };
                uIClass.rightMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMethRichTextBox.LostFocus += LostFocusClassSaving;
                // right StaticMeth
                uIClass.rightStaticMethCheckBox = new CheckBox { Name = "rightStaticMeth", Width = 16, Height = 16, Margin = new Thickness(320, 52, 0, 0), IsChecked = true, Opacity = 0.4 };
                uIClass.rightStaticMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightStaticMethCheckBox.Click += LostFocusClassSaving;
                Grid rightStaticMethButtonContentGrid = new Grid();
                rightStaticMethButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 32, 0), Text = "static" });
                rightStaticMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.rightStaticMethButton = new Button { Name = "rightStaticMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 73, -30, 21), Content = rightStaticMethButtonContentGrid };
                uIClass.rightStaticMethButton.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightStaticMethRichTextBox = new RichTextBox { Name = "rightStaticMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -50, -430, -348), Visibility = Visibility.Collapsed };
                uIClass.rightStaticMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightStaticMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticMethRichTextBox.LostFocus += LostFocusClassSaving;
                // right Message
                uIClass.rightMessageCheckBox = new CheckBox { Name = "rightMessage", Width = 16, Height = 16, Margin = new Thickness(320, 90, 0, 0), IsChecked = true, Opacity = 0.4 };
                uIClass.rightMessageCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMessageCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightMessageCheckBox.Click += LostFocusClassSaving;
                Grid rightMessageButtonContentGrid = new Grid();
                rightMessageButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Message()" });
                uIClass.rightMessageButton = new Button { Name = "rightMessage", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 92, -30, 2), Content = rightMessageButtonContentGrid };
                uIClass.rightMessageButton.GotFocus += ChildrenGotFocus;
                uIClass.rightMessageButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightMessageRichTextBox = new RichTextBox { Name = "rightMessageRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -30, -430, -366), Visibility = Visibility.Collapsed };
                uIClass.rightMessageRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightMessageRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMessageRichTextBox.LostFocus += LostFocusClassSaving;

                // Left

                // left A
                uIClass.leftACheckBox = new CheckBox { Name = "leftA", Width = 16, Height = 16, Margin = new Thickness(0, 0, 320, 88), IsChecked = true, Opacity = 0.4 };
                uIClass.leftACheckBox.GotFocus += ChildrenGotFocus;
                uIClass.leftACheckBox.Click += OpenHideButtonsMethod;
                uIClass.leftACheckBox.Click += LostFocusClassSaving;
                Grid leftAButtonContentGrid = new Grid();
                leftAButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 10, 0), Text = "int" });
                leftAButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "a" });
                uIClass.leftAButton = new Button { Name = "leftA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 89, 196)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(-30, 3, 370, 91), Content = leftAButtonContentGrid };
                uIClass.leftAButton.GotFocus += ChildrenGotFocus;
                uIClass.leftAButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.leftARichTextBox = new RichTextBox { Name = "leftARichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(-430, -120, 430, -278), Visibility = Visibility.Collapsed };
                uIClass.leftARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.leftARichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.leftARichTextBox.LostFocus += LostFocusClassSaving;
                // left Meth
                uIClass.leftMethCheckBox = new CheckBox { Name = "leftMeth", Width = 16, Height = 16, Margin = new Thickness(0, 20, 320, 0), IsChecked = true, Opacity = 0.4 };
                uIClass.leftMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.leftMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.leftMethCheckBox.Click += LostFocusClassSaving;
                Grid leftMethButtonContentGrid = new Grid();
                leftMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.leftMethButton = new Button { Name = "leftMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 89, 196)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(-30, 57, 370, 37), Content = leftMethButtonContentGrid };
                uIClass.leftMethButton.GotFocus += ChildrenGotFocus;
                uIClass.leftMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.leftMethRichTextBox = new RichTextBox { Name = "leftMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(-430, -66, 430, -332), Visibility = Visibility.Collapsed };
                uIClass.leftMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.leftMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.leftMethRichTextBox.LostFocus += LostFocusClassSaving;
                // left Static Meth
                uIClass.leftStaticMethCheckBox = new CheckBox { Name = "leftStaticMeth", Width = 16, Height = 16, Margin = new Thickness(0, 52, 320, 0), IsChecked = true, Opacity = 0.4 };
                uIClass.leftStaticMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.leftStaticMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.leftStaticMethCheckBox.Click += LostFocusClassSaving;
                Grid leftStaticMethButtonContentGrid = new Grid();
                leftStaticMethButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 32, 0), Text = "static" });
                leftStaticMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.leftStaticMethButton = new Button { Name = "leftStaticMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 89, 196)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(-30, 73, 370, 21), Content = leftStaticMethButtonContentGrid };
                uIClass.leftStaticMethButton.GotFocus += ChildrenGotFocus;
                uIClass.leftStaticMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.leftStaticMethRichTextBox = new RichTextBox { Name = "leftStaticMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(-430, -50, 430, -348), Visibility = Visibility.Collapsed };
                uIClass.leftStaticMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.leftStaticMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.leftStaticMethRichTextBox.LostFocus += LostFocusClassSaving;

                // Adding elements to classGrid
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
                Canvas.SetLeft(uIClass.classGrid, 0);
                Canvas.SetTop(uIClass.classGrid, 0);
            }            
        }

        // Saving all properties of the selected class
        private void LostFocusClassSaving(object sender, RoutedEventArgs e)
        {
            Grid grid = (sender as FrameworkElement).Parent as Grid; // Получение Grid-а, чьи дочерние элементы потеряли фокус
            if (grid == null)
            {
                grid = sender as Grid; // Либо получение самого грида, который был передан из GridMouseLeftButtonUp()
            }
            using (FileStream fs = new FileStream("D:\\" + grid.Name + ".json", FileMode.Create))
            {
                rtb = sender as RichTextBox;
                DataClass dataClassSaving = new DataClass();
                TextBox[] textBoxes = grid.Children.OfType<TextBox>().ToArray();
                CheckBox[] checkBoxes = grid.Children.OfType<CheckBox>().ToArray();
                RichTextBox[] richTextBoxes = grid.Children.OfType<RichTextBox>().ToArray();

                dataClassSaving.canvasPointClass = currentCanvasPointClass; // установка положения класса (его Grid) на холсте. Для нового класса это (0, 0), для уже созданного и перемещенного - его последнее положение
                foreach (TextBox tb in textBoxes) // Запись текста из всех TextBox-ов класса в соответствующие свойства DataClass
                {
                    if (tb.Name == "classNameTextBox") { dataClassSaving.className = tb.Text; }
                    else if (tb.Name == "classDescriptionTextBox") { dataClassSaving.classDescription = tb.Text; }
                }
                foreach (CheckBox cb in checkBoxes) // Запись значений из всех CheckBox-ов класса в соответствующие свойства DataClass
                {
                    string checkBoxName = cb.Name + "DataClassCheckBox";
                    bool b = (bool)cb.IsChecked;
                    switch (checkBoxName)
                    {
                        case "RightADataClassCheckBox": dataClassSaving.rightADataClassCheckBox = b; break;
                        case "RightStaticADataClassCheckBox": dataClassSaving.rightStaticADataClassCheckBox = b; break;
                        case "RightConstrDataClassCheckBox": dataClassSaving.rightConstrDataClassCheckBox = b; break;
                        case "RightMethDataClassCheckBox": dataClassSaving.rightMethDataClassCheckBox = b; break;
                        case "RightStaticMethDataClassCheckBox": dataClassSaving.rightStaticMethDataClassCheckBox = b; break;
                        case "RightMessageDataClassCheckBox": dataClassSaving.rightMessageDataClassCheckBox = b; break;
                        case "LeftADataClassCheckBox": dataClassSaving.leftADataClassCheckBox = b; break;
                        case "LeftMethDataClassCheckBox": dataClassSaving.leftMethDataClassCheckBox = b; break;
                        case "LeftStaticMethDataClassCheckBox": dataClassSaving.leftStaticMethDataClassCheckBox = b; break;
                    }
                }
                foreach (RichTextBox rtb in richTextBoxes) // Запись всех Inline-ов всех RichTextBox-ов класса в соответствующие им листы DataClass
                {
                    string richTextBoxName = rtb.Name;
                    switch (richTextBoxName)
                    {
                        case "rightARichTextBox":
                            Paragraph paragraph1 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph1.Inlines)
                            {
                                dataClassSaving.rightAInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.rightAInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.rightAInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.rightAInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "rightStaticARichTextBox":
                            Paragraph paragraph2 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph2.Inlines)
                            {
                                dataClassSaving.rightStaticAInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.rightStaticAInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.rightStaticAInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.rightStaticAInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "rightConstrRichTextBox":
                            Paragraph paragraph3 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph3.Inlines)
                            {
                                dataClassSaving.rightConstrInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.rightConstrInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.rightConstrInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.rightConstrInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "rightMethRichTextBox":
                            Paragraph paragraph4 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph4.Inlines)
                            {
                                dataClassSaving.rightMethInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.rightMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.rightMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.rightMethInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "rightStaticMethRichTextBox":
                            Paragraph paragraph5 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph5.Inlines)
                            {
                                dataClassSaving.rightStaticMethInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.rightStaticMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.rightStaticMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.rightStaticMethInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "rightMessageRichTextBox":
                            Paragraph paragraph6 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph6.Inlines)
                            {
                                dataClassSaving.rightMessageInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.rightMessageInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.rightMessageInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.rightMessageInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "leftARichTextBox":
                            Paragraph paragraph7 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph7.Inlines)
                            {
                                dataClassSaving.leftAInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.leftAInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.leftAInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.leftAInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "leftMethRichTextBox":
                            Paragraph paragraph8 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph8.Inlines)
                            {
                                dataClassSaving.leftMethInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.leftMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.leftMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.leftMethInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                        case "leftStaticMethRichTextBox":
                            Paragraph paragraph9 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            foreach (Inline i in paragraph9.Inlines)
                            {
                                dataClassSaving.leftStaticMethInlineTextFontWeight.Add(i.FontWeight.ToString()); // Запись типа текста (жирный/не жирный) в Inline-е
                                dataClassSaving.leftStaticMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                dataClassSaving.leftStaticMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                dataClassSaving.leftStaticMethInlineTextColor.Add(i.Foreground.ToString()); // Запись размера шрифта текста в Inline-е
                            }
                            break;
                    }
                }
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                jsonSerializer.WriteObject(fs, dataClassSaving);
            }
        }

        // Измненение имени выделенного класса
        private void ChildrenGotFocus(object sender, RoutedEventArgs e) // При выделении classNameTextBox
        {
            currentClassName = ((sender as FrameworkElement).Parent as Grid).Name; // в эту переменную записывается текущее имя выделенного класса
            currentCanvasPointClass = new Point((double)((sender as FrameworkElement).Parent as Grid).GetValue(Canvas.LeftProperty), (double)((sender as FrameworkElement).Parent as Grid).GetValue(Canvas.TopProperty));
        }

        // Удаление файла со старым именем класса
        private void ChangeClassNameLostFocus(object sender, RoutedEventArgs e) // Изменяет имя класса. Создает новый файл с новым именем класса, сохранив в него все свойства этого класса, а так же удаляет файл со старым именем класса
        {
            Grid grid = (sender as FrameworkElement).Parent as Grid;
            grid.Name = (sender as TextBox).Text;
            File.Delete("D://" + currentClassName + ".json");
        }

        //
        private void OpenHideClassDescriptionTextBoxMethod(object sender, RoutedEventArgs e) // Makes ClassDescriptionTextBox Visible when classDescriptionButton is pressed
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
        private void OpenHideRichTextBoxesMethod(object sender, RoutedEventArgs e) // Makes RichTextBoxes Visible/Collapsed
        {
            int a = 0;
            string buttonName = (sender as Button).Name + "RichTextBox";
            RichTextBox[] richTextBoxes = ((sender as Button).Parent as Grid).Children.OfType<RichTextBox>().ToArray();
            foreach (RichTextBox r in richTextBoxes)
            {
                if (r.Name == buttonName && richTextBoxes[a].Visibility == Visibility.Collapsed) { richTextBoxes[a].Visibility = Visibility.Visible; }
                else if (r.Name == buttonName && richTextBoxes[a].Visibility == Visibility.Visible) { richTextBoxes[a].Visibility = Visibility.Collapsed; }
                a++;
            }
        }
        private void OpenHideButtonsMethod(object sender, RoutedEventArgs e) // Makes Buttons Visible/Collapsed
        {
            int a = 0;
            string checkBoxName = (sender as CheckBox).Name;
            Button[] buttons = ((sender as CheckBox).Parent as Grid).Children.OfType<Button>().ToArray();
            foreach (Button b in buttons)
            {
                if (b.Name == checkBoxName && buttons[a].Visibility == Visibility.Collapsed) { buttons[a].Visibility = Visibility.Visible; }
                else if (b.Name == checkBoxName && buttons[a].Visibility == Visibility.Visible) { buttons[a].Visibility = Visibility.Collapsed; }
                a++;
            }
        }

        // Реализация перемещения мышкой UIClass-а
        private void GridMouseLeftButtonDown(object sender, MouseButtonEventArgs e) // Захват родительского Grid-а класса. Это событие не работет, если нажать на дочерние элементы Grid-а
        {
            if (e.OriginalSource.GetType().ToString() == "System.Windows.Controls.Grid") // Код ниже сработает, если событие было вызвано по нажатию на родительский элемент (т.е. Grid), а не по нажатию на его дочерние элементы
            {
                TakeClassGrid = sender as Grid;
                GetMouseClass = true;
                Mouse.Capture(TakeClassGrid);
                mousePosClassCanvasOld = e.GetPosition(canvas);
            }
        }
        private void GridMouseMove(object sender, MouseEventArgs e) // Само перемещение классов по холсту
        {
            if (GetMouseClass)
            {
                Point mousePosClassCanvasNew = e.GetPosition(canvas);
                Point CanvasElement = new Point((double)TakeClassGrid.GetValue(Canvas.LeftProperty), (double)TakeClassGrid.GetValue(Canvas.TopProperty));
                Canvas.SetLeft(TakeClassGrid, CanvasElement.X + (mousePosClassCanvasNew.X - mousePosClassCanvasOld.X));
                Canvas.SetTop(TakeClassGrid, CanvasElement.Y + (mousePosClassCanvasNew.Y - mousePosClassCanvasOld.Y));
                mousePosClassCanvasOld.X = mousePosClassCanvasNew.X;
                mousePosClassCanvasOld.Y = mousePosClassCanvasNew.Y;
            }
        }
        private void GridMouseLeftButtonUp(object sender, MouseButtonEventArgs e) // Когда кнопка мыши отпущена, перемещение классов по холсту прекращается
        {
            if (TakeClassGrid != null)
            {
                GetMouseClass = false;
                TakeClassGrid.ReleaseMouseCapture();
                mousePosClassCanvasOld = new Point(0, 0);
                currentCanvasPointClass = new Point((double)TakeClassGrid.GetValue(Canvas.LeftProperty), (double)TakeClassGrid.GetValue(Canvas.TopProperty));
                TakeClassGrid = null;
                LostFocusClassSaving(sender, e);
            }
        }

        // Реализация перемещения мышкой холста (canvas)
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) // Если левая кнопка мыши нажата вниз на холсте (canvas)
        {
            if (e.OriginalSource.GetType().ToString() == "System.Windows.Controls.Canvas") // Код ниже сработает, если событие было вызвано по нажатию на холст (canvas)
            {
                GetMouseCanvas = true;
                mousePosCanvasOld = e.GetPosition(W1);
            }
        }
        private void canvas_MouseMove(object sender, MouseEventArgs e) // Само перемещение холста (canvas) по ScrollViewer
        {

            if (GetMouseCanvas)
            {
                Point mousePosCanvasNew = e.GetPosition(W1);
                ScrollViewer1.ScrollToHorizontalOffset(ScrollViewer1.HorizontalOffset - (mousePosCanvasNew.X - mousePosCanvasOld.X));
                ScrollViewer1.ScrollToVerticalOffset(ScrollViewer1.VerticalOffset - (mousePosCanvasNew.Y - mousePosCanvasOld.Y));
                mousePosCanvasOld.X = mousePosCanvasNew.X;
                mousePosCanvasOld.Y = mousePosCanvasNew.Y;
            }
        }
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) // Когда кнопка мыши отпущена, перемещение холста прекращается
        {
            GetMouseCanvas = false;
        }

        // Open Menu
        private void OpenMenuButtonClick(object sender, RoutedEventArgs e) // Открывает боковое меню
        {
            if (Menu.Margin.Left == 1880) { Menu.Margin = new Thickness(1572, 0, 0, 0); }
            else { Menu.Margin = new Thickness(1880, 0, 0, 0); }
        }

        // Change Text Color of RichTextBoxes
        private void ChangeTextColorButton(object sender, RoutedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text != " " && rtb.Selection.Text.Length > 0) // Если выделена хоть одна буква - то открывает ColorDialog для выбора цвета выделенного текста
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                colorDialog.ShowDialog();
                rtb.Selection.ApplyPropertyValue(ForegroundProperty, new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
                LostFocusClassSaving(rtb, e);
                rtb = null;
            }
        }

        // Change TextSize of RichTextBoxes
        private void ChangeTextSizeButtons(object sender, RoutedEventArgs e)
        {
            if (rtb != null && rtb.Selection.Text != " " && rtb.Selection.Text.Length > 0) // Происходит увеличение шрифта выделенного фрагмента текста
            {
                if((sender as Button).Name == "Apply"){ rtb.Selection.ApplyPropertyValue(FontSizeProperty, FontSizeTextBox.Text); }
                else if((sender as Button).Name == "Plus")
                {
                    int a = Convert.ToInt32(rtb.Selection.GetPropertyValue(TextElement.FontSizeProperty));
                    rtb.Selection.ApplyPropertyValue(FontSizeProperty, (++a).ToString());
                }
                else if ((sender as Button).Name == "Minus")
                {
                    int a = Convert.ToInt32(rtb.Selection.GetPropertyValue(TextElement.FontSizeProperty));
                    rtb.Selection.ApplyPropertyValue(FontSizeProperty, (--a).ToString());
                }
            }
        }








        //FlowDocument flowDocument = new FlowDocument(); // FlowDocument, чтобы потом в него добавить Paragraph
        //Paragraph paragraph = new Paragraph(); // Paragraph, чтобы в него можно было добавить коллекцию Inline-ов
        //flowDocument.Blocks.Add(paragraph); // Добавление Paragraph-а в FlowDocument
        //uIClass.rightARichTextBox.Document = flowDocument; // Добавление FlowDocument-а в RichTextBox






    }
}
