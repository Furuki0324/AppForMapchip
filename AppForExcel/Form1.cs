using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppForExcel
{
    public partial class Form1 : Form
    {
        private Image image = null;
        private float scalar = 0.95f;

        public Form1()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            //インスタンス生成
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Filter = "PNGファイル|*.png|JPGファイル|*.jpg;*.jpeg";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (image != null)
                {
                    image.Dispose();
                }
                string filePath = openFile.FileName;
                image = Image.FromFile(filePath);
                pictureBox1.Image = image;
                pictureBox1.Size = image.Size;
            }
        }

        private void OnImageClick(object sender, EventArgs e)
        {
            //マウスと画像の原点のモニター上のスクリーン座標を取得
            System.Drawing.Point mousePoint = MousePosition;
            System.Drawing.Point pictureOrigin = PointToScreen(pictureBox1.Location);

            //取得した座標からマウスが画像上のどの位置にいるのか求める
            int x = mousePoint.X - pictureOrigin.X;
            int y = mousePoint.Y - pictureOrigin.Y;

            int chipWidth;
            int chipHeight;
            int spritePerRow;

            //後の計算が正常に実行されない値が入力されていた場合は中断する
            if(WidthPixel.TextLength == 0
                || WidthPixel.Text == "0"
                || !int.TryParse(WidthPixel.Text, out chipWidth)
                || HeightPixel.TextLength == 0
                || HeightPixel.Text == "0"
                || !int.TryParse(HeightPixel.Text, out chipHeight))
            {
                SpriteNumber.Text = "NaN";
                return;
            }

            //画像の一列の中に何枚のチップがあるか求める
            spritePerRow = pictureBox1.Width / chipWidth;

            //クリックしたチップの番号を求める
            int spriteNum = (y / chipHeight) * spritePerRow + x / chipWidth;
            SpriteNumber.Text = spriteNum.ToString();
        }

        private void OnFormSizeChanged(object sender, EventArgs e)
        {
            //フォームのサイズが変更されたらコンテナのサイズも変更させる
            //100%ではスクロールバーが画面外に出てしまうため補正する
            splitContainer1.Width = (int)(ActiveForm.Width * scalar);
            splitContainer1.Height = (int)(ActiveForm.Height * scalar);
            splitContainer1.SplitterDistance = (int)(splitContainer1.Height * scalar);
        }
    }
}
