using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Drawing;

namespace Spectrum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindows : Window
    {
        public static string[] fileNames = { };

        public MainWindows()
        {
            InitializeComponent();
        }

        private void ClickProcess(object sender, System.EventArgs e)
        {
            string line = "";

            //List<double> maxLamda = new List<double>();
            //List<double> maxIntensity = new List<double>();

            int n = 0;
            //string selectedDataFile = fileName.Insert(fileName.LastIndexOf('\\') + 1, "Selected_");
            string selectedDataFile = fileNames[0].Replace(fileNames[0].Substring(fileNames[0].LastIndexOf('\\') + 1, fileNames[0].IndexOf('.') - fileNames[0].LastIndexOf('\\') - 1), "SelectedData");
            //showText.Text += selectedDataFile+"\r\n";
            StreamWriter sWriter = new StreamWriter(selectedDataFile);
            foreach (string fileName in fileNames)
            {
                List<double> lamda = new List<double>();
                List<double> intensity = new List<double>();

                StreamReader sReader = new StreamReader(fileName);

                //line = sReader.ReadToEnd();
                //showText.Text += "Processing " + fileName + " ...\r\n";
                

                while (!sReader.EndOfStream)
                {
                    line = sReader.ReadLine();
                    //showText.Text += line+"\n";

                    bool isLineNumeric = true;
                    string[] splitLine = System.Text.RegularExpressions.Regex.Split(line, @"\s");
                    foreach (string sl in splitLine)
                    {
                        //showText.Text += sl+" ";
                        if (isNumeric(sl))
                            isLineNumeric = true;
                        else
                            isLineNumeric = false;
                    }
                    //showText.Text +=splitLine.Length+ "\n";

                    
                    double lamdaMin = Convert.ToDouble(LambdaStart.Text), lamdaMax = Convert.ToDouble(LambdaStop.Text);
                    if (isLineNumeric && splitLine.Length == 2)
                    {
                        double str2num = 0;
                        str2num = Convert.ToDouble(splitLine[0].Trim());

                        if (str2num > lamdaMin && str2num < lamdaMax)
                        {
                            lamda.Add(str2num);
                            str2num = Convert.ToDouble(splitLine[1].Trim());       //equal to: str2num = double.Parse(line);
                            intensity.Add(str2num);
                            
                        }
                    }
                }
                if (n==0)
                {
                    sWriter.Write("\t");
                    foreach (double temp in lamda)
                    {
                        sWriter.Write(temp + "\t");
                    }
                    sWriter.Write("\r\n");
                }
                n = n + 1;
                sWriter.Write(n+"\t");
                foreach(double temp in intensity)
                {
                    sWriter.Write(temp+"\t");
                }
                sWriter.Write("\r\n");
                
                /*for (int i = 0; i < lamda.Count;i++)
                {
                    showText.Text += "\n" + lamda[i] +' ' + intensity[i];
                }*/

                showText.Text += lamda[intensity.IndexOf(intensity.Max())]+" "+intensity.Max()+"\r\n";

            }
            sWriter.Close();
        }

        public bool isNumeric(string value)
        {
            string pattern = @"^(-|\d)*\.\d*$";
            return System.Text.RegularExpressions.Regex.IsMatch(value, pattern);
        }

        private void ClickOpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|Pictures(*.tif)|*.tif|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                fileNames = openFileDialog.FileNames;
            }
        }

        private void ClickEditSetting(object sender, RoutedEventArgs e)
        {

        }

        private void ClickSaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            //saveFileDialog.InitialDirectory = @"c:\";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|Pictures(*.tif)|*.tif|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                string saveName = saveFileDialog.FileName;
                StreamWriter sWriter = new StreamWriter(saveName);
                //sWriter.Write(showText);
                for (int i = 0; i < showText.LineCount; i++)
                {
                    sWriter.Write(showText.GetLineText(i));
                }
                sWriter.Close();
            }

        }

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            showText.Clear();
        }

        private void Click_RGBArray(object sender, RoutedEventArgs e)
        {
            Bitmap srcbmp = new Bitmap(fileNames[0]);
            
            showText.Text += srcbmp.Width + " " + srcbmp.Height;
            for(int width=0;width<srcbmp.Width;width++)
            {
                for(int height=0;height<srcbmp.Height;height++)
                {
                    System.Drawing.Color pixelColor = srcbmp.GetPixel(width, height);
                    byte alpha = pixelColor.A;
                    byte R = pixelColor.R;
                    byte G = pixelColor.G;
                    byte B = pixelColor.B;
                    showText.Text +=alpha+" "+ R + " " + G + " " + B+"\r\n";
                }
            }

        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Please contact Y.L. Feng if you have any question");
        }
    }
}
