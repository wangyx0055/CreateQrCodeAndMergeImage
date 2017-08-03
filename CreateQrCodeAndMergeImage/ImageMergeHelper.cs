using System;
using System.Drawing;
using System.Linq;

namespace CreateQrCodeAndMergeImage
{
    public class ImageMergeHelper
    {
        /// <summary>
        /// 将多张图片拼接合并成一张指定大小的图片，各图像进行顺序排列
        /// </summary>
        /// <param name="height">新图像的高度</param>
        /// <param name="width">新图像的宽度</param>
        /// <param name="bw">图像间距</param>
        /// <param name="noimgtext">无图片时显示的文字，为空默认为:暂无图片</param>
        /// <param name="imgs">图像数组</param>
        /// <returns></returns>
        public static Image ImgMerge(int height, int width, int bw, string noimgtext, params Image[] imgs)
        {
            if (string.IsNullOrEmpty(noimgtext)) noimgtext = "暂无图片";
            Image ret = new System.Drawing.Bitmap(width, height);
            Graphics g = Graphics.FromImage(ret);
            g.Clear(Color.White);
            //新图像组合的图像个数
            int cnt = 0;
            if (imgs.Length == 1) cnt = 1;
            if (imgs.Length > 1) cnt = 4;
            if (imgs.Length > 4) cnt = 9;
            imgs = imgs.Take<Image>(cnt).ToArray();
            //求新列表维数
            int rat = Convert.ToInt16(Math.Sqrt(cnt));
            if (rat > 0)
            {
                //图片宽高度不能小于2像素
                if ((rat + 1) * bw + 2 * rat > width) bw = (width - 2 * rat) / (rat + 1);
                int th = (height - 2 * rat) / (rat + 1);
                if (th < bw)
                {
                    //相对高度计算出来的间距，取小不取大，这样图像宽度显示更大一些
                    bw = th;
                }
                if (bw <= 0) bw = 1; //防止意外
                //计算排列图片的尺寸
                int swidth = (width - (rat + 1) * bw) / rat;
                int sheight = (height - (rat + 1) * bw) / rat;
                //依次排列图片
                int hs = 1; //行数
                int ls = 1; //列数
                for (int i = 1; i <= imgs.Length; i++)
                {
                    Rectangle r = new Rectangle()
                    {
                        Height = sheight,
                        Width = swidth,
                        X = bw * ls + swidth * (ls - 1),
                        Y = bw * hs + sheight * (hs - 1)
                    };
                    g.DrawImage(imgs[i - 1], r);

                    //处理完后下一个位置输出
                    ls++;
                    if (i % rat == 0)
                    {
                        hs++;
                        ls = 1;
                    }
                }
                GC.Collect();
            }
            else
            {
                //写入文字：暂无图片
                StringFormat stringFormat = new StringFormat();
                stringFormat.LineAlignment = StringAlignment.Center;
                stringFormat.Alignment = StringAlignment.Center;
                g.DrawString(noimgtext, new Font("宋体", 15), new SolidBrush(Color.Black),
                    new RectangleF(0, 0, width, height), stringFormat);
            }
            return ret;
        }

    }
}
