using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Threading;
using Emgu.CV.CvEnum;

namespace Emgu_CV
{
    public partial class Form1 : Form
    {
        private Capture _Capture;
        Image<Bgr,Byte> frame;
        private bool _captureInProgress = false;
        Image<Bgr, byte> temp;
        public Form1()
        {
            InitializeComponent();
        }
  private void Form1_Load(object sender, EventArgs e)
        {
            _Capture = new Capture(0);
            Application.Idle += new EventHandler(TimerEventProcessor);
        }
        void TimerEventProcessor(object sender, EventArgs e)
        {
            frame = _Capture.QueryFrame();
            imageBox1.Image = frame;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
           
            
            if (_Capture == null)
            {
                
                try
                {
                    //打開預設的webcam
                    
                    _Capture = new Capture();
                   
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("請先插入WebCam裝置，再按確定!");
                    
                }
            }
            if (_Capture != null)
            {
                //frame啟動
                if (_captureInProgress)
                {  //stop the capture
                    _captureInProgress = false;
                    button1.Text = "重新連接";
                    Application.Idle -= TimerEventProcessor;
                }
                //frame關閉
                else
                {
                    //start the capture
                    _captureInProgress = true;
                    button1.Text = "结束";
                    Application.Idle += TimerEventProcessor;
                }
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            
            if (_Capture == null)
            {
                try
                {
                    //打開預設的webcam
                    _Capture = new Capture(0);

                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            }
            else
            {

                _Capture = new Capture();
                Application.Idle += new EventHandler(TimerEventProcessor);
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog
                {
                    FileName = DateTime.Now.ToString("yyyy_MMdd_hh_mm_ss"),
                    Filter = "Image Files(*.bmp)|*.bmp|All files (*.*)|*.*"
                };
                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    frame.Save(SaveFileDialog1.FileName);
                }
            }



        }

        private void Button3_Click(object sender, EventArgs e)
        {
            
            
            if (_Capture == null)
            {
                MessageBox.Show("請重新連接WebCam", "error");
            }
            else
            {
                _Capture = new Capture();
                temp = _Capture.QueryFrame();
                SaveFileDialog SaveFileDialog2 = new SaveFileDialog
                {
                    FileName = DateTime.Now.ToString("yyyy_MMdd_hh_mm_ss"),
                    Filter = "Image Files(*.avi)|*.avi|All files (*.*)|*.*"
                };
                if (SaveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("開始錄製，按ESC結束錄製");
                }
                
                VideoWriter video = new VideoWriter(SaveFileDialog2.FileName, CvInvoke.CV_FOURCC('X', 'V', 'I', 'D'), 5, 640, 480, true);
                while (temp != null)
                {
                    CvInvoke.cvShowImage("camera", temp.Ptr);
                    temp = _Capture.QueryFrame();
                    int c = CvInvoke.cvWaitKey(3);
                    video.WriteFrame<Bgr, byte>(temp);
                    if (c == 27) break;
                }
                video.Dispose();
                CvInvoke.cvDestroyWindow("camera");

                //錄影完需將影像停止不然會出錯
                _captureInProgress = false;
                button1.Text = "開始";
                Application.Idle -= TimerEventProcessor;
            }

        }
       
}
}
