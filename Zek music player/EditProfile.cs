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
using System.IO;
namespace Zek_music_player
{
    public partial class EditProfile : Form
    {
        Form1 frm1;

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

        public EditProfile(Form1 zz, string lokasi_gambar, string nama, string gelar, string instagram)
        {
            InitializeComponent();
            frm1 = zz;

            picture_location = lokasi_gambar;
            nickname = nama;
            gelarnya = gelar;
            nama_instagram = instagram;


            textBox1.Text = nama;
            textBox2.Text = gelar;
            textBox3.Text = instagram;
            try
            {
                guna2CirclePictureBox1.Image = new Bitmap(lokasi_gambar);

            }
            catch
            {
                guna2CirclePictureBox1.Image = null;
                textBox1.Text = "YOURNAME";
                textBox2.Text = "Master ~";
                textBox3.Text = "@Zekkel AR";
            }
        }

        public string picture_location = "";
        public string nickname         = "";
        public string gelarnya         = "";
        public string nama_instagram   = "";
        public string solo_pict        = "";
        public string solo2_pict       = "";
        private void EditProfile_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

            try
            {
                frm1.textBox1.Text = textBox1.Text;
                frm1.textBox2.Text = textBox2.Text;
                frm1.textBox3.Text = textBox3.Text;
                frm1.guna2CirclePictureBox1.Image = new Bitmap(lokasi_gambar_changed);


                string target_path = Directory.GetCurrentDirectory() + "/PROFILE/"+get_pict_name;
                File.Copy(lokasi_gambar_changed, target_path, true);
                string path_narik = "PROFILE/profile.txt";

                string real_target = "/PROFILE/"+get_pict_name;
                solo_pict = target_path;
                solo2_pict = real_target;
                string output = textBox1.Text + "|" + textBox2.Text + "|" + textBox3.Text + "|" + real_target;
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path_narik)))
                {
                    outputFile.WriteLine(output);
                }
            }
            catch
            {
                //frm1.guna2CirclePictureBox1.Image = new Bitmap(solo_pict);
                string path_narik = "PROFILE/profile.txt";
                string pict2 = Path.GetFileName(picture_location);
                string output = textBox1.Text + "|" + textBox2.Text + "|" + textBox3.Text + "|" + "/PROFILE/"+pict2;
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(path_narik)))
                {
                    outputFile.WriteLine(output);
                }
            }


        }


        public string lokasi_gambar_changed = "";
        public string nickname_changed = "";
        public string gelar_changed = "";
        public string instagram_changed = "";
        public string get_pict_name = "";
        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog buka_file = new OpenFileDialog();
            buka_file.Filter = "Audio Files|*.jpeg;*.png;*.jpg";
            buka_file.Multiselect = true;
            if (buka_file.ShowDialog() == DialogResult.OK)
            {
                guna2CirclePictureBox1.Image = new Bitmap(buka_file.FileName);
                lokasi_gambar_changed = buka_file.FileName;
                get_pict_name = Path.GetFileName(lokasi_gambar_changed);
                //MessageBox.Show(get_pict_name);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditProfile_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }
    }
}
