using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Grid FirstClassGrid;
        Grid SecondClassGrid;
        Point mousePosClassCanvasOld = new Point(0, 0);
        Point mousePosCanvasOld = new Point(0, 0);
        Point currentCanvasPointClass = new Point(0, 0);
        ObservableCollection<string> allClassesList = new ObservableCollection<string>();
        public double canvasSize;
        public string currentClassName = "";
        public string classParentName = "";
        public string nameOfParent = "";
        bool GetMouseClass = false;
        bool GetMouseCanvas = false;
        public MainWindow()
        {
            InitializeComponent();
            LoadAllClasses();
        }

        // Загрузка всех классов (из папки "Classes") на холст
        public void LoadAllClasses()
        {
            if (Directory.Exists("D:\\Unity Class Tree"))
            {
                if(File.Exists("D:\\Unity Class Tree\\Canvas Size.json"))
                {
                    using (FileStream fs = new FileStream("D:\\Unity Class Tree\\Canvas Size.json", FileMode.Open))
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(double));
                        canvasSize = (double)jsonSerializer.ReadObject(fs);
                    }
                }
                RoutedEventArgs e = new RoutedEventArgs();
                string[] allClassFiles = Directory.GetFiles("D:\\Unity Class Tree\\Classes");
                foreach (string s in allClassFiles)
                {
                    using (FileStream fs2 = new FileStream(s, FileMode.Open))
                    {
                        DataContractJsonSerializer jsonSerializer2 = new DataContractJsonSerializer(typeof(DataClass));
                        try
                        {
                            DataClass dataClass = (DataClass)jsonSerializer2.ReadObject(fs2);
                            CreateNewClass(dataClass, e);
                        }
                        catch (Exception) { MessageBox.Show("Проблемы с файлом: " + s); }
                    }
                    string classNameInAllClassesList = s.Replace("D:\\Unity Class Tree\\Classes\\", "");
                    classNameInAllClassesList = classNameInAllClassesList.Replace(".json", "");
                    allClassesList.Add(classNameInAllClassesList);
                }
                allClassesListBox.ItemsSource = allClassesList;
            }          
        }
        
        // Создание нового класса
        private void CreateNewClass(object sender, RoutedEventArgs e)
        {
            DataClass dt = new DataClass();
            bool createOreLoadNewClass = false;
            if(sender.GetType().ToString() == "Unity_Class_Tree.DataClass")
            {
               dt = sender as DataClass;
               NewClassNameTextBox.Text = dt.className;
                createOreLoadNewClass = true;
            }
            else
            {
                if (File.Exists("D:\\Unity Class Tree\\Classes\\" + NewClassNameTextBox.Text + ".json")) { MessageBox.Show("Класс с таким именем уже создан."); } // Проверка введенного нового имени класса. Если такое имя уже есть, не создаст класс
                else
                {
                    //___CREATE DataClass OBJECT___
                    dt = new DataClass();
                    dt.className = NewClassNameTextBox.Text;

                    // Создание файла (в папке) для этого класса
                    using (FileStream fs = new FileStream("D:\\Unity Class Tree\\Classes\\" + dt.className + ".json", FileMode.Create))
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                        jsonSerializer.WriteObject(fs, dt);
                    }
                    createOreLoadNewClass = true;
                }
            }
            if(createOreLoadNewClass)
            {
                int a = 0;
                //___CREATE UIClass OBJECT___

                UIClass uIClass = new UIClass();

                // classGrid
                uIClass.classGrid = new Grid { Name = NewClassNameTextBox.Text, Tag = "", Width = 400, Height = 109, Background = new SolidColorBrush(Color.FromRgb(108, 108, 109)) };
                uIClass.classGrid.MouseLeftButtonDown += GridMouseLeftButtonDown;
                uIClass.classGrid.MouseLeftButtonUp += GridMouseLeftButtonUp;
                uIClass.classGrid.MouseMove += GridMouseMove;
                // classBorders
                uIClass.classBorderWhite = new Border { Name = "classBorderWhite", Width = 300, Height = 50, Background = new SolidColorBrush(Colors.White), CornerRadius = new CornerRadius(6), VerticalAlignment = VerticalAlignment.Center, BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 22) };
                // classDescriptionButton
                uIClass.classDescriptionButton = new Button { Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), Foreground = new SolidColorBrush(Color.FromRgb(43, 175, 62)), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 88), Content = "???" };
                uIClass.classDescriptionButton.Click += OpenHideClassDescriptionTextBoxMethod;
                // 2 classTextboxes
                uIClass.classNameTextBlock = new TextBlock { Name = "classNameTextBlock", Width = 296, FontSize = 19, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(43, 145, 175)), Margin = new Thickness(0, 0, 0, 22), Text = dt.className };
                uIClass.classNameTextBlock.GotFocus += ChildrenGotFocus;
                uIClass.classDescriptionTextBox = new TextBox { Name = "classDescriptionTextBox", FontFamily = new FontFamily("Calibri"), Width = 296, Height = 140, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, TextWrapping = TextWrapping.Wrap, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(52, -140, 52, 110), Visibility = Visibility.Collapsed, Text = dt.classDescription };
                uIClass.classDescriptionTextBox.GotFocus += ChildrenGotFocus;
                uIClass.classDescriptionTextBox.LostFocus += LostFocusClassSaving;

                // Right

                // right A
                uIClass.rightACheckBox = new CheckBox { Name = "rightA", Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 88), IsChecked = dt.rightADataClassCheckBox, Opacity = 0.4 };
                uIClass.rightACheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightACheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightACheckBox.Click += LostFocusClassSaving;
                Grid rightAButtonContentGrid = new Grid();
                rightAButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 10, 0), Text = "int" });
                rightAButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "a" });
                uIClass.rightAButton = new Button { Name = "rightA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 3, -30, 91), Content = rightAButtonContentGrid };
                if (dt.rightADataClassCheckBox != true) { uIClass.rightAButton.Visibility = Visibility.Collapsed; }
                uIClass.rightAButton.GotFocus += ChildrenGotFocus;
                uIClass.rightAButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightARichTextBox = new RichTextBox { FontFamily = new FontFamily("Calibri"), Name = "rightARichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -120, -430, -278), Visibility = Visibility.Collapsed, FontStyle = FontStyles.Normal };
                uIClass.rightARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightARichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightARichTextBox.LostFocus += LostFocusClassSaving;
                if(dt.rightAInlineText.Count > 0)
                {
                    foreach (string t in dt.rightAInlineText)
                    {
                        FlowDocument rightAflowDocument = new FlowDocument();
                        Paragraph rightAParagraph = new Paragraph();
                        rightAflowDocument.Blocks.Add(rightAParagraph);
                        uIClass.rightARichTextBox.Document = rightAflowDocument;
                        Run run = new Run { Text = t, FontSize = dt.rightAInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.rightAInlineTextColor[a]) };
                        if (dt.rightAInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        rightAParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // right Static A
                uIClass.rightStaticACheckBox = new CheckBox { Name = "rightStaticA", Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 56), IsChecked = dt.rightStaticADataClassCheckBox, Opacity = 0.4 };
                uIClass.rightStaticACheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticACheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightStaticACheckBox.Click += LostFocusClassSaving;
                Grid rightStaticAButtonContentGrid = new Grid();
                rightStaticAButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 10, 0), Text = "static int" });
                rightStaticAButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "a" });
                uIClass.rightStaticAButton = new Button { Name = "rightStaticA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 19, -30, 75), Content = rightStaticAButtonContentGrid };
                if (dt.rightStaticADataClassCheckBox != true) { uIClass.rightStaticAButton.Visibility = Visibility.Collapsed; }
                uIClass.rightStaticAButton.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticAButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightStaticARichTextBox = new RichTextBox { Name = "rightStaticARichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -110, -430, -300), Visibility = Visibility.Collapsed };
                uIClass.rightStaticARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightStaticARichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticARichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument rightStaticAflowDocument = new FlowDocument();
                Paragraph rightStaticAParagraph = new Paragraph();
                rightStaticAflowDocument.Blocks.Add(rightStaticAParagraph);
                uIClass.rightStaticARichTextBox.Document = rightStaticAflowDocument;
                if (dt.rightStaticAInlineText.Count > 0)
                {
                    foreach (string t in dt.rightStaticAInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.rightAInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.rightStaticAInlineTextColor[a]) };
                        if (dt.rightStaticAInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        rightStaticAParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // right Constr
                uIClass.rightConstrCheckBox = new CheckBox { Name = "rightConstr", Width = 16, Height = 16, Margin = new Thickness(320, 0, 0, 18), IsChecked = dt.rightConstrDataClassCheckBox, Opacity = 0.4 };
                uIClass.rightConstrCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightConstrCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightConstrCheckBox.Click += LostFocusClassSaving;
                Grid rightConstrButtonContentGrid = new Grid();
                rightConstrButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Constr()" });
                uIClass.rightConstrButton = new Button { Name = "rightConstr", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 38, -30, 56), Content = rightConstrButtonContentGrid };
                if (dt.rightConstrDataClassCheckBox != true) { uIClass.rightConstrButton.Visibility = Visibility.Collapsed; }
                uIClass.rightConstrButton.GotFocus += ChildrenGotFocus;
                uIClass.rightConstrButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightConstrRichTextBox = new RichTextBox { Name = "rightConstrRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -90, -430, -318), Visibility = Visibility.Collapsed };
                uIClass.rightConstrRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightConstrRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightConstrRichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument rightConstrflowDocument = new FlowDocument();
                Paragraph rightConstrParagraph = new Paragraph();
                rightConstrflowDocument.Blocks.Add(rightConstrParagraph);
                uIClass.rightConstrRichTextBox.Document = rightConstrflowDocument;
                if (dt.rightConstrInlineText.Count > 0)
                {
                    foreach (string t in dt.rightConstrInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.rightConstrInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.rightConstrInlineTextColor[a]) };
                        if (dt.rightConstrInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        rightConstrParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // right Meth
                uIClass.rightMethCheckBox = new CheckBox { Name = "rightMeth", Width = 16, Height = 16, Margin = new Thickness(320, 20, 0, 0), IsChecked = dt.rightMethDataClassCheckBox, Opacity = 0.4 };
                uIClass.rightMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightMethCheckBox.Click += LostFocusClassSaving;
                Grid rightMethButtonContentGrid = new Grid();
                rightMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.rightMethButton = new Button { Name = "rightMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 57, -30, 37), Content = rightMethButtonContentGrid };
                if (dt.rightMethDataClassCheckBox != true) { uIClass.rightMethButton.Visibility = Visibility.Collapsed; }
                uIClass.rightMethButton.GotFocus += ChildrenGotFocus;
                uIClass.rightMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightMethRichTextBox = new RichTextBox { Name = "rightMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -66, -430, -332), Visibility = Visibility.Collapsed };
                uIClass.rightMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMethRichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument rightMethflowDocument = new FlowDocument();
                Paragraph rightMethParagraph = new Paragraph();
                rightMethflowDocument.Blocks.Add(rightMethParagraph);
                uIClass.rightMethRichTextBox.Document = rightMethflowDocument;
                if (dt.rightMethInlineText.Count > 0)
                {
                    foreach (string t in dt.rightMethInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.rightMethInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.rightMethInlineTextColor[a]) };
                        if (dt.rightMethInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        rightMethParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // right StaticMeth
                uIClass.rightStaticMethCheckBox = new CheckBox { Name = "rightStaticMeth", Width = 16, Height = 16, Margin = new Thickness(320, 52, 0, 0), IsChecked = dt.rightStaticMethDataClassCheckBox, Opacity = 0.4 };
                uIClass.rightStaticMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightStaticMethCheckBox.Click += LostFocusClassSaving;
                Grid rightStaticMethButtonContentGrid = new Grid();
                rightStaticMethButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 32, 0), Text = "static" });
                rightStaticMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.rightStaticMethButton = new Button { Name = "rightStaticMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 73, -30, 21), Content = rightStaticMethButtonContentGrid };
                if (dt.rightStaticMethDataClassCheckBox != true) { uIClass.rightStaticMethButton.Visibility = Visibility.Collapsed; }
                uIClass.rightStaticMethButton.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightStaticMethRichTextBox = new RichTextBox { Name = "rightStaticMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -50, -430, -348), Visibility = Visibility.Collapsed };
                uIClass.rightStaticMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightStaticMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightStaticMethRichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument rightStaticMethflowDocument = new FlowDocument();
                Paragraph rightStaticMethParagraph = new Paragraph();
                rightStaticMethflowDocument.Blocks.Add(rightStaticMethParagraph);
                uIClass.rightStaticMethRichTextBox.Document = rightStaticMethflowDocument;
                if (dt.rightStaticMethInlineText.Count > 0)
                {
                    foreach (string t in dt.rightStaticMethInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.rightStaticMethInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.rightStaticMethInlineTextColor[a]) };
                        if (dt.rightStaticMethInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        rightStaticMethParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // right Message
                uIClass.rightMessageCheckBox = new CheckBox { Name = "rightMessage", Width = 16, Height = 16, Margin = new Thickness(320, 90, 0, 0), IsChecked = dt.rightMessageDataClassCheckBox, Opacity = 0.4 };
                uIClass.rightMessageCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMessageCheckBox.Click += OpenHideButtonsMethod;
                uIClass.rightMessageCheckBox.Click += LostFocusClassSaving;
                Grid rightMessageButtonContentGrid = new Grid();
                rightMessageButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Message()" });
                uIClass.rightMessageButton = new Button { Name = "rightMessage", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(43, 175, 62)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(370, 92, -30, 2), Content = rightMessageButtonContentGrid };
                if (dt.rightMessageDataClassCheckBox != true) { uIClass.rightMessageButton.Visibility = Visibility.Collapsed; }
                uIClass.rightMessageButton.GotFocus += ChildrenGotFocus;
                uIClass.rightMessageButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.rightMessageRichTextBox = new RichTextBox { Name = "rightMessageRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(430, -30, -430, -366), Visibility = Visibility.Collapsed };
                uIClass.rightMessageRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.rightMessageRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.rightMessageRichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument rightMessageflowDocument = new FlowDocument();
                Paragraph rightMessageParagraph = new Paragraph();
                rightMessageflowDocument.Blocks.Add(rightMessageParagraph);
                uIClass.rightMessageRichTextBox.Document = rightMessageflowDocument;
                if (dt.rightMessageInlineText.Count > 0)
                {
                    foreach (string t in dt.rightMessageInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.rightMessageInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.rightMessageInlineTextColor[a]) };
                        if (dt.rightMessageInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        rightMessageParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }

                // Left

                // left A
                uIClass.leftACheckBox = new CheckBox { Name = "leftA", Width = 16, Height = 16, Margin = new Thickness(0, 0, 320, 88), IsChecked = dt.leftADataClassCheckBox, Opacity = 0.4 };
                uIClass.leftACheckBox.GotFocus += ChildrenGotFocus;
                uIClass.leftACheckBox.Click += OpenHideButtonsMethod;
                uIClass.leftACheckBox.Click += LostFocusClassSaving;
                Grid leftAButtonContentGrid = new Grid();
                leftAButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 10, 0), Text = "int" });
                leftAButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "a" });
                uIClass.leftAButton = new Button { Name = "leftA", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 89, 196)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(-30, 3, 370, 91), Content = leftAButtonContentGrid };
                if (dt.leftADataClassCheckBox != true) { uIClass.leftAButton.Visibility = Visibility.Collapsed; }
                uIClass.leftAButton.GotFocus += ChildrenGotFocus;
                uIClass.leftAButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.leftARichTextBox = new RichTextBox { Name = "leftARichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(-430, -120, 430, -278), Visibility = Visibility.Collapsed };
                uIClass.leftARichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.leftARichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.leftARichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument leftAflowDocument = new FlowDocument();
                Paragraph leftAParagraph = new Paragraph();
                leftAflowDocument.Blocks.Add(leftAParagraph);
                uIClass.leftARichTextBox.Document = leftAflowDocument;
                if (dt.leftAInlineText.Count > 0)
                {
                    foreach (string t in dt.leftAInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.leftAInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.leftAInlineTextColor[a]) };
                        if (dt.leftAInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        leftAParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // left Meth
                uIClass.leftMethCheckBox = new CheckBox { Name = "leftMeth", Width = 16, Height = 16, Margin = new Thickness(0, 20, 320, 0), IsChecked = dt.leftMethDataClassCheckBox, Opacity = 0.4 };
                uIClass.leftMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.leftMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.leftMethCheckBox.Click += LostFocusClassSaving;
                Grid leftMethButtonContentGrid = new Grid();
                leftMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.leftMethButton = new Button { Name = "leftMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 89, 196)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(-30, 57, 370, 37), Content = leftMethButtonContentGrid };
                if (dt.leftMethDataClassCheckBox != true) { uIClass.leftMethButton.Visibility = Visibility.Collapsed; }
                uIClass.leftMethButton.GotFocus += ChildrenGotFocus;
                uIClass.leftMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.leftMethRichTextBox = new RichTextBox { Name = "leftMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(-430, -66, 430, -332), Visibility = Visibility.Collapsed };
                uIClass.leftMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.leftMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.leftMethRichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument leftMethflowDocument = new FlowDocument();
                Paragraph leftMethParagraph = new Paragraph();
                leftMethflowDocument.Blocks.Add(leftMethParagraph);
                uIClass.leftMethRichTextBox.Document = leftMethflowDocument;
                if (dt.leftMethInlineText.Count > 0)
                {
                    foreach (string t in dt.leftMethInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.leftMethInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.leftMethInlineTextColor[a]) };
                        if (dt.leftMethInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        leftMethParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }
                // left Static Meth
                uIClass.leftStaticMethCheckBox = new CheckBox { Name = "leftStaticMeth", Width = 16, Height = 16, Margin = new Thickness(0, 52, 320, 0), IsChecked = dt.leftStaticMethDataClassCheckBox, Opacity = 0.4 };
                uIClass.leftStaticMethCheckBox.GotFocus += ChildrenGotFocus;
                uIClass.leftStaticMethCheckBox.Click += OpenHideButtonsMethod;
                uIClass.leftStaticMethCheckBox.Click += LostFocusClassSaving;
                Grid leftStaticMethButtonContentGrid = new Grid();
                leftStaticMethButtonContentGrid.Children.Add(new TextBlock { Foreground = new SolidColorBrush(Color.FromRgb(40, 80, 252)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 32, 0), Text = "static" });
                leftStaticMethButtonContentGrid.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, Text = "Meth()" });
                uIClass.leftStaticMethButton = new Button { Name = "leftStaticMeth", Width = 60, Height = 16, Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Color.FromRgb(65, 89, 196)), VerticalAlignment = VerticalAlignment.Center, BorderThickness = new Thickness(2), Margin = new Thickness(-30, 73, 370, 21), Content = leftStaticMethButtonContentGrid };
                if (dt.leftStaticMethDataClassCheckBox != true) { uIClass.leftStaticMethButton.Visibility = Visibility.Collapsed; }
                uIClass.leftStaticMethButton.GotFocus += ChildrenGotFocus;
                uIClass.leftStaticMethButton.Click += OpenHideRichTextBoxesMethod;
                uIClass.leftStaticMethRichTextBox = new RichTextBox { Name = "leftStaticMethRichTextBox", Width = 400, Height = 260, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, Foreground = new SolidColorBrush(Colors.Black), BorderBrush = new SolidColorBrush(Color.FromRgb(121, 120, 120)), BorderThickness = new Thickness(1), Margin = new Thickness(-430, -50, 430, -348), Visibility = Visibility.Collapsed };
                uIClass.leftStaticMethRichTextBox.SetValue(Paragraph.LineHeightProperty, 0.1);
                uIClass.leftStaticMethRichTextBox.GotFocus += ChildrenGotFocus;
                uIClass.leftStaticMethRichTextBox.LostFocus += LostFocusClassSaving;
                FlowDocument leftStaticMethflowDocument = new FlowDocument();
                Paragraph leftStaticMethParagraph = new Paragraph();
                leftStaticMethflowDocument.Blocks.Add(leftStaticMethParagraph);
                uIClass.leftStaticMethRichTextBox.Document = leftStaticMethflowDocument;
                if (dt.leftStaticMethInlineText.Count > 0)
                {
                    foreach (string t in dt.leftStaticMethInlineText)
                    {
                        Run run = new Run { Text = t, FontSize = dt.leftStaticMethInlineTextFontSize[a], Foreground = new SolidColorBrush(dt.leftStaticMethInlineTextColor[a]) };
                        if (dt.leftStaticMethInlineTextFontWeight[a]) { run.FontWeight = FontWeights.Bold; }
                        leftStaticMethParagraph.Inlines.Add(run);
                        a++;
                    }
                    a = 0;
                }

                // Adding elements to classGrid
                uIClass.classGrid.Children.Add(uIClass.classBorderWhite);
                uIClass.classGrid.Children.Add(uIClass.classDescriptionButton);
                uIClass.classGrid.Children.Add(uIClass.classNameTextBlock);
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
                bigCanvas.Children.Add(uIClass.classGrid);
                Canvas.SetLeft(uIClass.classGrid, dt.canvasPointClass.X);
                Canvas.SetTop(uIClass.classGrid, dt.canvasPointClass.Y);

                string[] allClasses = Directory.GetFiles("D:\\Unity Class Tree\\Classes"); // Список всех классов в папке Classes
                foreach (string s in allClasses) // Загрузка положения родителя на холсте, чтобы нарисовать линию от дочернего класса
                {
                    if (s == "D:\\Unity Class Tree\\Classes\\" + dt.parentClassNameForThis + ".json") // Находится файл с именем родительского класса
                    {
                        using (FileStream fs = new FileStream(s, FileMode.Open))
                        {
                            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                            try
                            {
                                DataClass parentClass = (DataClass)jsonSerializer.ReadObject(fs); // Загружается файл родительского класса
                                Line line = new Line { Name = dt.className, X1 = parentClass.canvasPointClass.X + 200, Y1 = parentClass.canvasPointClass.Y + 109, X2 = dt.canvasPointClass.X + 200, Y2 = dt.canvasPointClass.Y, Fill = new SolidColorBrush(Colors.Yellow), Stroke = new SolidColorBrush(Color.FromRgb(253, 221, 4)), StrokeThickness = 4 };
                                bigCanvas.Children.Add(line);
                        }
                            catch (Exception ex)
                        {
                            MessageBox.Show("Проблемы с файлом: " + s + "    " + ex.Message);
                        }
                    }
                    }
                }
            }                
        }

        // Удаление класса по его имени
        private void DeleteClass(object sender, RoutedEventArgs e)
        {
            if(File.Exists("D://Unity Class Tree//Classes//" + NewClassNameTextBox.Text + ".json"))
            {
                if (MessageBox.Show("Are you sure you want to delete this class?", "Deleting the class", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int a = 0;
                    File.Delete("D://Unity Class Tree//Classes//" + NewClassNameTextBox.Text + ".json");
                    Grid[] grids = bigCanvas.Children.OfType<Grid>().ToArray();
                    foreach(Grid g in grids)
                    {
                        if (g.Name == NewClassNameTextBox.Text) { bigCanvas.Children.Remove(grids[a]); }
                        a++;
                    }                    
                }
            }
        }

        // Saving all properties of the selected class
        private void LostFocusClassSaving(object sender, RoutedEventArgs e)
        {
            Grid grid = (sender as FrameworkElement).Parent as Grid; // Получение Grid-а, чьи дочерние элементы потеряли фокус
            if (grid == null)
            {
                grid = sender as Grid; // Либо получение самого грида, который был передан из GridMouseLeftButtonUp() или из ClassConnectButtonClick()
            }
            try
            {
                using (FileStream fs2 = new FileStream("D:\\Unity Class Tree\\Classes\\" + grid.Name + ".json", FileMode.Open)) // Проверяет, был -ли у вызывающего этот метод класса родитель, записанный в свойстве parentClassNameForThis
                {
                    DataContractJsonSerializer jsonSerializer2 = new DataContractJsonSerializer(typeof(DataClass));
                    DataClass YesOrNoParent = (DataClass)jsonSerializer2.ReadObject(fs2);
                    if (YesOrNoParent.parentClassNameForThis != null) { nameOfParent = YesOrNoParent.parentClassNameForThis; }
                }
            }
            catch(Exception)
            {

            }

            using (FileStream fs = new FileStream("D:\\Unity Class Tree\\Classes\\" + grid.Name + ".json", FileMode.Create))
            {
                rtb = sender as RichTextBox;
                DataClass dataClassSaving = new DataClass();
                TextBox[] textBoxes = grid.Children.OfType<TextBox>().ToArray();
                TextBlock[] textBlocks = grid.Children.OfType<TextBlock>().ToArray();
                CheckBox[] checkBoxes = grid.Children.OfType<CheckBox>().ToArray();
                RichTextBox[] richTextBoxes = grid.Children.OfType<RichTextBox>().ToArray();

                dataClassSaving.canvasPointClass = currentCanvasPointClass; // установка положения класса (его Grid) на холсте. Для нового класса это (0, 0), для уже созданного и перемещенного - его последнее положение
                dataClassSaving.parentClassNameForThis = nameOfParent;
                if (grid.Tag.ToString() == "SecondClassGrid")
                {
                    if (grid.Tag.ToString() == "SecondClassGrid" && FirstClassGrid != null)
                    {
                        dataClassSaving.parentClassNameForThis = FirstClassGrid.Name;
                    }
                }
                foreach (TextBox tb in textBoxes) // Запись текста из всех TextBox-ов класса в соответствующие свойства DataClass
                {
                    if (tb.Name == "classDescriptionTextBox") { dataClassSaving.classDescription = tb.Text; }
                }
                foreach (TextBlock tbl in textBlocks) // Запись текста из всех TextBlock-ов класса в соответствующие свойства DataClass
                {
                    if (tbl.Name == "classNameTextBlock") { dataClassSaving.className = tbl.Text; }
                }
                foreach (CheckBox cb in checkBoxes) // Запись значений из всех CheckBox-ов класса в соответствующие свойства DataClass
                {
                    string checkBoxName = cb.Name + "DataClassCheckBox";
                    bool b = cb.IsChecked.Value;
                    switch (checkBoxName)
                    {
                        case "rightADataClassCheckBox": dataClassSaving.rightADataClassCheckBox = b; break;
                        case "rightStaticADataClassCheckBox": dataClassSaving.rightStaticADataClassCheckBox = b; break;
                        case "rightConstrDataClassCheckBox": dataClassSaving.rightConstrDataClassCheckBox = b; break;
                        case "rightMethDataClassCheckBox": dataClassSaving.rightMethDataClassCheckBox = b; break;
                        case "rightStaticMethDataClassCheckBox": dataClassSaving.rightStaticMethDataClassCheckBox = b; break;
                        case "rightMessageDataClassCheckBox": dataClassSaving.rightMessageDataClassCheckBox = b; break;
                        case "leftADataClassCheckBox": dataClassSaving.leftADataClassCheckBox = b; break;
                        case "leftMethDataClassCheckBox": dataClassSaving.leftMethDataClassCheckBox = b; break;
                        case "leftStaticMethDataClassCheckBox": dataClassSaving.leftStaticMethDataClassCheckBox = b; break;
                    }
                }
                foreach (RichTextBox rtb in richTextBoxes) // Запись всех Inline-ов всех RichTextBox-ов класса в соответствующие им листы DataClass
                {
                    string richTextBoxName = rtb.Name;
                    switch (richTextBoxName)
                    {
                        case "rightARichTextBox":
                            Paragraph paragraph1 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if(paragraph1 != null)
                            {
                                foreach (Inline i in paragraph1.Inlines)
                                {
                                    dataClassSaving.rightAInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.rightAInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.rightAInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.rightAInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "rightStaticARichTextBox":
                            Paragraph paragraph2 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph2 != null)
                            {
                                foreach (Inline i in paragraph2.Inlines)
                                {
                                    dataClassSaving.rightStaticAInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.rightStaticAInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.rightStaticAInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.rightStaticAInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "rightConstrRichTextBox":
                            Paragraph paragraph3 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph3 != null)
                            {
                                foreach (Inline i in paragraph3.Inlines)
                                {
                                    dataClassSaving.rightConstrInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.rightConstrInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.rightConstrInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.rightConstrInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "rightMethRichTextBox":
                            Paragraph paragraph4 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph4 != null)
                            {
                                foreach (Inline i in paragraph4.Inlines)
                                {
                                    dataClassSaving.rightMethInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.rightMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.rightMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.rightMethInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "rightStaticMethRichTextBox":
                            Paragraph paragraph5 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph5 != null)
                            {
                                foreach (Inline i in paragraph5.Inlines)
                                {
                                    dataClassSaving.rightStaticMethInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.rightStaticMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.rightStaticMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.rightStaticMethInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "rightMessageRichTextBox":
                            Paragraph paragraph6 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph6 != null)
                            {
                                foreach (Inline i in paragraph6.Inlines)
                                {
                                    dataClassSaving.rightMessageInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.rightMessageInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.rightMessageInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.rightMessageInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "leftARichTextBox":
                            Paragraph paragraph7 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph7 != null)
                            {
                                foreach (Inline i in paragraph7.Inlines)
                                {
                                    dataClassSaving.leftAInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.leftAInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.leftAInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.leftAInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "leftMethRichTextBox":
                            Paragraph paragraph8 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph8 != null)
                            {
                                foreach (Inline i in paragraph8.Inlines)
                                {
                                    dataClassSaving.leftMethInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.leftMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.leftMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.leftMethInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                        case "leftStaticMethRichTextBox":
                            Paragraph paragraph9 = rtb.Document.Blocks.FirstBlock as Paragraph; // Получение всех Inline-ов (они храняться в Paragraph-е) выделенного RichTextBox-а
                            if (paragraph9 != null)
                            {
                                foreach (Inline i in paragraph9.Inlines)
                                {
                                    dataClassSaving.leftStaticMethInlineTextFontWeight.Add((i.FontWeight.ToString() == "Bold")); // Запись типа текста (жирный/не жирный) в Inline-е
                                    dataClassSaving.leftStaticMethInlineText.Add(new TextRange(i.ContentStart, i.ContentEnd).Text); // Запись самого текста Inline-а
                                    dataClassSaving.leftStaticMethInlineTextFontSize.Add((int)i.FontSize); // Запись размера шрифта текста в Inline-е
                                    dataClassSaving.leftStaticMethInlineTextColor.Add((i.Foreground as SolidColorBrush).Color); // Запись размера шрифта текста в Inline-е
                                }
                            }
                            break;
                    }
                }
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                jsonSerializer.WriteObject(fs, dataClassSaving);
                nameOfParent = null;
            }
        }

        // Получение фокуса дочерними элементами
        private void ChildrenGotFocus(object sender, RoutedEventArgs e) // При выделении classNameTextBox
        {
            currentClassName = ((sender as FrameworkElement).Parent as Grid).Name; // в эту переменную записывается текущее имя выделенного класса
            currentCanvasPointClass = new Point((double)((sender as FrameworkElement).Parent as Grid).GetValue(Canvas.LeftProperty), (double)((sender as FrameworkElement).Parent as Grid).GetValue(Canvas.TopProperty));
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
                if (TakeClassGrid.Name != currentClassName)
                {
                    Grid[] grids = bigCanvas.Children.OfType<Grid>().ToArray();
                    foreach (Grid g in grids)
                    {
                        if (g.Name == currentClassName)
                        {
                            Border[] borders = g.Children.OfType<Border>().ToArray();
                            foreach (Border b in borders) { if (b.Name == "OwterGlow") { g.Children.Remove(b); } }
                        }
                    }
                }
                else
                {
                    Border[] borders = TakeClassGrid.Children.OfType<Border>().ToArray();
                    foreach (Border b in borders) { if (b.Name == "OwterGlow") { TakeClassGrid.Children.Remove(b); } }
                }
                TakeClassGrid.Children.Add(new Border { Name = "OwterGlow", Width = 400, Height = 109, VerticalAlignment = VerticalAlignment.Center, BorderBrush = new SolidColorBrush(Color.FromRgb(255, 150, 0)), BorderThickness = new Thickness(3) });
                currentClassName = TakeClassGrid.Name;
                GetMouseClass = true;
                Mouse.Capture(TakeClassGrid);
                mousePosClassCanvasOld = e.GetPosition(bigCanvas);
            }
        }
        private void GridMouseMove(object sender, MouseEventArgs e) // Само перемещение классов по холсту
        {
            if (GetMouseClass)
            {
                Point mousePosClassCanvasNew = e.GetPosition(bigCanvas);
                Point CanvasElement = new Point((double)TakeClassGrid.GetValue(Canvas.LeftProperty), (double)TakeClassGrid.GetValue(Canvas.TopProperty));
                if (CanvasElement.X < 0) { CanvasElement.X = 0; } else if(CanvasElement.Y < 0) { CanvasElement.Y = 0; }
                if(CanvasElement.X > bigCanvas.Width - 400) { CanvasElement.X = (bigCanvas.Width - 400) - 1; } else if (CanvasElement.Y > bigCanvas.Height - 109) { CanvasElement.Y = (bigCanvas.Height - 109) - 1; }
                Canvas.SetLeft(TakeClassGrid, CanvasElement.X + (mousePosClassCanvasNew.X - mousePosClassCanvasOld.X));
                Canvas.SetTop(TakeClassGrid, CanvasElement.Y + (mousePosClassCanvasNew.Y - mousePosClassCanvasOld.Y));
                mousePosClassCanvasOld.X = mousePosClassCanvasNew.X;
                mousePosClassCanvasOld.Y = mousePosClassCanvasNew.Y;

                DataClass ExampleClass = new DataClass();
                string[] allClasses = Directory.GetFiles("D:\\Unity Class Tree\\Classes");
                foreach (string s in allClasses)
                {
                    if (s == "D:\\Unity Class Tree\\Classes\\" + TakeClassGrid.Name + ".json") // Находится файл с именем перемещаемого класса
                    {
                        using (FileStream fs = new FileStream(s, FileMode.Open))
                        {
                            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                            try
                            {
                                ExampleClass = (DataClass)jsonSerializer.ReadObject(fs); // Загружается файл перемещаемого класса в переменную movingClass
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Проблемы с файлом: " + s);
                            }
                        }
                    }
                }
                if (ExampleClass.parentClassNameForThis != null) // Если у перемещаемого класса в его свойстве parentClassNameForThis указан родитель
                {
                    foreach (string s in allClasses) // Тогда найти этого родителя, а у него найти его координаты
                    {
                        if (s == "D:\\Unity Class Tree\\Classes\\" + ExampleClass.parentClassNameForThis + ".json")
                        {
                            using (FileStream fs = new FileStream(s, FileMode.Open))
                            {
                                try
                                {
                                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                                    ExampleClass = (DataClass)jsonSerializer.ReadObject(fs);
                                    TakeClassGrid.Tag = ExampleClass.className;
                                    Point ParentPoint = ExampleClass.canvasPointClass;
                                    Point ChildPoint = new Point((double)TakeClassGrid.GetValue(Canvas.LeftProperty), (double)TakeClassGrid.GetValue(Canvas.TopProperty));                                  
                                    Line[] lines = bigCanvas.Children.OfType<Line>().ToArray();
                                    foreach (Line l in lines) // Постоянно удалять старые линии
                                    {
                                        if (l.Name == TakeClassGrid.Name)
                                        {
                                            bigCanvas.Children.Remove(l);
                                        }
                                    }
                                    // А создавать новые (новую, по текущему положение)
                                    Line line = new Line { Name = TakeClassGrid.Name, X1 = ParentPoint.X + 200, Y1 = ParentPoint.Y + 109, X2 = ChildPoint.X + 200, Y2 = ChildPoint.Y, Fill = new SolidColorBrush(Colors.Yellow), Stroke = new SolidColorBrush(Color.FromRgb(253, 221, 4)), StrokeThickness = 4 };
                                    bigCanvas.Children.Add(line);
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Проблемы с файлом: " + s);
                                }
                            }
                        }
                    }
                }
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
                LostFocusClassSaving(TakeClassGrid, e);
            }
        }

        // Реализация перемещения мышкой холста (canvas)
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) // Если левая кнопка мыши нажата вниз на холсте (canvas)
        {
            if (e.OriginalSource.GetType().ToString() == "System.Windows.Controls.Canvas") // Код ниже сработает, если событие было вызвано по нажатию на холст (canvas)
            {
                if (TakeClassGrid != null) // Здесь происходит удаление подсветки выбранного класса
                {
                    Border[] borders = TakeClassGrid.Children.OfType<Border>().ToArray();
                    foreach (Border b in borders) { if (b.Name == "OwterGlow") { TakeClassGrid.Children.Remove(b); } }
                }
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

        // Выбор первого класса для установки связи
        private void AddFirstClassButtonClick(object sender, RoutedEventArgs e)
        {
            if(TakeClassGrid != null)
            {
                if(SecondClassGrid != TakeClassGrid)
                {
                    FirstClassGrid = TakeClassGrid;
                    FirstClassGrid.Tag = "FirstClassGrid";
                    FirstClassGridTextBox.Text = FirstClassGrid.Name;
                    TakeClassGrid = null;
                }
            }
            else { MessageBox.Show("Этот класс уже выбран."); }
        }
        // Выбор второго класса для установки связи
        private void AddSecondClassButtonClick(object sender, RoutedEventArgs e)
        {
            if (TakeClassGrid != null)
            {
                if(FirstClassGrid != TakeClassGrid)
                {
                    SecondClassGrid = TakeClassGrid;
                    SecondClassGrid.Tag = "SecondClassGrid";
                    SecondClassGridTextBox.Text = SecondClassGrid.Name;
                    TakeClassGrid = null;
                }
            }
            else { MessageBox.Show("Этот класс уже выбран."); }
        }
        // Установка связи между двумя выбранными классами, а также вызов метода сохранения свойств для каждого из этих двух классов
        private void ClassConnectButtonClick(object sender, RoutedEventArgs e)
        {
            if(FirstClassGrid != null && SecondClassGrid != null)
            {
                Point FirstClassGridPoint = new Point((double)FirstClassGrid.GetValue(Canvas.LeftProperty), (double)FirstClassGrid.GetValue(Canvas.TopProperty));
                Point SecondClassGridPoint = new Point((double)SecondClassGrid.GetValue(Canvas.LeftProperty), (double)SecondClassGrid.GetValue(Canvas.TopProperty));
                Line line = new Line { Name = SecondClassGrid.Name, X1 = FirstClassGridPoint.X + 200, Y1 = FirstClassGridPoint.Y + 109, X2 = SecondClassGridPoint.X + 200, Y2 = SecondClassGridPoint.Y, Fill = new SolidColorBrush(Colors.Yellow), Stroke = new SolidColorBrush(Color.FromRgb(253, 221, 4)), StrokeThickness = 4 };
                bigCanvas.Children.Add(line);
                LostFocusClassSaving(SecondClassGrid, e);
                FirstClassGrid = null;
                SecondClassGrid = null;
                TakeClassGrid = null;
                FirstClassGridTextBox.Text = "First Class Name";
                SecondClassGridTextBox.Text = "Second Class Name";
            }
        }
        // Удаление связи между выбранными классами, а так же установка по дефолту значений для FirstClassGridTextBox.Text и SecondClassGridTextBox.Text
        private void ClassDeleteConnectButtonClick(object sender, RoutedEventArgs e)
        {
            if(SecondClassGrid != null)
            {
                DataClass deleteConnectionParent = new DataClass();
                using (FileStream fs = new FileStream("D:\\Unity Class Tree\\Classes\\" + SecondClassGrid.Name + ".json", FileMode.Open))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DataClass));
                    deleteConnectionParent = (DataClass)jsonSerializer.ReadObject(fs); // Загружается файл дочернего класса, чтобы удалить наследование
                    deleteConnectionParent.parentClassNameForThis = null;
                }
                using (FileStream fs2 = new FileStream("D:\\Unity Class Tree\\Classes\\" + SecondClassGrid.Name + ".json", FileMode.Create))
                {
                    DataContractJsonSerializer jsonSerializer2 = new DataContractJsonSerializer(typeof(DataClass));
                    jsonSerializer2.WriteObject(fs2, deleteConnectionParent);
                    int a = 0;
                    Line[] lines = bigCanvas.Children.OfType<Line>().ToArray();
                    foreach (Line l in lines)
                    {
                        if (l.Name == deleteConnectionParent.className) { bigCanvas.Children.Remove(lines[a]); }
                        a++;
                    }
                }
            }
            FirstClassGridTextBox.Text = "First Class Name";
            SecondClassGridTextBox.Text = "Second Class Name";
            FirstClassGrid = null;
            SecondClassGrid = null;
        }

        // Команда открытия littleCanvas
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(bigCanvas.Visibility == Visibility.Visible) // Если bigCanvas открыт, то закроет его, а на его месте откроет создаст и откроет littleCanvas, на который загрузи в миниатюре все что было в bigCanvas (в том числе и положение скрола (ScrollViewer1))
            {
                Canvas littleCanvas = new Canvas { Name = "littleCanvas", Background = new SolidColorBrush(Color.FromRgb(178, 178, 178)), Width = bigCanvas.Width, Height = bigCanvas.Height};
                littleCanvas.Visibility = Visibility.Visible;
                bigCanvas.Visibility = Visibility.Collapsed;
                Grid[] grids = bigCanvas.Children.OfType<Grid>().ToArray();
                Line[] lines = bigCanvas.Children.OfType<Line>().ToArray();
                foreach (Grid g in grids)
                {
                    Grid grid = new Grid { Width = g.Width / 3, Height = g.Height / 3, Background = new SolidColorBrush(Color.FromRgb(108, 108, 109)) };
                    Border border = new Border { Width = g.Children.OfType<Border>().First<Border>().Width / 2.5, Height = g.Children.OfType<Border>().First<Border>().Height / 2.5, Background = new SolidColorBrush(Colors.White), CornerRadius = new CornerRadius(6), VerticalAlignment = VerticalAlignment.Center, BorderBrush = new SolidColorBrush(Color.FromRgb(65, 65, 65)), BorderThickness = new Thickness(2), Margin = new Thickness(0, 0, 0, 22 / 2.5) };
                    TextBlock textBlock = new TextBlock { Text = g.Children.OfType<TextBlock>().First<TextBlock>().Text, Width = 296 / 2.5, FontSize = 19 / 2, VerticalAlignment = VerticalAlignment.Center, TextAlignment = TextAlignment.Center, Foreground = new SolidColorBrush(Color.FromRgb(43, 145, 175)), Margin = new Thickness(0, 0, 0, 22 / 2.5) };
                    grid.Children.Add(border);
                    grid.Children.Add(textBlock);
                    littleCanvas.Children.Add(grid);
                    Canvas.SetLeft(grid, new Point((double)g.GetValue(Canvas.LeftProperty), (double)g.GetValue(Canvas.TopProperty)).X / 3);
                    Canvas.SetTop(grid, new Point((double)g.GetValue(Canvas.LeftProperty), (double)g.GetValue(Canvas.TopProperty)).Y / 3);
                }
                foreach (Line l in lines)
                {
                    Line line = new Line { X1 = l.X1 / 3, Y1 = l.Y1 / 3, X2 = l.X2 / 3, Y2 = l.Y2 / 3, Fill = new SolidColorBrush(Colors.Yellow), Stroke = new SolidColorBrush(Color.FromRgb(253, 221, 4)), StrokeThickness = 2 };
                    littleCanvas.Children.Add(line);
                }
                    canvasGrid.Children.Add(littleCanvas);
                ScrollViewer1.ScrollToHorizontalOffset(ScrollViewer1.HorizontalOffset / 3);
                ScrollViewer1.ScrollToVerticalOffset(ScrollViewer1.VerticalOffset / 3);
            } // Если bigCanvas закрыт, тогда откроет его и удалит сам littleCanvas и все что было на нем
            else if(bigCanvas.Visibility == Visibility.Collapsed)
            {
                bigCanvas.Visibility = Visibility.Visible;
                canvasGrid.Children.Clear();
                canvasGrid.Children.Add(bigCanvas);
                ScrollViewer1.ScrollToHorizontalOffset(ScrollViewer1.HorizontalOffset * 3);
                ScrollViewer1.ScrollToVerticalOffset(ScrollViewer1.VerticalOffset * 3);
            }
        }

        private void allClassesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grid[] grids = bigCanvas.Children.OfType<Grid>().ToArray();
            foreach(Grid g in grids)
            {
                if(g.Name == (sender as ListBox).SelectedItem.ToString())
                {
                    Point CanvasElement = new Point((double)g.GetValue(Canvas.LeftProperty), (double)g.GetValue(Canvas.TopProperty));
                    ScrollViewer1.ScrollToHorizontalOffset(CanvasElement.X);
                    ScrollViewer1.ScrollToVerticalOffset(CanvasElement.Y);
                }
            }
        }

        private void ChangebigCanvasSize(object sender, RoutedEventArgs e)
        {
            bigCanvas.Width = Convert.ToDouble(bigCanvasSizeTextBox.Text);
            bigCanvas.Height = Convert.ToDouble(bigCanvasSizeTextBox.Text);
            canvasSize = Convert.ToDouble(bigCanvasSizeTextBox.Text);
            using (FileStream fs = new FileStream("D:\\Unity Class Tree\\Canvas Size.json", FileMode.Create))
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(double));
                jsonSerializer.WriteObject(fs, canvasSize);
            }
        }
    }
}
