using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = DBA.GetDBToDataTable("select * from tbl_award");
            DrawAward(dt);
        }

        void DrawAward(DataTable dt)
        {
            Bitmap bitmap = new Bitmap(300, 300);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            int scale = 360 / 8;
            int startScale = 0 - 45 / 2;
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                Color color = ColorTranslator.FromHtml("#ffcb3f");
                if (i % 2 == 0)
                {
                    color = ColorTranslator.FromHtml("#ffb820");
                }
                //g.DrawArc(Pens.Red, 0, 0, 300, 300, startScale, scale);
                g.FillPie(new SolidBrush(color), 0, 0, 300, 300, startScale, scale);
                startScale += scale;
                string text = row["text"].ToString();
                if (i == 0)
                {
                    AddText(bitmap, "220,140", 15, text, 90);
                }
                if (i == 1)
                {
                    AddText(bitmap, "200,220", 15, text, 135);
                }
                if (i == 2)
                {
                    AddText(bitmap, "115,250", 15, text, 180);
                }
                if (i == 3)
                {
                    AddText(bitmap, "30,210", 15, text, 225);
                }

                if (i == 4)
                {
                    AddText(bitmap, "10,140", 15, text, 270);
                }
                if (i == 5)
                {
                    AddText(bitmap, "40,60", 15, text, 315);
                }

                if (i == 6)
                {
                    AddText(bitmap, "120,30", 15, text, 0);
                }
                if (i == 7)
                {
                    AddText(bitmap, "200,60", 15, text, 45);
                }
                i++;
            }
            pictureBox1.Image = bitmap;
            pictureBox1.Image.Save("award.png");
        }

        /// <summary>
        /// 图片添加任意角度文字(文字旋转是中心旋转，角度顺时针为正)
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="locationLeftTop">文字左上角定位(x1,y1)</param>
        /// <param name="fontSize">字体大小，单位为像素</param>
        /// <param name="text">文字内容</param>
        /// <param name="angle">文字旋转角度</param>
        /// <param name="fontName">字体名称</param>
        /// <returns>添加文字后的Bitmap对象</returns>
        public Bitmap AddText(Bitmap bmp, string locationLeftTop, int fontSize, string text, int angle = 0, string fontName = "黑体")
        {
            //Image img = Image.FromFile(imgPath);

            //int width = img.Width;
            //int height = img.Height;
            //Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;       
            // 画底图
            //graphics.DrawImage(bmp, 0, 0, width, height);

            Font font = new Font(fontName, fontSize, GraphicsUnit.Pixel);
            SizeF sf = g.MeasureString(text, font); // 计算出来文字所占矩形区域

            // 左上角定位
            string[] location = locationLeftTop.Split(',');
            float x1 = float.Parse(location[0]);
            float y1 = float.Parse(location[1]);

            // 进行文字旋转的角度定位
            if (angle != 0)
            {
                #region 法一：TranslateTransform平移 + RotateTransform旋转

                /* 
                    * 注意：
                    * Graphics.RotateTransform的旋转是以Graphics对象的左上角为原点，旋转整个画板的。
                    * 同时x，y坐标轴也会跟着旋转。即旋转后的x，y轴依然与矩形的边平行
                    * 而Graphics.TranslateTransform方法，是沿着x，y轴平移的
                    * 因此分三步可以实现中心旋转
                    * 1.把画板(Graphics对象)平移到旋转中心
                    * 2.旋转画板
                    * 3.把画板平移退回相同的距离(此时的x，y轴仍然是与旋转后的矩形平行的)
                    */
                //// 把画板的原点(默认是左上角)定位移到文字中心
                //graphics.TranslateTransform(x1 + sf.Width / 2, y1 + sf.Height / 2);
                //// 旋转画板
                //graphics.RotateTransform(angle);
                //// 回退画板x,y轴移动过的距离
                //graphics.TranslateTransform(-(x1 + sf.Width / 2), -(y1 + sf.Height / 2));

                #endregion

                #region 法二：矩阵旋转

                Matrix matrix = g.Transform;
                matrix.RotateAt(angle, new PointF(x1 + sf.Width / 2, y1 + sf.Height / 2));
                g.Transform = matrix;

                #endregion
            }

            Color color_text = ColorTranslator.FromHtml("#e4370e");
            // 写上自定义角度的文字
            g.DrawString(text, font, new SolidBrush(color_text), x1, y1);

            g.Dispose();
            //img.Dispose();

            return bmp;
        }
    }
}
