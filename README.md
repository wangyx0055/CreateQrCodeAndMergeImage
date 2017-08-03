# 二维码自动生成并合成图片
@(二维码生成)[图片合并|物料|扫码]

**CreateQrCodeAndMergeImage**用于自动生成二维码，拼接模板图片，合成生成二维码物料图片，主要用于微信会员卡商家张贴物料二维码图片，用户扫码领卡，类似于支付宝收款码物料！


## 生成二维码

关于二维码的介绍这里就不多说了，可以参照以下的几篇介绍文章：

#### (1)http://coolshell.cn/articles/10590.html

#### (2)http://blog.csdn.net/u012611878/article/details/53167009

本文的用户场景是需要根据短链接生成二维码，然后合并物料二维码的模板（有自己的样式的图片，）,批量生成多张物料图片

交付给文印室打印寄出，用户扫码进行微信会员卡的领卡二维码绑定。绑定后用户到店消费扫描二维码即可领卡成为会员。


其中生成二维码的方法代码：
``` python
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
```
其中的参数：
strData：要生成的文字或者数字，支持中文。如：strData是要生成的文字或者数字，支持中文

如： "15377541070 上海 Akon_Coder；

qrEncoding： 三种尺寸：BYTE ，ALPHA_NUMERIC，NUMERIC

level ：大小：L M Q H<

version： 版本：如 8

scale： 比例：如 4

## 合并图片

（1）合成图片首先进行图片水印预设
```
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
```

（1）图片水印操作
先获取物料模板图片，然后根据图片的路径创建合成图片的存放目录。其中打水印后生成新的图片设计到

【1】新建一个Image属性  
【2】将颜色矩阵添加到属性
【3】原图格式检验+水印
【4】索引图片,转成位图再加图片

操作结果：
![Akon_coder](https://github.com/AkonCoder/CreateQrCodeAndMergeImage/blob/master/CreateQrCodeAndMergeImage/TestImg/2.png)   

等一系列操作。合成完成以后保存在bin目录下的ProductWaterMark文件下：

![Akon_coder](https://github.com/AkonCoder/CreateQrCodeAndMergeImage/blob/master/CreateQrCodeAndMergeImage/TestImg/1.png) 

有问题欢迎随时反馈：
微信：Akon_Coder 
QQ: 1013630498




  



