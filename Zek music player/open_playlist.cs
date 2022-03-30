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
    public partial class open_playlist : Form
    {

        Form1 frm1;
        public open_playlist(Form1 halo)
        {
            InitializeComponent();
            check_playlist();
            frm1 = halo;
        }

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
            string dir = Directory.GetCurrentDirectory() + "/FAVORITE/" + comboBox1.Text + "/yourlist.txt";
            string[] lines = File.ReadAllLines(dir);
            foreach (string line in lines)
            {
                ListViewItem myitem = frm1.listView1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(line));;
                myitem.SubItems.Add(frm1.wp.newMedia(line).durationString);
                myitem.SubItems.Add(line);
               
            }
        }


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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void open_playlist_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }
    }
}
