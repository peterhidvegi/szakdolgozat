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
using System.Windows.Shapes;

namespace Validator
{
    /// <summary>
    /// Interaction logic for MotionScanSettings.xaml
    /// </summary>
    public partial class MotionScanSettings : Window
    {
        public MotionScanSettings()
        {
            InitializeComponent();
            motionMinDiffAvgBox.Text = App.motionMinDiffAvg.ToString();
            motionMaxDiffAvgBox.Text = App.motionMaxDiffAvg.ToString();
            motionPixelCorrectLimitBox.Text = App.motionPixelCorrectLimit.ToString();
            motionMainCorrectPixelPercentageBox.Text = App.motionMainCorrectPixelPercentage.ToString();
            motionNodeCorrectPixelPercentageBox.Text = App.motionNodeCorrectPixelPercentage.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            App.motionMinDiffAvg = int.Parse(motionMinDiffAvgBox.Text);
            App.motionMaxDiffAvg = int.Parse(motionMaxDiffAvgBox.Text);
            App.motionPixelCorrectLimit = int.Parse(motionPixelCorrectLimitBox.Text);
            App.motionMainCorrectPixelPercentage = float.Parse(motionMainCorrectPixelPercentageBox.Text);
            App.motionNodeCorrectPixelPercentage = float.Parse(motionNodeCorrectPixelPercentageBox.Text);
            this.Close();
        }
    }
}
