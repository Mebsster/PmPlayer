using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace MyMp3Player
{

    public partial class Form1 : Form
    {
        private Mp3Player mp3Player = new Mp3Player();
        bool playing = false;
        bool showing = false;
        string current = "no";
        string name = "PmP";
        int index = 0;
        int selected = -1;
        int timemax;
        string[] paths, files;
        bool checks = Directory.Exists(@".\PmP Music");

        public Form1()
        {
            InitializeComponent();
            button1.Visible = false;
            textBox1.Visible = false;
            textBox1.ForeColor = Color.Silver;
            button11.Visible = false;
            button12.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            listBox1.Visible = false;
            timer1.Enabled = true;
            timer1.Interval = 200; // The time per tick (ms)
            timer1.Tick += new EventHandler(timer1_Tick);
            mp3Player.SetVolume(500);
            check();
        }

        // ADD FILE ####################################################################################################################
        private void button1_Click(object sender, EventArgs e)
        {
            mp3Player.close();
            timer1.Stop();
            trackBar1.Value = 0;
            button3.BackgroundImage = Properties.Resources.play;
            playing = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Mp3 Files|*.mp3";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                files = ofd.SafeFileNames;
                paths = ofd.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    string newPath = @".\PmP Music";
                    string destFile = Path.Combine(newPath, Path.GetFileName(paths[i]));
                    System.IO.File.Copy(paths[i], destFile, true);
                    System.IO.File.SetAttributes(newPath, FileAttributes.Normal);
                }
                Ref();
            }
            // listBox1.SelectedIndex = index;
        }
        // PPLAY BUTTON ################################################################################################################
        private void button3_Click(object sender, EventArgs e)
        {
            if (current == "no")
            {
            }
            else
            {
                mp3Player.stop();
                mp3Player.open(current);
                mp3Player.play();

                if (playing == false)
                {
                    timerstart();
                    button3.BackgroundImage = Properties.Resources.pause;
                    playing = true;
                    label1.Text = name;
                    // label1.Text = @"" + listBox1.SelectedItem + "";
                }
                else
                {
                    timer1.Stop();
                    playing = false;
                    button3.BackgroundImage = Properties.Resources.play;
                    mp3Player.stop();
                    label1.Text = "PmP";
                }
            }
        }
        //DRUG ##########################################################################################################################
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Capture = false;
            Message m = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }

        //VOLUME #########################################################################################################################
        /*
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN); SYSTEM VOLUME
        */
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            mp3Player.SetVolume(trackBar2.Value);
        }

        //LIST BOX SELECT #################################################################################################################
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (showing == true)
            {
                timer1.Stop();
                trackBar1.Value = 0;
                mp3Player.close();
                button3.BackgroundImage = Properties.Resources.play;
                current = @".\PmP Music\" + listBox1.SelectedItem + ".mp3";
                mp3Player.open(current);

                if (listBox1.SelectedIndex == -1)
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    selected = listBox1.SelectedIndex;
                   // button1.Location = new Point(260, 304);
                    button9.Visible = true;
                    button10.Visible = true;
                    button11.Visible = false;
                    button12.Visible = false;
                    textBox1.Visible = false;
                    button1.Visible = true;
                    name = @"" + listBox1.SelectedItem + "";
                    index = listBox1.SelectedIndex;
                    playing = false;
                }
            }
            else
            {
                if (listBox1.SelectedIndex == -1)
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    timer1.Stop();
                    trackBar1.Value = 0;
                    mp3Player.close();
                    button3.BackgroundImage = Properties.Resources.play;
                    current = @".\PmP Music\" + listBox1.SelectedItem + ".mp3";
                    mp3Player.open(current);
                    selected = listBox1.SelectedIndex;
                }
            }
            mp3Player.SetVolume(trackBar2.Value);

        }

        //SHOW SONGS MENU BUTTON ##########################################################################################################
        private void button4_Click(object sender, EventArgs e)
        {
            if (button1.Visible == true)
            {
                button4.BackgroundImage = Properties.Resources.open;
                button1.Visible = false;
                this.Width = 250;
                this.Height = 350;
                panel1.Width = 250;
                button6.Location = new Point(223, 3);
                //  label1.Location = new Point(82, 3);
                button13.Location = new Point(195, 3);
                label1.Width = 130;


            }
            else
            {
                button4.BackgroundImage = Properties.Resources.close;
                button1.Visible = true;
                this.Width = 435;
                this.Height = 350;
                panel1.Width = 435;
                button6.Location = new Point(408, 3);
                // label1.Location = new Point(174, 3);
                button13.Location = new Point(380, 3);
                label1.Width = 315;
            }

            Ref();

            if (showing == false)
            {
                if (index == 0)
                { }
                else
                {
                    listBox1.SelectedIndex = index;
                }
                listBox1.Show();
                showing = true;
                // playing = false;
            }
            else
            {
                listBox1.Hide();
                showing = false;
            }

            if (this.listBox1.SelectedItems.Count == 0)
            {
                // button1.Location = new Point(260, 304);
               button9.Visible = false;
               button10.Visible = false;
            }
            else
            {
                // button1.Location = new Point(314, 305);
                button9.Visible = true;
                button10.Visible = true;
            }
        }

        //CLOSE APPLICATION BUTTON ########################################################################################################
        private void button6_Click_1(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            label1.Capture = false;
            Message m = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            WndProc(ref m);
        }
        //NEXT BUTTON #####################################################################################################################
        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 1)
            {
                listBox1.SetSelected(0, true);

                if (playing == true)
                {
                    mp3Player.play();
                    timerstart();
                    button3.BackgroundImage = Properties.Resources.pause;
                    label1.Text = @"" + listBox1.SelectedItem + "";
                    index = listBox1.SelectedIndex;
                }
            }
            else
            {
                if (current == "no")
                { }
                else
                {
                    if (selected == listBox1.Items.Count)
                    {
                        listBox1.SetSelected(listBox1.Items.Count - listBox1.Items.Count, true);
                    }
                    else
                    {
                        if (selected + 1 >= listBox1.Items.Count)
                        {
                            listBox1.SelectedIndex = 0;
                            selected = 0;
                        }
                        else
                        {
                            listBox1.SelectedIndex = selected + 1;
                        }
                    }
                    if (playing == true)
                    {
                        mp3Player.play();
                        timerstart();
                        button3.BackgroundImage = Properties.Resources.pause;
                        label1.Text = @"" + listBox1.SelectedItem + "";
                        index = listBox1.SelectedIndex;
                    }

                    /*
                if (listBox1.SelectedIndex == listBox1.Items.Count - 1)
                {
                listBox1.SetSelected(listBox1.Items.Count - listBox1.Items.Count, true);
                }
                else
                {
                listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                }
                    if (playing == true)
                    {
                        mp3Player.play();
                        timerstart();
                        button3.BackgroundImage = Properties.Resources.pause;
                    }
                    */
                }
            }

        }

        //DELETE BUTTON ###################################################################################################################
        private void button10_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            mp3Player.close();
            File.Delete(current);
            current = "no";
            button3.BackgroundImage = Properties.Resources.play;
            Ref();
            if (index == 0)
            {
                if (listBox1.Items.Count == 0)
                { }
                else
                {
                    listBox1.SelectedIndex = 0;
                }
            }
            else
            {
                listBox1.SelectedIndex = index - 1;
            }
        }

        //RENAME BUTTON ###################################################################################################################
        private void button9_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItems.Count == 0)
            { }
            else
            {
                textBox1.ForeColor = Color.Silver;
                textBox1.Text = "Enter new name";
                textBox1.Visible = true;
                button1.Visible = false;
                button10.Visible = false;
                button11.Visible = true;
                button12.Visible = true;
            }
        }

        //RENAME FIELD CLICK ##############################################################################################################
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Black;
            textBox1.Text = "";
        }

        //RENAME FIELD LEAVE ###############################################################################################################
        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Silver;
            textBox1.Text = "Enter new name";
        }

        //PREVIOUS BUTTON ##################################################################################################################
        private void button8_Click(object sender, EventArgs e)
        {
            if (current == "no")
            { }
            else
            {
                if (selected == 0)
                {
                    listBox1.SetSelected(listBox1.Items.Count - 1, true);
                }
                else
                {
                    listBox1.SetSelected(selected - 1, true);
                }
                if (playing == true)
                {
                    mp3Player.play();
                    timerstart();
                    button3.BackgroundImage = Properties.Resources.pause;
                    label1.Text = @"" + listBox1.SelectedItem + "";
                }
                index = listBox1.SelectedIndex;
            }
        }

        //REFRESH FUNCTION #################################################################################################################
        private void Ref()
        {
            listBox1.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(@".\PmP Music");
            FileInfo[] files = dir.GetFiles("*.mp3");
            foreach (FileInfo fi in files)
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(fi.ToString()));
            }
        }

        //SONG LENGTH FUNCTION #############################################################################################################
        private int Leng()
        {
            Mp3FileReader reader = new Mp3FileReader(current);
            TimeSpan duration = reader.TotalTime;
            double m = duration.TotalSeconds;
            int res = Convert.ToInt32(m);
            //listBox1.Items.Add(res);
            //  listBox1.Items.Add(duration);
            reader.Close();
            return (res);
        }

        //TIMER TICK #######################################################################################################################
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (trackBar1.Value != timemax)
            {
                trackBar1.Value++;
                // progressBar1.Value++;
            }
            else
            {
                if (playing == true)
                {
                    continuin();
                }
                else
                {
                    timer1.Stop();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            File.Move(current, @".\PmP Music\" + textBox1.Text + ".mp3");
            current = @".\PmP Music\" + textBox1.Text + ".mp3";
            //  textBox1.ForeColor = Color.Silver;
            // textBox1.Text = "Enter new name";
            button11.Visible = false;
            button12.Visible = false;
            textBox1.Visible = false;
            button1.Visible = true;
            button10.Visible = true;
            Ref();
            listBox1.SelectedItem = textBox1.Text;
            // listBox1.SelectedIndex = index;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.Silver;
            textBox1.Text = "Enter new name";
            button11.Visible = false;
            button12.Visible = false;
            textBox1.Visible = false;
            button1.Visible = true;
            button10.Visible = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            mp3Player.SetPosition(trackBar1.Value * 100);
        }

        //START PROGRESS BAR FUNCTION ######################################################################################################
        private void timerstart()
        {
            timemax = (Leng() * 10) - 1;
            trackBar1.Maximum = timemax;
            timer1.Start();
            // listBox1.Items.Add(Leng());
        }
        private void timerstop()
        {
            trackBar1.Maximum = 0;
            trackBar1.Value = 0;
            timer1.Stop();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void continuin()
        {
            if (listBox1.SelectedIndex == listBox1.Items.Count - 1)
            {
                listBox1.SetSelected(listBox1.Items.Count - listBox1.Items.Count, true);
            }
            else
            {
                listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
            }
            if (playing == true)
            {
                mp3Player.play();
                timerstart();
                button3.BackgroundImage = Properties.Resources.pause;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            mp3Player.close();
            button3.BackgroundImage = Properties.Resources.play;
            playing = false;
            timerstop();
            // Directory.Delete(@"..\..\PmP Music");
            delete(@".\PmP Music");
            Directory.Delete(@".\PmP Music");
            listBox1.Items.Clear();
            button10.Visible = false;
            button9.Visible = false;
            this.Hide();
            Form f2 = new Form2();
            f2.Show();
            //  Application.Exit();
        }

        private void check()
        {
            if (checks == false)
            {
                Directory.CreateDirectory(@".\PmP Music");
            }
        }

        private void delete(string folder)
        {
            //Класс DirectoryInfo как раз позволяет работать с папками. Создаём объект этого
            //класса, в качестве параметра передав путь до папки.
            DirectoryInfo di = new DirectoryInfo(@".\PmP Music");
           // DirectoryInfo di = new DirectoryInfo(@"..\..\PmP Music");
            //Создаём массив дочерних вложенных директорий директории di
            DirectoryInfo[] diA = di.GetDirectories();
            //Создаём массив дочерних файлов директории di
            FileInfo[] fi = di.GetFiles();
            //В цикле пробегаемся по всем файлам директории di и удаляем их
            foreach (FileInfo f in fi)
            {
                f.Delete();
            }
            //В цикле пробегаемся по всем вложенным директориям директории di 
            foreach (DirectoryInfo df in diA)
            {
                //Как раз пошла рекурсия
                delete(df.FullName);
                //Если в папке нет больше вложенных папок и файлов - удаляем её,
                if (df.GetDirectories().Length == 0 && df.GetFiles().Length == 0) df.Delete();
            }

        }
    }
}
