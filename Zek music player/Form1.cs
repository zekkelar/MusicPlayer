using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WMPLib;
using System.IO;
using System.Diagnostics;
namespace Zek_music_player
{
    public partial class Form1 : Form
    {


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
      (
          int nLeftRect,     // x-coordinate of upper-left corner
          int nTopRect,      // y-coordinate of upper-left corner
          int nRightRect,    // x-coordinate of lower-right corner
          int nBottomRect,   // y-coordinate of lower-right corner
          int nWidthEllipse, // height of ellipse
          int nHeightEllipse // width of ellipse
      );
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);


        public WindowsMediaPlayer wp = new WindowsMediaPlayer();

        public string lokasi_gambar = "";
        public string nickname = "";
        public string gelar = "";
        public string instagrama = "";
        public Form1()
        {

            try
            {

                InitializeComponent();
                check_awal();

                Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            }

            catch
            {
                check_exist_file();
                
            }
            
        }
        private void check_exist_file()
        {
            string folder_favorite = "FAVORITE";
            string folder_profile = "PROFILE";
            if (!Directory.Exists(folder_favorite))
            {
                Directory.CreateDirectory(folder_favorite);


            }
            if (!Directory.Exists(folder_profile))
            {
                Directory.CreateDirectory(folder_profile);


            }

            string profile = "PROFILE/profile.txt";
            if (!File.Exists(profile))
            {
                using (StreamWriter sw = new StreamWriter(profile, true))
                {
                    sw.WriteLine("");

                }


            }
            

        }

        private void check_awal()
        {
            try
            {
                guna2TrackBar2.Enabled = false;
                string[] grab_profile = File.ReadAllLines("PROFILE/profile.txt");

                foreach (string line in grab_profile)
                {
                    string[] ehe = line.Split('|');
                    string username = ehe[0];
                    string gelarcuy = ehe[1];
                    string instagram = ehe[2];
                    string profile = ehe[3];

                    string location_image = Directory.GetCurrentDirectory() + ehe[3];
                    textBox1.Text = ehe[0];
                    textBox2.Text = ehe[1];
                    textBox3.Text = ehe[2];

                    lokasi_gambar = location_image;
                    nickname = textBox1.Text;
                    gelar = textBox2.Text;
                    instagrama = textBox3.Text;


                    Bitmap bmp = new Bitmap(location_image);
                    guna2CirclePictureBox1.Image = bmp;
                    this.FormBorderStyle = FormBorderStyle.None;
                    label7.Visible = false;
                    label1.Visible = false;
                    button1.Enabled = false;
                    button2.Enabled = false;

                }

            }
            catch
            {
                check_exist_file();
                guna2CirclePictureBox1.Image = null;
                label7.Visible = false;
                label1.Visible = false;
                button1.Enabled = false;
                button2.Enabled = false;
            }





        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            guna2ShadowForm1.SetShadowForm(this);
            if (System.IO.File.Exists("history.opx"))
            {
                string[] line_lagu = File.ReadAllLines("history.opx");
                foreach (string line in line_lagu)
                {
                    string[] data = line.Split('\t');
                    ListViewItem myitem = listView1.Items.Add(data[0]);
                    myitem.SubItems.Add(data[1]);
                    myitem.SubItems.Add(data[2]);
                }
            }

        }

        private void RemoveItems()
        {
            foreach (ListViewItem eachItem in listView1.SelectedItems)
            {
                listView1.Items.Remove(eachItem);
            }
        }

        private void NextSong()
        {

            try
            {
                int ok = listView1.SelectedIndices[0];
                ok += 1;
                try
                {
                    listView1.Items[ok].Selected = true;
                    listView1.Items[ok].EnsureVisible();
                    listView1.Select();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Print(e.Message);
                    listView1.Items[0].Selected = true;
                    listView1.Items[0].EnsureVisible();
                    listView1.Select();
                }
                PlayMusic();
            }
            catch
            {
                
            }
            
        }

        

        private void PrevSong()
        {
            try
            {
                int ok = listView1.SelectedIndices[0];
                ok -= 1;
                try
                {
                    listView1.Items[ok].Selected = true;
                    listView1.Items[ok].EnsureVisible();
                    listView1.Select();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    listView1.Items[listView1.Items.Count - 1].Selected = true;
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                    listView1.Select();

                }
                PlayMusic();
            }
            catch
            { 
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                guna2TrackBar2.Maximum = Convert.ToInt32(Math.Truncate(wp.currentMedia.duration));
                guna2TrackBar2.Value = Convert.ToInt32(Math.Truncate(wp.controls.currentPosition));
                label5.Text = wp.controls.currentPositionString;
                if (wp.playState == WMPPlayState.wmppsStopped)
                    if (guna2TrackBar2.Value == guna2TrackBar2.Minimum)
                    {
                        NextSong();
                    }
            }
            catch
            { 
            
            }
           
        }
        private void PlayMusic()
        {
            if (wp.playState == WMPPlayState.wmppsPaused)
            {
                wp.controls.play();
                timer1.Start();
            }
            else
            {
                try
                {
                    if (listView1.Items.Count > 0)
                    {
                        wp.URL = listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text;
                        wp.controls.play();
                        timer1.Start();
                    }
                }
                catch
                {
                    
                }
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            OpenFileDialog buka_file= new OpenFileDialog();
            buka_file.Filter = "Audio Files|*.mp3;*.wma;*.wav";
            buka_file.Multiselect = true;
            if (buka_file.ShowDialog() == DialogResult.OK)
            {
                foreach(string file in buka_file.FileNames)
                {
                    ListViewItem myitem = listView1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                    myitem.SubItems.Add(wp.newMedia(file).durationString);
                    myitem.SubItems.Add(file);

                    

                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (wp.playState == WMPPlayState.wmppsPlaying)
            {
                wp.controls.pause();
                timer1.Stop();
            }
        }

        private void guna2TrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
          
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            NextSong();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            PrevSong();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            guna2TrackBar2.Enabled = true;
            PlayMusic();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            wp.controls.stop();
            timer1.Stop();
            guna2TrackBar2.Value = 0;
            label5.Text = "00:00";
        }

    

        private void guna2TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
          
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }


        private void pictureBox9_Click(object sender, EventArgs e)
        {

            listView1.MultiSelect = true;
            label7.Visible = true;
            button1.Enabled = true;


            //listView1.Items[listView1.SelectedIndices[0]].EnsureVisible();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RemoveItems();
            listView1.MultiSelect = false;
            label7.Visible = false;
            button1.Enabled = false;
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog buka_file = new OpenFileDialog();
            buka_file.Filter = "Audio Files|*.jpeg;*.png;*.jpg";
            buka_file.Multiselect = true;
            if (buka_file.ShowDialog() == DialogResult.OK)
            {
                guna2CirclePictureBox1.Image = new Bitmap(buka_file.FileName);
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            EditProfile openprofile = new EditProfile(this,lokasi_gambar,nickname,gelar,instagrama);
            openprofile.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        
        
        private void guna2VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

            //vScrollHelper = new Guna.UI.Lib.ScrollBar.PanelScrollHelper(panel2, guna2VScrollBar1, true);
            //guna2VScrollBar1.Value = 0;
            //guna2VScrollBar1.Maximum = listView1.Items.Count + 1 + guna2VScrollBar1.LargeChange - 1;
            //guna2VScrollBar1.Value = e.NewValue;
            //listView1.EnsureVisible(8);
        }

        private void test_scrol()
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void add_favorit()
        {
            
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            listView1.MultiSelect = true;
            label1.Visible = true;
            button2.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
           

           
            //string get_filename = Path.GetFileName(sPath);
            //string get_path = Path.GetDirectoryName(sPath);
            //string complete = get_path + get_filename;
            //MessageBox.Show(complete);

            
            for (int i = 0; i <= listView1.SelectedItems.Count - 1; i++)
            {
                string sPath = listView1.SelectedItems[i].SubItems[2].Text;
                List<string> MusicList = new List<string>();
                MusicList.Add(sPath);
                
                
                
                //MessageBox.Show(MusicList.ToString());
            }
            playlist favorite = new playlist(this);
            favorite.ShowDialog();


        }

        private void guna2TrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                string time = Convert.ToString(TimeSpan.FromSeconds(Convert.ToDouble(guna2TrackBar2.Value)));
                label5.Text = time.Substring(time.Length - 5, 5);
            }
            catch
            { }
        }

        private void guna2TrackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            wp.controls.currentPosition = Convert.ToDouble(guna2TrackBar2.Value);
            timer1.Start();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            open_playlist opencuy = new open_playlist(this);
            opencuy.ShowDialog();

        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
