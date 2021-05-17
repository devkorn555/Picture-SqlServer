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
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Insert_image_2sql
{
    public partial class Form1 : Form
    {
        string strcon = @"Data Source=DESKTOP-MPTIJRG\SQLEXPRESS;Initial Catalog=r_control;User ID=sa;PWD=12345";

        byte[] img;
        FileStream fs;
        BinaryReader br;
        string path = null;
        string imgname = null;

        SqlConnection con;

        public Form1()
        {
            InitializeComponent();
        }

        private void flatButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog di = new OpenFileDialog();
            di.Filter = "Image File|*.jpg";
            DialogResult dr = di.ShowDialog();

            if (dr == DialogResult.OK)
            {
                path = di.FileName;
                tbpath.Text = path;

                int lastindex = path.LastIndexOf("\\");
                imgname = path.Substring(lastindex + 1);
               
                pictureBox1.Image = Image.FromFile(path);
                pictureBox1.Refresh();

                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                img = br.ReadBytes(Convert.ToInt32(fs.Length));
            }
            else
            {
                path = null;
                tbpath.Text = null;
                pictureBox1.Image = Insert_image_2sql.Properties.Resources.question_mark_2_256;
                pictureBox1.Refresh();
            }
        }

        private void flatButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Connectdb();
                string txt = "INSERT INTO Testimg(img_name,img_path,img) VALUES (@name,@path,@img)";
                SqlCommand cmd = new SqlCommand(txt, con);


                cmd.Parameters.Add(new SqlParameter("name", imgname));
                cmd.Parameters.Add(new SqlParameter("path", path));
                cmd.Parameters.Add(new SqlParameter("img", img));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Suscess!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void Connectdb()
        {
            con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Insert_image_2sql.Properties.Resources.question_mark_2_256;
        }
    }
}
