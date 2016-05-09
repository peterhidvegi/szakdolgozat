using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Validator.src
{
    class GetEvents : IMotionListListener, IStoppableThread
    {
        int numberOfSuccessfulDownloadedFile = 0;
        BackgroundWorker worker = new BackgroundWorker();
        NetworkCredential networkAccess = new NetworkCredential(App.nameFTP, App.passFTP);
        MainWindow mainWindow;
        Senzor[] senzors;
        DataTable dataTable;
        DataTable ticketsTable;
        MySqlCommand quaryCommand;
        MySqlConnection mysqlConn;

       
        public GetEvents(MainWindow mainWindow)
        {
            mysqlConn = App.getEPSMySqlConnection();
            dataTable = new DataTable();
            ticketsTable = new DataTable();
            this.mainWindow = mainWindow;
            worker.DoWork += Worker_DoWork;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainWindow.progressBar.Value = e.ProgressPercentage;
            mainWindow.labelDownload.Content = e.UserState.ToString();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool tryFromFTP = !Directory.Exists(mainWindow.Drive);

            connectToDataBaseAndGetDatas(getQuaryToGetEvents(),false);

            if (tryFromFTP)
            {
                downloadPrepFromFTP();
                App.fromFTP = true;
            }
            else
            {
                App.fromFTP = false;
            }

            getEvents();
            mysqlConn.Close();
            getEventIdsToCBAndRefresh();

            worker.ReportProgress(100, "Ready to work!");
        }


        private List<string> getSelecetedDayImagesInOrder()
        {
            IEnumerable<string> files;

            if (App.fromFTP)
            {
                files = Directory.EnumerateFiles(App.mainPath + "\\snap\\" + mainWindow.Date.Year.ToString() + Converter.ValueConverter(mainWindow.Date.Month) + Converter.ValueConverter(mainWindow.Date.Day) + "\\", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + Types.SaveType.png) || s.EndsWith("." + Types.SaveType.jpg));
            }
            else
            {
                files = Directory.EnumerateFiles(mainWindow.Drive + App.newImgLocal + mainWindow.Date.Year.ToString() + Converter.ValueConverter(mainWindow.Date.Month) + Converter.ValueConverter(mainWindow.Date.Day) + "\\", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + Types.SaveType.png) || s.EndsWith("." + Types.SaveType.jpg));
            }

            return files.OrderBy(o => o).ToList();
        }

        private void downloadPrepFromFTP()
        {
            App.fromFTP = true;

            try
            {
                List<string> snaps = getRelevantFolderNameByDate(ListFiles(App.addressOfFTP), Types.ImgType.snap);
                if (snaps != null)
                    letsDownloadFiles(snaps, Types.ImgType.snap);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "FTP download error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Dispatcher.Invoke(()=>mainWindow.Close());

            }
            
        }

        private List<string> ListFiles(String location)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(location);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = networkAccess;
                request.UsePassive = true;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Download Error");
                return null;
            }
        }

        // This method need to compare "date name"
        private DateTime getDateToCompareFromString(String item)
        {
            int year = int.Parse(item.Substring(0, 4));
            int month = int.Parse(item.Substring(4, 2));
            int day = int.Parse(item.Substring(6, 2));
            DateTime currItemDate = new DateTime(year, month, day);
            return currItemDate;
        }

        // Create (relevant) list (by date) from the list to Download
        private List<string> getRelevantFolderNameByDate(List<string> list, Types.ImgType type)
        {
            if (list != null)
            {
                List<string> relevantList = new List<string>();
                foreach (string item in list)
                {
                    if (getDateToCompareFromString(item) == mainWindow.Date)
                    {
                        if (Types.ImgType.snap == type)
                        {
                            App.fromFTP = true;
                            if (!Directory.Exists(App.mainPath + App.snapDirName + "\\" + item))
                            {
                                relevantList.Add(item);
                            }
                            else
                            {
                                doOverwrite(item, type, relevantList);
                            }
                        }                    
                    }
                }

                return relevantList;
            }
            else
            {
                MessageBox.Show("DownloadList is not available.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
                return null;
            }

        }

        // overwrite?
        private void doOverwrite(string item, Types.ImgType type, List<string> relevantList)
        {
            string message = "Are you sure that you would like to overwrite your " + getDateToCompareFromString(item).ToString("yyyy/MM/dd") + "(date/" + type.ToString() + ") images?";
            const string caption = "Overwrite?";
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                relevantList.Add(item);
            }
        }

        // download images from list
        private string letsDownloadFiles(List<string> namesOfList, Types.ImgType typeToDownload)
        {

            // mainWindow.startOfProcess();
            Application.Current.Dispatcher.Invoke(() => {
                mainWindow.progressBar.Maximum = 100;
                mainWindow.progressBar.Value = 0;
                mainWindow.labelDownload.Content = "Downloading...";
                numberOfSuccessfulDownloadedFile = 0;
            });
            string localPath = App.mainPath;
            string remotePath = App.addressOfFTP;

            localPath += App.snapDirName;

            foreach (var item in namesOfList)
            {
                string currLocalPath = localPath + item;
                if (!Directory.Exists(currLocalPath))
                {
                    Directory.CreateDirectory(currLocalPath);
                }

                List<string> imageFilesOfDate = new List<string>();
                imageFilesOfDate = ListFiles(remotePath + item + "/");

                int maxValue = imageFilesOfDate.Count;
                foreach (string inneritem in imageFilesOfDate)
                {
                    var request = WebRequest.Create(remotePath + "/" + item + "/" + inneritem);
                    request.Credentials = networkAccess;
                    Stream stream = request.GetResponse().GetResponseStream();
                    string fileAddress = localPath + item + "\\" + inneritem;

                    FileStream writeStream = new FileStream(fileAddress, FileMode.Create);
                    int Length = 2048;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = stream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = stream.Read(buffer, 0, Length);
                    }

                    writeStream.Close();
                    stream.Close();

                    numberOfSuccessfulDownloadedFile++;
                    int percents = (numberOfSuccessfulDownloadedFile * 100 / maxValue);
                    worker.ReportProgress(percents, inneritem);

                }

            }

            string result = numberOfSuccessfulDownloadedFile + " images downloaded!";

            Application.Current.Dispatcher.Invoke(() => {
                mainWindow.progressBar.Value = 0;
                mainWindow.labelDownload.Content = result;
            });
            
            return "Completed";
        }


        private void connectToDataBaseAndGetDatas(string quary,bool ticket)
        {
            try
            {
                if (mysqlConn.State == ConnectionState.Closed)
                    mysqlConn.Open();
                quaryCommand = new MySqlCommand(quary, mysqlConn);
                MySqlDataReader queryCommandReader = quaryCommand.ExecuteReader();
                if (ticket)
                    ticketsTable.Load(queryCommandReader);
                else
                    dataTable.Load(queryCommandReader);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "DataBase error!", MessageBoxButton.OK, MessageBoxImage.Error);
                mainWindow.Close();
            }
        }


        // put event_ids to ComboBox
        private void getEventIdsToCBAndRefresh()
        {
            mainWindow.ListOfEvents = mainWindow.ListOfEvents.OrderBy(o => o.DateTime).ToList();
            mainWindow.ListOfEvents = mainWindow.ListOfEvents.OrderBy(o => o.NodeId).ToList();

            Application.Current.Dispatcher.Invoke(() => {
                mainWindow.CBFrom.Items.Clear();
                mainWindow.CBTo.Items.Clear();
                mainWindow.maxPage.Content = mainWindow.ListOfEvents.Count;

                foreach (OneEvent item in mainWindow.ListOfEvents)
                {
                    mainWindow.CBFrom.Items.Add(item.EventId);
                    mainWindow.CBTo.Items.Add(item.EventId);
                }              
            });
        }

        // create quary to get (relevant) events (by date) from database
        private string getQuaryToGetEvents()
        {
            string dateToString = mainWindow.Date.ToString("yyyy-MM-dd");
            string quaryAllName = "SELECT * FROM  " + App.dbName;
            string quaryWhereDate = " WHERE `timestamp` BETWEEN '" + dateToString + " 00:00:00' AND '" + dateToString + " 23:59:59'";
            return quaryAllName + quaryWhereDate;
        }

        //get events from motions
        private void getMotionEvents(List<RealMotion> motions)
        {

            List<OneEvent> motionEvents = new List<OneEvent>();
            if (mysqlConn.State != ConnectionState.Open)
                    mysqlConn.Open();

            foreach (RealMotion realmotion in motions)
            {
                 for (int i = 0; i < realmotion.motionNode.Count; i++)
                 {
                    if((realmotion.avgPercentage > (App.motionMinDiffAvg * realmotion.motionNode.Count) + realmotion.motionNode[i].percentage) || (realmotion.avgPercentage < realmotion.motionNode[i].percentage-(App.motionMaxDiffAvg * realmotion.motionNode.Count)))
                    {
                        saveMotionEvents(realmotion, motions, motionEvents, i);
                    }
                  
                 } 
            }
            
            mysqlConn.Close();
            // add motionEvents to (all of) events 
            motionEvents = motionEvents.OrderBy(o => o.NodeId).ToList();
            foreach (OneEvent oneMotionEvent in motionEvents)
            {
                int i = 0;
                while(i<mainWindow.ListOfEvents.Count && mainWindow.ListOfEvents[i].EventId!=oneMotionEvent.EventId)
                {
                    i++;
                }

                if(i >= mainWindow.ListOfEvents.Count)
                {
                    mainWindow.ListOfEvents.Add(oneMotionEvent);
                }   
            }
      
            Application.Current.Dispatcher.Invoke(() => {
                mainWindow.progressBar.Value = 100;
                mainWindow.labelDownload.Content = "Completed.";
            });
        }
        
        private void saveMotionEvents(RealMotion realmotion, List<RealMotion> motions, List<OneEvent> motionEvents, int index)
        {
            int j = 0;
            while (j < dataTable.Rows.Count && (DateTime.Compare(realmotion.date, ((DateTime)dataTable.Rows[j]["timestamp"])) > 0))
            {
                j++;
            }
     
            if (j < dataTable.Rows.Count)
            {
                string validation = dataTable.Rows[j]["validation"].ToString();
                int eventId = int.Parse(dataTable.Rows[j]["event_id"].ToString());
                int lastStatus = int.Parse(dataTable.Rows[j]["occupancy"].ToString());
                Application.Current.Dispatcher.Invoke(() => {
                    motionEvents.Add(new OneEvent((DateTime)dataTable.Rows[j]["timestamp"], eventId, realmotion.motionNode[index].node, validation, lastStatus, mainWindow.Drive, Types.EventType.motion));
                    mainWindow.progressBar.Value = (motions.IndexOf(realmotion) * 100 / motions.Count);
                    mainWindow.labelDownload.Content = "Please wait for Motion event implementation.";
                });
            }
        }

        private int getNumberOfNodes()
        {
            int maxVal = int.MinValue;
  
            Application.Current.Dispatcher.Invoke(() => { 
                 mainWindow.sensorComboBox.Items.Clear();
                //init
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (maxVal < int.Parse(dataTable.Rows[i]["node_id"].ToString()))
                    {
                        maxVal = int.Parse(dataTable.Rows[i]["node_id"].ToString());

                        mainWindow.sensorComboBox.Items.Add(maxVal);
                    }
                }
            });
            return maxVal;
        }

        private void fillSenzors()
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                worker.ReportProgress((i * 100) / dataTable.Rows.Count, "Search events...");
                int node = int.Parse(dataTable.Rows[i]["node_id"].ToString());
                node--;
                int eventid = int.Parse(dataTable.Rows[i]["event_id"].ToString());

                int occ = int.Parse(dataTable.Rows[i]["occupancy"].ToString());
                DateTime dateTime = (DateTime)dataTable.Rows[i]["timestamp"];
                string validation = dataTable.Rows[i]["validation"].ToString();

                senzors[node].EventId.Add(eventid);
                senzors[node].Occupancy.Add(occ);
                senzors[node].DateTime.Add(dateTime);
                senzors[node].Validation.Add(validation);
            }
        }

        private void saveSenzorsToMyDB()
        {
         
                for (int i = 0; i < senzors.Length; i++)
                {
                    worker.ReportProgress((i * 100) / senzors.Length, "Create events...");

                    Application.Current.Dispatcher.Invoke(() => {    //save first state to validate
                    OneEvent firstState = new OneEvent(senzors[i].DateTime[0], senzors[i].EventId[0], i + 1, senzors[i].Validation[0], senzors[i].Occupancy[0], mainWindow.Drive, Types.EventType.sensor);
                        mainWindow.ListOfEvents.Add(firstState);
                    });
           
                    for (int j = 1; j < senzors[i].Occupancy.Count; j++)
                    {
                        //if change state then save to validate...
                        if (senzors[i].Occupancy[j] != senzors[i].Occupancy[j - 1])
                        {
                            if(senzors[i].Occupancy[j - 1]==0)
                            {
                                int k = j - 1;
                                while (k >= 0 && senzors[i].Occupancy[k] == 0)
                                    k--;

                                if (k >= 0 && senzors[i].Occupancy[j] != senzors[i].Occupancy[k])
                                {
                                    Application.Current.Dispatcher.Invoke(() => {
                                    OneEvent newOnevEvent = new OneEvent(senzors[i].DateTime[j], senzors[i].EventId[j], i + 1, senzors[i].Validation[j], senzors[i].Occupancy[j], mainWindow.Drive,Types.EventType.sensor);
                                        mainWindow.ListOfEvents.Add(newOnevEvent);
                                    });
                                }

                            }else
                            {
                                if (senzors[i].Occupancy[j] != 0)
                                {
                                    Application.Current.Dispatcher.Invoke(() => {
                                    OneEvent newOnevEvent = new OneEvent(senzors[i].DateTime[j], senzors[i].EventId[j], i + 1, senzors[i].Validation[j], senzors[i].Occupancy[j], mainWindow.Drive,Types.EventType.sensor);
                                        mainWindow.ListOfEvents.Add(newOnevEvent);
                                    });
                                }
                            }
                        }
                    }
                }
        }


        // get Events from database
        private void getEvents()
        {
            //init senzors...
            senzors = new Senzor[getNumberOfNodes()];
            for (int i = 0; i < senzors.Length; i++)
            {
                senzors[i] = new Senzor();
            }
            fillSenzors();
            saveSenzorsToMyDB();

            doMotionCheck();

            addTicketEventsToGlobalEvents(getTicketEvents(getListOfTickets()));
            getEventIdsToCBAndRefresh();
        }

        private void addTicketEventsToGlobalEvents(List<OneEvent> ticketEvents)
        {
            foreach (OneEvent ticket in ticketEvents)
            {
                mainWindow.ListOfEvents.Add(ticket);
            }
        }

        private List<OneEvent> getTicketEvents(List<Ticket> tickets)
        {
            List<OneEvent> ticketEvents = new List<OneEvent>();
            foreach (Ticket ticket in tickets)
            {
                int j = 0;
                while (j < dataTable.Rows.Count && (DateTime.Compare(ticket.Start, ((DateTime)dataTable.Rows[j]["timestamp"])) > 0))           
                    j++;
                

                while (j < dataTable.Rows.Count && int.Parse(dataTable.Rows[j]["node_id"].ToString()) < ticket.Sensor_id)
                    j++;
               

                if (j < dataTable.Rows.Count)
                {
                    string validation = dataTable.Rows[j]["validation"].ToString();
                    int eventId = int.Parse(dataTable.Rows[j]["event_id"].ToString());
                    int lastStatus = int.Parse(dataTable.Rows[j]["occupancy"].ToString());
                    Application.Current.Dispatcher.Invoke(() => {
                        ticketEvents.Add(new OneEvent((DateTime)dataTable.Rows[j]["timestamp"], eventId, ticket.Sensor_id, validation, lastStatus, mainWindow.Drive, Types.EventType.ticketStart));
                        mainWindow.progressBar.Value = (tickets.IndexOf(ticket) * 100 / tickets.Count);
                        mainWindow.labelDownload.Content = "Please wait for get Tickets.";
                    });
                }


                // events for end of parking time too
                while (j < dataTable.Rows.Count && (DateTime.Compare(ticket.End, ((DateTime)dataTable.Rows[j]["timestamp"])) > 0))
                    j++;


                while (j < dataTable.Rows.Count && int.Parse(dataTable.Rows[j]["node_id"].ToString()) < ticket.Sensor_id)
                    j++;


                if (j < dataTable.Rows.Count)
                {
                    string validation = dataTable.Rows[j]["validation"].ToString();
                    int eventId = int.Parse(dataTable.Rows[j]["event_id"].ToString());
                    int lastStatus = int.Parse(dataTable.Rows[j]["occupancy"].ToString());
                    Application.Current.Dispatcher.Invoke(() => {
                        ticketEvents.Add(new OneEvent((DateTime)dataTable.Rows[j]["timestamp"], eventId, ticket.Sensor_id, validation, lastStatus, mainWindow.Drive, Types.EventType.ticketEnd));
                        mainWindow.progressBar.Value = (tickets.IndexOf(ticket) * 100 / tickets.Count);
                        mainWindow.labelDownload.Content = "Please wait for get Tickets.";
                    });
                }
            }

            return ticketEvents;
        }

        private List<Ticket> getListOfTickets()
        {
            List<Ticket> tickets = new List<Ticket>();
            connectToDataBaseAndGetDatas(getTicketQuary(),true);
            for (int i = 0; i < ticketsTable.Rows.Count; i++)
            {
                DateTime start = (DateTime)ticketsTable.Rows[i]["start"];
                DateTime end = (DateTime)ticketsTable.Rows[i]["end"];
                int sensor_id = int.Parse(ticketsTable.Rows[i]["sensor_id"].ToString());
                string plate = ticketsTable.Rows[i]["plate"].ToString();
                tickets.Add(new Ticket(sensor_id, start, end, plate));
            }

            return tickets;
        }

        private string getTicketQuary()
        {
            string dateToString = mainWindow.Date.ToString("yyyy-MM-dd");
            string quaryAllName = "SELECT * FROM  " + App.dbTicket;
            string quaryWhereDate = " WHERE `start` BETWEEN '" + dateToString + " 00:00:00' AND '" + dateToString + " 23:59:59'";
            return quaryAllName + quaryWhereDate;
        }

        private void doMotionCheck()
        {
            bool doMotions = false;
            Application.Current.Dispatcher.Invoke(() => {
                mainWindow.motionCheckBox.Checked += MotionCheckBox_Checked;
                doMotions = (mainWindow.motionCheckBox.IsChecked == true) ? true : false;
            });

            if(doMotions)
                doOnceMotionScan();
        }

        private void MotionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            doOnceMotionScan();
        }

        private void doOnceMotionScan()
        {
            mainWindow.Stoppables.Add(new ImageLoader(this, getSelecetedDayImagesInOrder(), mainWindow));
            Application.Current.Dispatcher.Invoke(() => { mainWindow.motionCheckBox.IsEnabled = false; });
        }

        public void GetMotions(List<RealMotion> getEvent)
        {
            getMotionEvents(getEvent);
            getEventIdsToCBAndRefresh();
        }

        public void StopBackgroundWorker()
        {
            worker.CancelAsync();
            worker.Dispose();
        }
    }
}
