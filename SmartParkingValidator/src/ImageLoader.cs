using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Validator.src;

namespace Validator
{
    class ImageLoader : IMotionDataListener, IStoppableThread
    {
        MainWindow window;
        BackgroundWorker worker;
        IMotionListListener listener;
        List<string> datas = new List<string>();
        List<Motion> motions;
        //old points: 0,150;640,150;0,225;640,225
        //new points: 0,100;640,100;0,160;640,160
        //5.1+ coordinatas: 0,318 ; 1280,318 ; 0,420 ; 1280,420
        public ImageLoader(IMotionListListener listener, List<string> path, MainWindow main)
        {
            this.listener = listener;
            motions = new List<Motion>();
            window = main;
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWorkWithPath;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync(path);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                window.progressBarForImageTest.Value = e.ProgressPercentage;
                window.labelImageComparator.Content = e.UserState;
            });
        }

        private void Worker_DoWorkWithPath(object sender, DoWorkEventArgs e)
        {
            List<string> motions = (e.Argument as List<string>);
            for (int i = 1; i < motions.Count; i++)
            {
                if (!worker.CancellationPending)
                {
                    string firstSrc = motions[i - 1];
                    string secondSrc = motions[i];
                    Bitmap firstDefault = imageCropper(firstSrc);
                    Bitmap secondDefault = imageCropper(secondSrc);
                    float firstCheck = ImageComparator.CorrectPixels(firstDefault, secondDefault, App.motionPixelCorrectLimit, "camparator0");
                    if (firstCheck < App.motionMainCorrectPixelPercentage)
                    {
                        for (int j = 1; j < 10; j++)
                        {
                            window.Stoppables.Add(new ImageComparator(this, motions[i], (Bitmap)firstDefault.Clone(), (Bitmap)secondDefault.Clone(), j.ToString(), App.motionPixelCorrectLimit-5));
                        }
                    }
                    worker.ReportProgress((i * 100 / motions.Count), motions[i]);
                } else
                {
                    e.Cancel = true;
                    return;
                }
            }
           
            listener.GetMotions(getTheList());
            worker.ReportProgress(100, "Completed!");
        }

        private Bitmap imageCropper(string fileSrc)
        {
            Rectangle cropRect = new Rectangle(new System.Drawing.Point(0, 318), new System.Drawing.Size(1280, 102));
            Bitmap src = new Bitmap(fileSrc);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            return target;
        }

        private List<RealMotion> getTheList()
        {

            List<RealMotion> completedList = new List<RealMotion>();

            int i = 0;
            while (i < motions.Count - 1)
            {
                float avg = 0;
                List<MotionNode> motionNodes = new List<MotionNode>();
                bool singletonMotion = true;
                while (i < motions.Count - 1 && DateTime.Compare(motions[i].date, motions[i + 1].date) == 0)
                {
                    singletonMotion = false;
                    motionNodes.Add(new MotionNode(motions[i].node, motions[i].percentage));
                    avg += motions[i].percentage;
                    i++;
                }

                motionNodes.Add(new MotionNode(motions[i].node, motions[i].percentage));
                avg += motions[i].percentage;

                if (singletonMotion)
                    completedList.Add(new RealMotion(motions[i].date, motionNodes, 100));
                else
                {
                    avg = avg / motionNodes.Count;
                    completedList.Add(new RealMotion(motions[i].date, motionNodes, avg));
                }

                i++;
            }

            return completedList;
        }

        public void GetEventData(string eventDate, string node, float result)
        {
            // datas.Add(eventDate + " %" + node + " =" + Converter.ValueConverter(Convert.ToInt32(result)));
            string justDate = eventDate.Substring(eventDate.LastIndexOf('_') + 1);
            string date = justDate.Substring(0, justDate.IndexOf('-'));
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(4, 2));
            int day = int.Parse(date.Substring(6, 2));
            //time:
            string timeAndNode = justDate.Substring(justDate.IndexOf('-') + 1);
            int hour = int.Parse(timeAndNode.Substring(0, 2));
            int minute = int.Parse(timeAndNode.Substring(2, 2));
            int sec = 0;

            DateTime convertedDateTime = new DateTime(year, month, day, hour, minute, sec);
            motions.Add(new Motion(convertedDateTime, int.Parse(node), result));
        }


        public void StopBackgroundWorker()
        {
            worker.CancelAsync();
            worker.Dispose();
        }
    }


    /* old version

    /*
        private List<RealMotion> getRelevantList()
        {
            List<RealMotion> completedList = new List<RealMotion>();

            int i = 0;
            while (i < motions.Count - 1) {

                float avg = 0;
                int j = 0;
                float minPercentage = int.MaxValue;
                bool singletonMotion = true;
                MotionNode[] motionNodes = new MotionNode[9];
                while (i < motions.Count - 1 && DateTime.Compare(motions[i].date, motions[i + 1].date)==0)
                {                
                    singletonMotion = false;
                    if(motions[i].percentage < minPercentage)
                    {
                       bool foundEmptyPlace = false;
                       int k = 0;
                       while(!foundEmptyPlace && k < motionNodes.Length - 1)
                       {
                            if(motionNodes[k] == null)
                            {
                                foundEmptyPlace = true;
                                motionNodes[k] = new MotionNode(motions[i].node, motions[i].percentage);
                            }
                            k++;
                       }
                        if(!foundEmptyPlace)
                            motionNodes[0] = new MotionNode(motions[i].node, motions[i].percentage);

                       motionNodes = motionNodes.OrderByDescending(p =>
                       {
                           if (p != null) return p.percentage;
                           else return float.MaxValue;
                       }).ToArray();

                        if(motionNodes[0]!=null)
                        {
                            minPercentage = motionNodes[0].percentage;
                        }else
                        {
                            minPercentage = float.MaxValue;
                        }
                      
                    }
                    avg += motions[i].percentage;
                    j++;
                    i++;
                }

                if(singletonMotion)
                {
                    motionNodes[0] = new MotionNode(motions[i].node, motions[i].percentage);
                    completedList.Add(new RealMotion(motions[i].date, motionNodes, 100,1));
                  
                }
                else
                {
                    avg = avg / j;
                    completedList.Add(new RealMotion(motions[i].date, motionNodes, avg,j));
                }
                i++;

            }

            return completedList;
        }
        
        
    */
}
