using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyMp3Player
{
    class Mp3Player
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        [DllImport("winmm.dll")]
        public static extern int mciGetErrorString(int errCode, StringBuilder errMsg, int buflen);

        private int error;
        private string Pcommand;
        private bool IsPlaying = false;

//####################################################################################################################################################
        public void open(string File)
        {
            string Format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string command = string.Format(Format,File);
            mciSendString(command, null, 0, IntPtr.Zero);
        }
        
        public void play()
        {
            string command = "play MediaFile";
            mciSendString(command, null, 0, IntPtr.Zero);
            IsPlaying = true;
        }
        public void stop()
        {
            string command = "stop MediaFile";
            mciSendString(command, null, 0, IntPtr.Zero);
            IsPlaying = false;
        }

        public void close()
        {
            string command = "close MediaFile";
            mciSendString(command, null, 0, IntPtr.Zero);
            IsPlaying = false;
        }
 //###############################################################################################################################################
        public string Status()
        {
            string z = "";
            int i = 128;
            StringBuilder stringBuilder = new StringBuilder(i);
            mciSendString("status MediaFile mode", stringBuilder, i, IntPtr.Zero);
            z = stringBuilder.ToString();
            return z;
        }

        public void SetPosition(int miliseconds)
        {
            if (IsPlaying == true)
            {
                Pcommand = "play MediaFile from " + miliseconds.ToString();
                error = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
            else
            {
                Pcommand = "seek MediaFile to " + miliseconds.ToString();
                error = mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        public bool SetVolume(int volume)
        {
            if (volume >= 0 && volume <= 1000)
            {
                Pcommand = "setaudio MediaFile volume to " + volume.ToString();
                error = mciSendString(Pcommand, null, 0, IntPtr.Zero);
                return true;
            }
            else
                return false;
        }


    }
}
