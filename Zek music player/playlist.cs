using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Zek_music_player
{
    public partial class playlist : Form
    {

        Form1 frm1;
        public playlist(Form1 halo)
        {
            InitializeComponent();
            //check_playlist();
            frm1 = halo;

        
        }

        public string all_music = "";


        private void check_playlist()
        {

            string path = Directory.GetCurrentDirectory() + "/FAVORITE/";
          
            
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] diArr = di.GetDirectories();
            foreach (DirectoryInfo dum in diArr)
            {
                comboBox1.Items.Add(dum.Name);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= frm1.listView1.SelectedItems.Count - 1; i++)
            {
                string sPath = frm1.listView1.SelectedItems[i].SubItems[2].Text;
                List<string> MusicList = new List<string>();
                MusicList.Add(sPath);

                string dir_fav = comboBox1.Text;

                string make_files = Directory.GetCurrentDirectory() + "/FAVORITE/" + dir_fav + "/yourlist.txt";

                using (StreamWriter sw = new StreamWriter(make_files, true))
                {
                    sw.WriteLine(sPath);

                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("ENTER FIELD");

            }
            else
            {
                string path = Directory.GetCurrentDirectory()+"/FAVORITE/"+textBox1.Text;
                string name = Directory.GetCurrentDirectory() + "/FAVORITE/" + textBox1.Text + "/yourlist.txt";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                    if (File.Exists(name))
                    {

                    }
                    else
                    {
                        using (StreamWriter sw = File.CreateText(name))
                        {
                            MessageBox.Show("Playlist Added");

                        }
                    }
                    
                }
                else
                {
                   
                }
                


            }
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            frm1.button2.Enabled = false;
            this.Close();
            frm1.label1.Visible = false;
            frm1.listView1.MultiSelect = false;
            
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr one, int two, int three, int four);

        private void playlist_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);

        }

        private void comboBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            string path = Directory.GetCurrentDirectory() + "/FAVORITE/";


            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] diArr = di.GetDirectories();
            foreach (DirectoryInfo dum in diArr)
            {
                comboBox1.Items.Add(dum.Name);
            }

        }
    }
}
