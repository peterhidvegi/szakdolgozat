
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Validator.src;

namespace Validator
{
    class OneEvent
    {
   
        string drive;
        string validation;

        public Types.EventType eventType { get; }

        public List<string> ImgAddressByTime
        { get; set; }

        public DateTime DateTime
        { get; set; }

        public Polygon[] PolygonArray
        { get; set; }

        public int LastDisplayedImg
        { get; set; }

        public int RowWeNeed
        { get; set; }

        public int NodeId
        { get; set; }

        public int EventId
        { get; set; }

        public CheckBox CheckBoxIllegal
        { get; set; }

        public CheckBox CheckBoxLegal
        { get; set; }

        public int LastStatus
        { get; set; }

        public bool HaveMotions
        { get; set; }

        public bool[] SubsEvents
        { get; set; }

        public int FirstDisplayedImg
        { get; set; }

        public string Validation
        {
            get
            {
                return validation;
            }

            set
            {
                validation = value;

                if (validation.Equals("legal"))
                {
                    CheckBoxLegal.IsChecked = true;
                    CheckBoxIllegal.IsChecked = false;
                }
                if (validation.Equals("illegal"))
                {
                    CheckBoxLegal.IsChecked = false;
                    CheckBoxIllegal.IsChecked = true;
                }
            }
        }

        public OneEvent(DateTime dateTime, int eventId, int nodeId, string validation, int lastStatus,string drive,Types.EventType eventType)
        {
            ImgAddressByTime = new List<string>();
            HaveMotions = false;
            DateTime = dateTime;
            NodeId = nodeId;
            EventId = eventId;
            FirstDisplayedImg = 0;
            CheckBoxLegal = new CheckBox();
            CheckBoxIllegal = new CheckBox();
            this.eventType = eventType;
            this.drive = drive;
            LastStatus = lastStatus;
            createTriangesToNavigateInPictures();
            getSnaps();
            Validation = validation;
            SubsEvents = new bool[4];
           // MoreMotion();
        }

        private void MoreMotion()
        {
            if (Directory.Exists(drive + "motion\\"))
            {
                if (getMotions() == 0)
                    HaveMotions = false;
            }
            else
            {
                HaveMotions = false;
            }
        }

        public void orderImgsAddressByTime()
        {
        
            int min = DateTime.Minute;
            int hour = DateTime.Hour;

            min--;
            if (min == -1)
            {
                min = 59;
                hour--;
            }

            //javított buborékos rendezés
            int i = ImgAddressByTime.Count-1;
            while(i>=1)
            {
                int idx = 0;
                for (int j = 0; j < i; j++)
                {
                    if(string.Compare(ImgAddressByTime[j],ImgAddressByTime[j+1]) > 0)
                    {
                        Swap(j, j + 1);
                        idx = j;
                    }

                    int prevHourInt = int.Parse(getHourOrMinAtIndex(j, 0));
                    int prevMinInt = int.Parse(getHourOrMinAtIndex(j, 1));

                    if(prevHourInt == hour && prevMinInt == min)
                    {
                        FirstDisplayedImg = j;
                    }
                }
                i = idx;
            }
        }

        private void Swap(int indexA, int indexB)
        {
            string tmp = ImgAddressByTime[indexA];
            ImgAddressByTime[indexA] = ImgAddressByTime[indexB];
            ImgAddressByTime[indexB] = tmp;
        }

        private string getHourOrMinAtIndex(int index,int minOrHour)
        {
            if(minOrHour == 0)
            {
                string nextHour = ImgAddressByTime[index].Substring(ImgAddressByTime[index].LastIndexOf('-') + 1, 2);
                return nextHour;
            }else
            {
                string nextMin = ImgAddressByTime[index].Substring(ImgAddressByTime[index].LastIndexOf('-') + 3, 2);
                return nextMin;
            }
        }

        public string getNextMin(bool prev)
        {
            if(ImgAddressByTime.Count>0)
            {
                string time;
                string full;
                string nextHour;
                string nextMin;
                int nextHourInt;
                int nextMinInt;
                int index;
                bool exists;

                int numberOfNext = 0;
                do
                {
                    numberOfNext++;
                    if (prev) //min
                    {
                        index = ImgAddressByTime[0].LastIndexOf('-') + 1;
                        time = ImgAddressByTime[0].Substring(index);
                        full = ImgAddressByTime[0];
                        nextHour = getHourOrMinAtIndex(0, 0);
                        nextMin = getHourOrMinAtIndex(0, 1);
                        nextHourInt = int.Parse(nextHour);
                        nextMinInt = int.Parse(nextMin);
                        for (int i = 0; i < numberOfNext; i++)
                        {
                            nextMinInt--;
                            if (nextMinInt == -1)
                            {
                                nextMinInt = 59;
                                nextHourInt--;
                                if (nextHourInt == -1)
                                {
                                    return "";
                                }
                            }
                        }


                    }
                    else
                    {
                        index = ImgAddressByTime[ImgAddressByTime.Count - 1].LastIndexOf('-') + 1;
                        time = ImgAddressByTime[ImgAddressByTime.Count - 1].Substring(index);
                        full = ImgAddressByTime[ImgAddressByTime.Count - 1];
                        nextHour = getHourOrMinAtIndex(ImgAddressByTime.Count - 1, 0);
                        nextMin = getHourOrMinAtIndex(ImgAddressByTime.Count - 1, 1);
                        nextHourInt = int.Parse(nextHour);
                        nextMinInt = int.Parse(nextMin);
                        for (int i = 0; i < numberOfNext; i++)
                        {
                            nextMinInt++;
                            if (nextMinInt == 60)
                            {
                                nextMinInt = 0;
                                nextHourInt++;
                                if (nextHourInt == 24)
                                {
                                    return "";
                                }
                            }
                        }
                    }

                    string newNextHour = Converter.ValueConverter(nextHourInt);
                    string newNextMin = Converter.ValueConverter(nextMinInt);

                    string oldTime = nextHour + nextMin;
                    string newTime = newNextHour + newNextMin;
                    string result = time.Replace(oldTime, newTime);
                    full = full.Remove(index);
                    full += result;

                    exists = File.Exists(full);

                    if (!exists)
                    {
                        full = secExists(full);
                        exists = File.Exists(full);
                    }

                } while (!exists);
                return full;
            }
            return "";
        }

        private string secExists(string egesz)
        {
            bool secExists = false;
            int j = -1;

            do
            {
                j++;
                egesz = egesz.Remove(egesz.LastIndexOf('.') - 2, 2);
                egesz = egesz.Insert(egesz.LastIndexOf('.'), Converter.ValueConverter(j));
                secExists = File.Exists(egesz);
            } while (!secExists && j < 60);

            return egesz;
        }

        private void createTriangesToNavigateInPictures()
        {
            //createPolygons
            PolygonArray = new Polygon[2];
            for (int i = 0; i < PolygonArray.Length; i++)
            {
                PolygonArray[i] = new Polygon();

                PolygonArray[i].Stroke = Brushes.CornflowerBlue;
                PolygonArray[i].Fill = Brushes.CornflowerBlue;
                PolygonArray[i].StrokeThickness = 1;
                PolygonArray[i].HorizontalAlignment = HorizontalAlignment.Center;
                PolygonArray[i].VerticalAlignment = VerticalAlignment.Center;
            }
        }


        public Image getImageFromSavedFile(int i)
        {
           
            if (i < 0)
                i = 0;

            Image img = new Image();
            if (i < ImgAddressByTime.Count)
            {   
                LastDisplayedImg = i;
                img.Source = new BitmapImage(new Uri(ImgAddressByTime[i]));
            }
            return img;
        }

        public void getSnaps()
        {
     
            List<string> namesOfFiles = getRelevantSnapDateFolderFiles().ToList();
            namesOfFiles = namesOfFiles.OrderBy(o => o).ToList();
            foreach (string item in namesOfFiles)
                {
                    string hour = item.Substring(item.LastIndexOf('-') + 1, 2);
                    string min = item.Substring(item.LastIndexOf('-') + 3, 2);

                    string timehourString = DateTime.ToString("HH");
                    string timeminString = DateTime.ToString("mm");

                    int convMin = int.Parse(timeminString);
                    int prevMin = convMin - 1;
                    int nextMin = convMin + 1;

                    int convHour = int.Parse(timehourString);
                    string prevhour = timehourString;
                    string nexthour = timehourString;
                    string prevmin = Converter.ValueConverter(prevMin);
                    string nextmin = Converter.ValueConverter(nextMin);

                    if (prevMin == -1)
                    {
                        prevMin = 59;
                        prevmin = Converter.ValueConverter(prevMin);
                        convHour--;
                        prevhour = Converter.ValueConverter(convHour);
                    }
                    else if (nextMin == 60)
                    {
                        nextMin = 0;
                        nextmin = Converter.ValueConverter(nextMin);
                        convHour++;
                        nexthour = Converter.ValueConverter(convHour);
                    }

                    if ((timehourString.Equals(hour) && timeminString.Equals(min)) || (prevhour.Equals(hour) && prevmin.Equals(min)) || (nexthour.Equals(hour) && nextmin.Equals(min)))
                    {
                        ImgAddressByTime.Add(item);

                        if((prevhour.Equals(hour) && prevmin.Equals(min)))
                        {
                            FirstDisplayedImg = ImgAddressByTime.IndexOf(item);
                        }
                        
                    }

                }
        }


        private int getMotions()
        {
            int numberOfMotions = 0;
            IEnumerable<string> motions = getRelevantMotionDateFolderFiles();
            if(motions!=null)
            {
                foreach (string item in motions)
                {
                    string hour = item.Substring(item.IndexOf('_') + 1, 2);
                    string min = item.Substring(item.IndexOf('_') + 3, 2);
                    string timehourString = DateTime.ToString("HH");
                    string timeminString = DateTime.ToString("mm");

                    if (hour.Equals(timehourString) && min.Equals(timeminString))
                    {
                        ImgAddressByTime.Add(item);
                        numberOfMotions++;
                        string eventNumber = item.Substring(item.LastIndexOf('_'), item.Length - item.LastIndexOf('_'));

                        foreach (string insideItem in motions)
                        {
                            if (insideItem.Contains(eventNumber))
                            {
                                ImgAddressByTime.Add(insideItem);
                            }
                        }
                    }
                }
            }
            return numberOfMotions;
        }
        /*
        public static string Converter.ValueConverter(int conv)
        {
            if (conv < 10)
            {
                return "0" + conv;
            }
            else
            {
                return conv.ToString();
            }

        }
        */
        private IEnumerable<string> getRelevantSnapDateFolderFiles() 
        {
            IEnumerable<string> files;
      
            if(App.fromFTP)
            {
                files = Directory.EnumerateFiles(App.mainPath + "\\snap\\" + DateTime.Year.ToString() + Converter.ValueConverter(DateTime.Month) + Converter.ValueConverter(DateTime.Day) + "\\", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + Types.SaveType.png) || s.EndsWith("." + Types.SaveType.jpg));
            }
            else
            {
                files = Directory.EnumerateFiles(drive + App.newImgLocal + DateTime.Year.ToString() + Converter.ValueConverter(DateTime.Month) + Converter.ValueConverter(DateTime.Day) + "\\", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + Types.SaveType.png) || s.EndsWith("." + Types.SaveType.jpg));
            }
                    
            return files;
        }


        private IEnumerable<string> getRelevantMotionDateFolderFiles()
        {
            IEnumerable<string> files;
            try
            {
                if (App.fromFTP)
                {
                    files = Directory.EnumerateFiles(App.mainPath + "\\motion\\" + DateTime.Year.ToString() + Converter.ValueConverter(DateTime.Month) + Converter.ValueConverter(DateTime.Day) + "\\", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + Types.SaveType.png) || s.EndsWith("." + Types.SaveType.jpg));
                }
                else
                {
                    files = Directory.EnumerateFiles(drive + App.oldImgLocal + DateTime.Year.ToString() + Converter.ValueConverter(DateTime.Month) + Converter.ValueConverter(DateTime.Day) + "\\", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith("." + Types.SaveType.png) || s.EndsWith("." + Types.SaveType.jpg));

                }
            }
            catch(DirectoryNotFoundException)
            {
                return null;
            }
 
            return files;
        }
       
    }
}
