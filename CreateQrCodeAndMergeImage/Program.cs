using System;
using System.Drawing;
using System.IO;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace CreateQrCodeAndMergeImage
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("请输入要生成物料的数量！！！");
            var readCount = Console.ReadLine();
            var result = CommonHelper.IsNumeric(readCount);
            if (!result)
            {
                Console.WriteLine("请输入正确数量");
            }
            else
            {
                var jumpAddress = "https://github.com/AkonCoder/CreateQrCodeAndMergeImage/";
                var createNum = Convert.ToInt32(readCount);
                const string qrEncodingType = "BYTE";
                const string createLevel = "H";
                const int version = 8;
                const int scale = 12;

                for (int i = 0; i < createNum; i++)
                {
                    //1.生成二维码图片
                    var qrCodeFilePath = CreateCode_Choose(jumpAddress, qrEncodingType, createLevel, version, scale);

                    //2.拼接二维码图片，生成物料图片
                    var waterMark = WaterMarkImage(qrCodeFilePath);
                    DIVWaterMark(waterMark, i);
                    Console.WriteLine("第{0}合成图片成功!", i + 1);
                }
            }

            Console.ReadLine();
        }

        //生成二维码方法（简单）
        private static void CreateCode_Simple(string nr)
        {
            var qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeScale = 4,
                QRCodeVersion = 8,
                QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
            };
            //System.Drawing.Image image = qrCodeEncoder.Encode("15377541070 上海 Akon_Coder");
            Image image = qrCodeEncoder.Encode(nr);
            var filename = DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";
            var filepath = System.Web.HttpContext.Current.Server.MapPath(@"~\Upload") + "\\" + filename;
            var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

            fs.Close();
            image.Dispose();
            //二维码解码
            var codeDecoder = CodeDecoder(filepath);
        }

        /// <summary>
        /// 生成二维码方法（复杂）
        /// </summary>
        /// <param name="strData">要生成的文字或者数字，支持中文。如： "15377541070 上海 Akon_Coder</param>
        /// <param name="qrEncoding">三种尺寸：BYTE ，ALPHA_NUMERIC，NUMERIC</param>
        /// <param name="level">大小：L M Q H</param>
        /// <param name="version">版本：如 8</param>
        /// <param name="scale">比例：如 4</param>
        /// <returns></returns>
        public static string CreateCode_Choose(string strData, string qrEncoding, string level, int version, int scale)
        {
            var qrCodeEncoder = new QRCodeEncoder();
            string encoding = qrEncoding;
            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }
            //文字生成图片
            Image image = qrCodeEncoder.Encode(strData);
            var filename = DateTime.Now.ToString("yyyymmddhhmmssfff") + ".jpg";
            var filepath = AppDomain.CurrentDomain.BaseDirectory + @"\UploadPic\" + filename;
            var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
            image.Dispose();
            return filepath;
        }

        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public static string CodeDecoder(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            var myBitmap = new Bitmap(Image.FromFile(filePath));
            var decoder = new QRCodeDecoder();
            var decodedString = decoder.decode(new QRCodeBitmapImage(myBitmap));
            return decodedString;
        }

        /// <summary>
        /// 生成新的二维码
        /// </summary>
        /// <returns></returns>
        public static bool CreateImg()
        {
            Image img1 = Image.FromFile(@"D:\Pictures\hwl.jpg");
            Image img2 = Image.FromFile(@"D:\Pictures\sgh.jpg");
            Image img3 = Image.FromFile(@"D:\Pictures\syf.jpg");
            Image img = ImageMergeHelper.ImgMerge(200, 300, 5, null,
                new Image[] {img1, img2, img3, img2, img3, img2});
            img.Save(@"D:\Pictures\allinone.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return true;
        }

        #region 水印预设

        /// <summary>
        /// 水印文字预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkFont()
        {
            var waterMark = new WaterMark
            {
                WaterMarkType = WaterMarkTypeEnum.Text,
                Transparency = 0.7f,
                Text = "dunitian.cnblogs.com",
                FontStyle = System.Drawing.FontStyle.Bold,
                FontFamily = "Consolas",
                FontSize = 20f,
                BrushesColor = System.Drawing.Brushes.Black,
                WaterMarkLocation = WaterMarkLocationEnum.CenterCenter
            };
            return waterMark;
        }

        /// <summary>
        /// 图片水印预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkImage(string filePath)
        {
            var waterMark = new WaterMark
            {
                WaterMarkType = WaterMarkTypeEnum.Image,
                ImgPath = filePath,
                WaterMarkLocation = WaterMarkLocationEnum.CenterCenter,
                Transparency = 1f
            };
            return waterMark;
        }

        #endregion

        #region 水印操作

        /// <summary>
        /// 单个水印操作
        /// </summary>
        /// <param name="waterMark"></param>
        /// <param name="index"></param>
        private static void DIVWaterMark(WaterMark waterMark, int index = 0)
        {
            #region 必须参数获取

            //图片路径
            var filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Pictures\background.jpg";
            //文件名
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            //图片所处目录
            string dirPath = Path.GetDirectoryName(filePath);
            //存放目录     
            string savePath = dirPath + "\\ProductWaterMark";
            //是否存在，不存在就创建
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #endregion

            #region 水印操作

            Image successImage = WaterMarkHelper.SetWaterMark(filePath, waterMark);
            if (successImage != null)
            {
                //保存图片（不管打不打开都保存）
                successImage.Save(savePath + "\\" + index + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                Console.WriteLine("水印失败！请检查原图和水印图！");
            }

            #endregion
        }

        #endregion
    }
}