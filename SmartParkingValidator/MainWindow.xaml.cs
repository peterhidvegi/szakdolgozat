
using System;
using Validator.src;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace Validator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int currentNumberOfPage;
        private int selectedRow;
        private int selectedCol;
        GetEvents download;
        List<IStoppableThread> stoppables = new List<IStoppableThread>();
        private Window biggerImage = new Window();
       // private List<OneEvent> listOfEvents;
        private List<OneEvent> filteredListOfEvents;
        private Grid currentGrid;
        private List<Grid> currentInnerGrids = new List<Grid>();
        private User user;
        private Border border;

       // List<System.Drawing.Bitmap> compare = new List<System.Drawing.Bitmap>();

        internal List<OneEvent> ListOfEvents
        { get; set; }

        public string Drive
        { get; set; }

        public DateTime Date
        { get; set; }

        internal List<IStoppableThread> Stoppables
        {
            get
            {
                return stoppables;
            }
        }

        public MainWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            Title += user.Name;
            findDrive();
            biggerImage.MouseLeftButtonDown += BiggerImage_MouseLeftButtonDown;
            datePicker.ClearValue(DatePicker.SelectedDateProperty);
            datePicker.SelectedDate = DateTime.Today;
            biggerImage.WindowStyle = WindowStyle.None;
            biggerImage.Height = 200;
            biggerImage.Width = 1280;          
            this.IsVisibleChanged += MainWindow_IsVisibleChanged;
            this.MinHeight = 768;
            this.MinWidth = 1366;
            motionCheckBox.IsEnabled = true;
        }

        // Start the process: (download button)
        private void button_Click(object sender, RoutedEventArgs e)
        {
            StopMotionThreads();
            ListOfEvents = null;

            if (datePicker.SelectedDate.HasValue)
            {
                
                resetUIAndOthers();
                download = new GetEvents(this);
                Stoppables.Add(download);
            }
            else
            {
                MessageBox.Show("Select date first.", "Date is empty!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public void StopMotionThreads()
        {
            if (Stoppables.Count > 0)
                foreach (IStoppableThread stoppable in stoppables)
                {
                    stoppable.StopBackgroundWorker();
                }

            Stoppables.Clear();
        }

        private void findDrive()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {   
                bool isLocal = WNetGetConnectionClass.IsLocalDrive(drive.Name);
                if (!isLocal)
                {
                    drivesBox.Items.Add(drive.Name);
                }
            }
        }

        // PopUpWindow
        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!((Window)sender).IsVisible)
            {
                biggerImage.Hide();
            }
        }

        // PopUpSet
        private void BiggerImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Window)sender).DragMove();
        }

        // Reset, prepare to work
        private void resetUIAndOthers()
        {
            resetUI(true);
            ListOfEvents = new List<OneEvent>();
            filteredListOfEvents = new List<OneEvent>();
            Date = datePicker.SelectedDate.Value;
            sensorComboBox.Items.Clear();
            sensorComboBox.SelectedValue = null;
            maxPage.Content = 0;
            CBFrom.Items.Clear();
            CBTo.Items.Clear();
            if (drivesBox.Items.Count > 0)
                Drive = drivesBox.SelectedValue.ToString();
        }

        //  Navigate by arrows
        // Bottom Button
        private void Polygon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Down();
        }
        // Up Button
        private void Polygon_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Up();
        }

        // Get Three events in one row
        private List<OneEvent> getThreeEvent(int getPageNumber, List<OneEvent> list)
        {
            List<OneEvent> three = new List<OneEvent>();
            for (int i = 0; i < 3; i++)
            {
                if (list.Count > getPageNumber + i)
                {
                    if ((list[getPageNumber + i] != null))
                        three.Add(list[getPageNumber + i]);
                }
            }
            return three;
        }

        // Create New UI by number of current id
        private void getNewUI(bool resetPages, List<OneEvent> list)
        {
            resetUI(resetPages);
            int getPageNumber = currentNumberOfPage - 1;
            currentGrid = GetUI(getThreeEvent(getPageNumber, list));
            currentGrid.PreviewMouseDown += CurrentGrid_PreviewMouseDown;
            this.container.Children.Add(currentGrid);
            this.numberOfPage.Content = currentNumberOfPage;

            if(border!=null)
                viewSelectedImg();
        }

        private void viewSelectedImg()
        {
            currentGrid.Children.Add(border);
            var element = currentGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == selectedRow && Grid.GetColumn(e) == selectedCol);
            try { getImageToWindow((Image)element); } catch (Exception) { }
        }

        //set border to sign field
        private void CurrentGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    var point = Mouse.GetPosition(currentGrid);
                    int row = 0;
                    int col = 0;
                    double accumulatedHeight = 0.0;
                    double accumulatedWidth = 0.0;

                    foreach (var rowDefinition in currentGrid.RowDefinitions)
                    {
                        accumulatedHeight += rowDefinition.ActualHeight;
                        if (accumulatedHeight >= point.Y)
                            break;
                        row++;
                    }

                    foreach (var columnDefinition in currentGrid.ColumnDefinitions)
                    {
                        accumulatedWidth += columnDefinition.ActualWidth;
                        if (accumulatedWidth >= point.X)
                            break;
                        col++;
                    }

                    if (col > 2 && col < 6)
                    {
                        currentGrid.Children.Remove(border);
                        selectedCol = col;
                        selectedRow = row;
                        border = new Border();
                        border.SetValue(Grid.ColumnProperty, selectedCol);
                        border.SetValue(Grid.RowProperty, selectedRow);
                        border.BorderBrush = Brushes.Red;
                        border.BorderThickness = new Thickness(2,4,2,4);
                        viewSelectedImg();
                    }


                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    currentGrid.Children.Remove(border);
                    border = null;
                }
            }
        }

        // Reset before get a new one.
        private void resetUI(bool resetPages)
        {
            if (resetPages)
            {
                currentNumberOfPage = 1;
                this.numberOfPage.Content = currentNumberOfPage;
            }

            foreach (Grid item in currentInnerGrids)
            {
                item.Children.Clear();
            }

            currentInnerGrids.Clear();

            if (currentGrid != null)
            {
                currentGrid.Children.Clear();
                container.Children.Clear();
            }
        }

        //set Grid

        private void setParamsGrid(Grid grid)
        {
            // set container UI
            grid.Margin = new Thickness(2);
            ColumnDefinition idCol = new ColumnDefinition();
            idCol.Width = new System.Windows.GridLength(2, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(idCol);
            ColumnDefinition timeCol = new ColumnDefinition();
            timeCol.Width = new System.Windows.GridLength(5, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(timeCol);
            ColumnDefinition picLeftArrow = new ColumnDefinition();
            picLeftArrow.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(picLeftArrow);
            ColumnDefinition pic1Col = new ColumnDefinition();
            pic1Col.Width = new System.Windows.GridLength(12, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(pic1Col);
            ColumnDefinition pic2Col = new ColumnDefinition();
            pic2Col.Width = new System.Windows.GridLength(12, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(pic2Col);
            ColumnDefinition pic3Col = new ColumnDefinition();
            pic3Col.Width = new System.Windows.GridLength(12, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(pic3Col);
            ColumnDefinition picRightArrow = new ColumnDefinition();
            picRightArrow.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(picRightArrow);
            ColumnDefinition checkCol = new ColumnDefinition();
            checkCol.Width = new System.Windows.GridLength(4, System.Windows.GridUnitType.Star);
            grid.ColumnDefinitions.Add(checkCol);


            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

        }

        private void setIdGrid(Grid grid,List<OneEvent> currentEvents)
        {
            for (int i = 0; i < currentEvents.Count; i++)
            {
                if (currentEvents[i] != null)
                {
                    Label titleLabel = new Label();
                    titleLabel.Content = currentEvents[i].EventId;
                    if(currentEvents[i].eventType == Types.EventType.motion)
                    {
                        titleLabel.Background = Brushes.CornflowerBlue;
                        titleLabel.Foreground = Brushes.White;
                    } else if(currentEvents[i].eventType == Types.EventType.ticketStart)
                    {
                        titleLabel.Background = Brushes.Green;
                        titleLabel.Foreground = Brushes.White;
                    }  else if(currentEvents[i].eventType == Types.EventType.ticketEnd)
                    {
                        titleLabel.Background = Brushes.Red;
                        titleLabel.Foreground = Brushes.White;
                    }
                    titleLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    titleLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    Grid.SetColumn(titleLabel, 0);
                    Grid.SetRow(titleLabel, i);
                    grid.Children.Add(titleLabel);
                }
            }
        }

        private void setTimeGrid(Grid grid, List<OneEvent> currentEvents)
        {
            for (int i = 0; i < currentEvents.Count; i++)
            {
                if (currentEvents[i] != null)
                {
                    Label timeLabel = new Label();
                    timeLabel.Content = currentEvents[i].DateTime.ToString("yyyy/MM/dd-HH:mm ");
                    timeLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    timeLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    Grid.SetColumn(timeLabel, 1);
                    Grid.SetRow(timeLabel, i);
                    grid.Children.Add(timeLabel);
                }
            }
        }

        private void setImagesGrid(Grid grid,List<OneEvent> currentEvents)
        {
            //Images 
            for (int y = 0; y < currentEvents.Count; y++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (currentEvents[y] != null)
                    {
                        Image myImage = currentEvents[y].getImageFromSavedFile(currentEvents[y].FirstDisplayedImg + i);
                        //myTestWorker.RunWorkerAsync(currentEvents[y]);

                        myImage.MouseEnter += MyImage_MouseEnter;
                        Grid.SetColumn(myImage, 3 + i);
                        Grid.SetRow(myImage, y);
                        grid.Children.Add(myImage);
                        currentEvents[y].RowWeNeed = y;
                    }
                }

                //triangleToNavigatesInPictures
                for (int k = 0; k < 2; k++)
                {

                    if (k == 0)
                    {
                        currentEvents[y].PolygonArray[k].Points = new PointCollection() { new Point(10, 0), new Point(0, 20), new Point(10, 40) };
                        Grid.SetColumn(currentEvents[y].PolygonArray[k], 2);

                        // IsSubs already? <
                        if (!currentEvents[y].SubsEvents[0])
                        {
                            currentEvents[y].PolygonArray[k].MouseUp += triangleClick;
                            currentEvents[y].PolygonArray[k].MouseDown += triangleClickColor;
                            currentEvents[y].SubsEvents[0] = true;
                        }
                    }
                    else
                    {
                        Grid.SetColumn(currentEvents[y].PolygonArray[k], 6);
                        currentEvents[y].PolygonArray[k].Points = new PointCollection() { new Point(10, 0), new Point(20, 20), new Point(10, 40) };

                        // IsSubs already? >
                        if (!currentEvents[y].SubsEvents[1])
                        {
                            currentEvents[y].PolygonArray[k].MouseUp += triangleClick;
                            currentEvents[y].PolygonArray[k].MouseDown += triangleClickColor;
                            currentEvents[y].SubsEvents[1] = true;
                        }
                    }

                    Grid.SetRow(currentEvents[y].PolygonArray[k], y);
                    grid.Children.Add(currentEvents[y].PolygonArray[k]);


                }
            }
        }


        private void setInnerGrid(Grid grid, List<OneEvent> currentEvents)
        {
            //inner grid...
            for (int i = 0; i < currentEvents.Count; i++)
            {
                if (currentEvents[i] != null)
                {
                    Grid innerGrid = new Grid();
                    currentInnerGrids.Add(innerGrid);

                    innerGrid.Margin = new Thickness(1);
                    RowDefinition newFirstRow = new RowDefinition();
                    newFirstRow.Height = new GridLength(30);
                    RowDefinition newSecoundRow = new RowDefinition();
                    newSecoundRow.Height = new GridLength(30);
                    ColumnDefinition newFirstCol = new ColumnDefinition();
                    newFirstCol.Width = new GridLength(40);
                    ColumnDefinition newSecoundCol = new ColumnDefinition();
                    newSecoundCol.Width = new GridLength(40);

                    innerGrid.ColumnDefinitions.Add(newFirstCol);
                    innerGrid.ColumnDefinitions.Add(newSecoundCol);
                    innerGrid.RowDefinitions.Add(newFirstRow);
                    innerGrid.RowDefinitions.Add(newSecoundRow);

                    Rectangle rectangleRight = new Rectangle();
                    rectangleRight.Fill = Brushes.Green;
                    Grid.SetColumn(rectangleRight, 1);
                    Grid.SetRow(rectangleRight, 1);
                    innerGrid.Children.Add(rectangleRight);

                    Rectangle rectangleLeft = new Rectangle();
                    rectangleLeft.Fill = Brushes.Red;
                    Grid.SetColumn(rectangleLeft, 0);
                    Grid.SetRow(rectangleLeft, 1);
                    innerGrid.Children.Add(rectangleLeft);

                    Rectangle rectangle = new Rectangle();
                    if (currentEvents[i].LastStatus == 50)
                    {
                        rectangle.Fill = Brushes.Green;
                    }
                    else
                    {
                        rectangle.Fill = Brushes.Red;
                    }
                    Grid.SetRow(rectangle, 0);
                    Grid.SetColumnSpan(rectangle, 2);
                    innerGrid.Children.Add(rectangle);

                    CheckBox cbReserved = currentEvents[i].CheckBoxIllegal;
                    if (!currentEvents[i].SubsEvents[2])
                    {
                        cbReserved.Click += CbIllegal_Checked;
                        currentEvents[i].SubsEvents[2] = true;
                    }
                    cbReserved.VerticalAlignment = VerticalAlignment.Center;
                    cbReserved.HorizontalAlignment = HorizontalAlignment.Center;
                    ScaleTransform scale = new ScaleTransform(2.0, 2.0);
                    cbReserved.RenderTransformOrigin = new Point(0.5, 0.5);
                    cbReserved.RenderTransform = scale;
                    Grid.SetRow(cbReserved, 1);
                    Grid.SetColumn(cbReserved, 0);
                    innerGrid.Children.Add(cbReserved);

                    CheckBox cbFree = currentEvents[i].CheckBoxLegal;
                    if (!currentEvents[i].SubsEvents[3])
                    {
                        cbFree.Click += CbLegal_Checked;
                        currentEvents[i].SubsEvents[3] = true;
                    }

                    cbFree.VerticalAlignment = VerticalAlignment.Center;
                    cbFree.HorizontalAlignment = HorizontalAlignment.Center;
                    cbFree.RenderTransformOrigin = new Point(0.5, 0.5);
                    cbFree.RenderTransform = scale;
                    Grid.SetRow(cbFree, 1);
                    Grid.SetColumn(cbFree, 1);
                    innerGrid.Children.Add(cbFree);


                    innerGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    innerGrid.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    Grid.SetColumn(innerGrid, 7);
                    Grid.SetRow(innerGrid, i);
                    grid.Children.Add(innerGrid);
                }
            }
        }


        // UICreator
        private Grid GetUI(List<OneEvent> currentEvents)
        {
            Grid grid = new Grid();

            setParamsGrid(grid);

            setIdGrid(grid, currentEvents);

            setTimeGrid(grid, currentEvents);

            setImagesGrid(grid, currentEvents);

            setInnerGrid(grid, currentEvents);

            return grid;
        }

        //  CheckBox controllers
        // Red checkbox
        private void CbIllegal_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ListOfEvents)
            {
                if (item.CheckBoxIllegal.Equals(sender))
                {

                    if (((CheckBox)sender).IsChecked.Value)
                    {
                        item.CheckBoxIllegal.IsChecked = true;

                        int idToCompare = item.EventId;
                        int index = ListOfEvents.IndexOf(item) + 1;
                        
                        if (ListOfEvents.Count > index)
                        {
                            OneEvent nextEvent = ListOfEvents[index];
                            if (nextEvent.NodeId == item.NodeId)
                            {
                                new DbHandler(Types.Status.illegal, user.Id, item.EventId, nextEvent.EventId, item.NodeId);
                            }
                            else
                            {
                                new DbHandler(Types.Status.illegal, user.Id, item.EventId, item.NodeId, item.DateTime);
                            }
                        }
                        else
                        {
                            new DbHandler(Types.Status.illegal, user.Id, item.EventId, item.NodeId, item.DateTime);
                        }
                        exchangeCheckBoxRes(item);
                    }
                }
            }
        }

        private void exchangeCheckBoxRes(OneEvent item)
        {
            foreach (var innerItem in currentGrid.Children)
            {
                if (innerItem is Grid)
                {
                    foreach (var itemCheckBox in (innerItem as Grid).Children)
                    {

                        if (item.CheckBoxLegal.Equals(itemCheckBox))
                        {
                            item.CheckBoxLegal.IsChecked = false;
                            (itemCheckBox as CheckBox).IsChecked = false;

                        }
                    }
                }
            }
        }

        //Green checkbox
        private void CbLegal_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in ListOfEvents)
            {

                 if (item.CheckBoxLegal.Equals(sender))
                 {

                    if (((CheckBox)sender).IsChecked.Value)
                    {
                        item.CheckBoxLegal.IsChecked = true;
                        int index = ListOfEvents.IndexOf(item) + 1;

                        if (ListOfEvents.Count > index)
                        {
                            OneEvent nextEvent = ListOfEvents[index];
                            if (nextEvent.NodeId == item.NodeId)
                            {
                                new DbHandler(Types.Status.legal, user.Id, item.EventId, nextEvent.EventId, item.NodeId);
                            }
                            else
                            {
                                new DbHandler(Types.Status.legal, user.Id, item.EventId, item.NodeId, item.DateTime);
                            }
                        }
                        else
                        {
                            new DbHandler(Types.Status.legal, user.Id, item.EventId, item.NodeId, item.DateTime);
                        }
                        exchangeCheckBoxFree(item);

                    } 
               }
           } //end of foreach
        }

        private void exchangeCheckBoxFree(OneEvent item)
        {
            foreach (var innerItem in currentGrid.Children)
            {
                if (innerItem is Grid)
                {
                    foreach (var itemCheckBox in (innerItem as Grid).Children)
                    {

                        if (item.CheckBoxIllegal.Equals(itemCheckBox))
                        {
                            item.CheckBoxIllegal.IsChecked = false;
                            (itemCheckBox as CheckBox).IsChecked = false;

                        }
                    }
                }
            }
        }

        // Triangle (Arrow) color event
        private void triangleClickColor(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in ListOfEvents)
            {
                for (int i = 0; i < item.PolygonArray.Length; i++)
                {
                    if (item.PolygonArray[i].Equals(sender))
                    {
                        item.PolygonArray[i].Fill = Brushes.DarkBlue;
                    }
                }
            }
        }

        // Trienle, Rotate images (motion)
        private void triangleClick(object sender, MouseButtonEventArgs e)
        {
            List<OneEvent> triangleOneEventList = null;
            if (filteredListOfEvents.Count > 0)
            {
                triangleOneEventList = filteredListOfEvents;
            }else
            {
                triangleOneEventList = ListOfEvents;
            }
          
            foreach (var item in triangleOneEventList)
            {
             
                for (int i = 0; i < item.PolygonArray.Length; i++)
                {
                    if (item.PolygonArray[i].Equals(sender))
                    {

                        item.PolygonArray[i].Fill = Brushes.CornflowerBlue;
                        //Rotation to left
                        if (i == 0)
                        {
                            
                            int next = item.LastDisplayedImg - 3;

                            if(next < 0)
                            {
                                if(!item.HaveMotions)
                                {
                                    string add = item.getNextMin(true);
                                    if(add!="")
                                    {
                                        item.ImgAddressByTime.Add(add);
                                        item.orderImgsAddressByTime();
                                    }
                               
                                }
                                next = 0;
                            }
                           
                                for (int k = 0; k < 3; k++)
                                {
                                   
                                        Image getNextImage = item.getImageFromSavedFile(next);
                                        getNextImage.MouseEnter += MyImage_MouseEnter;
                                        Grid.SetColumn(getNextImage, 3 + k);
                                        Grid.SetRow(getNextImage, item.RowWeNeed);
                                        currentGrid.Children.Add(getNextImage);
                                        next++;
                                }
                            
                        }
                        else //Rotation to Right
                        {

                            int next = item.LastDisplayedImg + 1;
                            if (next == item.ImgAddressByTime.Count)
                            {
                                if (!item.HaveMotions)
                                {
                                    string add = item.getNextMin(false);
                                    if (add != "")
                                    {
                                        item.ImgAddressByTime.Add(add);
                                        item.orderImgsAddressByTime();
                                    }
                                }
                            }

                            if (next < item.ImgAddressByTime.Count)
                            {
                                next = next - 2;
                                for (int k = 0; k < 3; k++)
                                {
                                    Image getNextImage = item.getImageFromSavedFile(next);
                                    getNextImage.MouseEnter += MyImage_MouseEnter;
                                    Grid.SetColumn(getNextImage, 3 + k);
                                    Grid.SetRow(getNextImage, item.RowWeNeed);
                                    currentGrid.Children.Add(getNextImage);
                                    next++;
                                }

                            }
                        }
                   }
               }

            }

        }

        // check images to validate (get another window to see them)

        // Show
        private void MyImage_MouseEnter(object sender, MouseEventArgs e)
        {

            if(!biggerImage.IsVisible)
                biggerImage.Show();

            if (border == null)            
                getImageToWindow((Image)sender);

            if (!biggerImage.IsFocused)
                biggerImage.Focus();
        }

        private void getImageToWindow(Image img)
        {
            Image lookThisPic = new Image();
            Image oldPic = img;
            lookThisPic.Source = new DrawingImage(MergeWithTransparentMask(Converter.bitmapCropper(oldPic)));
            biggerImage.Content = lookThisPic;
        }

        // Get mask to images
        private DrawingGroup MergeWithTransparentMask(System.Drawing.Bitmap main)
        {
            var group = new DrawingGroup();
            string source;
            group.Children.Add(new ImageDrawing(Converter.ToBitmapImage(main), new Rect(0, 0, 1280, 200)));
            
            if (sensorComboBox.SelectedValue != null)
            {
                source = App.mainPath + App.masksDirName + sensorComboBox.SelectedValue + ".png";
                group.Children.Add(new ImageDrawing(new BitmapImage(new Uri(source)), new Rect(0, 0, 1280, 200)));
            }

            return group;
        }
        // End of (UI)

        // Filter events by node_id
        private void sensorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CBFrom.Items.Clear();
            CBTo.Items.Clear();

            filteredListOfEvents = new List<OneEvent>();
            if (ListOfEvents != null)
            {
                foreach (OneEvent eventItem in ListOfEvents)
                {
                    if (eventItem.NodeId == (int)sensorComboBox.SelectedValue)
                    {
                        filteredListOfEvents.Add(eventItem);
                        CBFrom.Items.Add(eventItem.EventId);
                        CBTo.Items.Add(eventItem.EventId);
                    }
                }
                
                getNewUI(true, filteredListOfEvents);
            }
            maxPage.Content = filteredListOfEvents.Count;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        { 
                if (e.Delta > 0)
                {
                    Up();
                }
                else
                {
                    Down();
                }
        }

        private void Up()
        {
            int tryToPage = currentNumberOfPage;
            string text = this.numberOfPage.Content.ToString();
            int.TryParse(text, out tryToPage);
            if (tryToPage > 1)
            {
                currentNumberOfPage = tryToPage;
                currentNumberOfPage--;
                if (sensorComboBox.SelectedValue != null)
                {
                    getNewUI(false, filteredListOfEvents);
                }
                else
                {
                    getNewUI(false, ListOfEvents);
                }
            }
       }

        private void Down()
        {
            int tryToPage = currentNumberOfPage;
            string text = this.numberOfPage.Content.ToString();
            int.TryParse(text, out tryToPage);
            if (ListOfEvents != null)
                if((filteredListOfEvents.Count>0 && (tryToPage < filteredListOfEvents.Count)) || (filteredListOfEvents.Count==0 && (tryToPage < ListOfEvents.Count)))
                {
                        currentNumberOfPage = tryToPage;
                        currentNumberOfPage++;
                        if (sensorComboBox.SelectedValue != null)
                        {
                            getNewUI(false, filteredListOfEvents);
                        }
                        else
                        {
                            getNewUI(false, ListOfEvents);
                        }
                    
               }
               
        }

        private void Button_Click_Interval(object sender, RoutedEventArgs e)
        {
            List<OneEvent> intervalOneEvent = new List<OneEvent>();
            List<OneEvent> selectFromHere;
            if(filteredListOfEvents.Count > 0)
            {
                selectFromHere = filteredListOfEvents;
            }else
            {
                selectFromHere = ListOfEvents;
            }

            int maxValue;
            if (CBTo.SelectedIndex + 1 == CBTo.Items.Count)
            {
                maxValue = int.MaxValue;
            }else
            {
                maxValue = int.Parse(CBTo.Items[CBTo.SelectedIndex + 1].ToString());
            }

            foreach (OneEvent item in selectFromHere)
            { 
                    if (item.EventId >= (int)CBFrom.SelectedValue && item.EventId < maxValue)
                    {
                        intervalOneEvent.Add(item);
                        item.Validation = radioButtonReturner();
                    }
            }

            OneEvent first = intervalOneEvent[0];
            new DbHandler(radioButtonReturner(), user.Id, first.EventId, maxValue, first.NodeId, first.DateTime);
        }

        private string radioButtonReturner()
        {
            if(RBFree.IsChecked == true)
            {
                return Types.Status.legal.ToString();
            }else
            {
                return Types.Status.illegal.ToString();
            }
        }


        private void Focus_Closing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                StopMotionThreads();
        }

        private void setMotion_Click(object sender, RoutedEventArgs e)
        {
            MotionScanSettings motionSettings = new MotionScanSettings();
            motionSettings.Show();
        }
    }

    /* old version
    private DrawingGroup MergeWithTransparentMask(ImageSource main)
    {
        var group = new DrawingGroup();
        string source;
        int moreThanOnce = 0;
        group.Children.Add(new ImageDrawing(main, new Rect(0, 0, 1280, 720)));
        if (sensorComboBox.SelectedValue != null)
        {
            source = App.mainPath + App.masksDirName + sensorComboBox.SelectedValue + ".png";
        }
        else
        {
            string nodeIdtoMask = "0";
            string justNameOfSoureFile = main.ToString().Substring(main.ToString().LastIndexOf('/') + 1);

            foreach (OneEvent item in ListOfEvents)
            {
                foreach (string address in item.ImgAddressByTime)
                {
                    string justAddress = address.Substring(address.LastIndexOf('\\') + 1);
                    if (justAddress.Equals(justNameOfSoureFile))
                    {
                        nodeIdtoMask = item.NodeId.ToString();
                        moreThanOnce++;
                    }
                }
            }

            source = App.mainPath + App.masksDirName + nodeIdtoMask + ".png";
        }

        if (moreThanOnce > 1)
            source = App.mainPath + App.masksDirName + "default" + ".png";

        group.Children.Add(new ImageDrawing(new BitmapImage(new Uri(source)), new Rect(0, 0, 1280, 720)));



        return group;
    }
    */
}
