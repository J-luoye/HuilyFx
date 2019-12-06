using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace webapi.Controllers
{

    class  model
    {
        public string id { get; set; }

        public bool ist { get; set; }

        public string sp { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        //二维码
        public ActionResult Qrcode(string name)
        {
            using (var ms2 = new MemoryStream())
            {
                string sign = "YWtoZzM4dTRocjIwaDNlcjIzZWRldHIzNHQ1NHQ1Z3JldGc0NTY0NXQ0ZXJ0NXk0dGc0NXQ0OTJiZ2YzcmczNHQ1";

                var pfx = GenerateKeys();
                

                var unixEpoch =DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch

                var str = new model()
                {
                    id = "2000000000000000",
                    ist = true,
                    sp =  Math.Round(unixEpoch.TotalSeconds).ToString()
                };

                var json = JsonConvert.SerializeObject(str, Formatting.None);

                var ms = new MemoryStream();
                var jsonBase = Encoding.UTF8.GetBytes(json).ToArray();
                var jsonBase64 = Convert.ToBase64String(jsonBase);
                var signStr = RASHashAndSign(jsonBase64, pfx[0]);

                if (new QrEncoder(ErrorCorrectionLevel.M).TryEncode($"{jsonBase64}.{signStr}", out QrCode qr))//对内容进行编码，并保存生成的矩阵
                {
                    var render = new GraphicsRenderer(new FixedModuleSize(8, QuietZoneModules.Two));
                    render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                }
                var code = Encoding.UTF8.GetString(jsonBase);



                Image img = Image.FromStream(ms);
                string filename = DateTime.Now.ToString("yyyymmddhhmmss");
                string path = Server.MapPath("~/content/") + filename + ".png";
                img.Save(path);
                ViewBag.QrImage = "/content/" + filename + ".png";

                ViewBag.one = sign;
                ViewBag.two = jsonBase;
                ViewBag.three = code;

                //Response.ContentType = "image/Png";
                //Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
                //Response.End();
            }
            return View();
        }

        /// <summary>
        /// 生成私钥和公钥 arr对私钥[0]arr对公钥[1]
        /// </summary>
        /// <returns></returns>
        public static string[] GenerateKeys()
        {
            string[] sKeys = new String[2];
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            sKeys[0] = rsa.ToXmlString(true);
            sKeys[1] = rsa.ToXmlString(false);
            return sKeys;
        }

        /// <summary>
        /// RAS 对数据签名
        /// </summary>
        /// <param name="str_DataToSign">需签名数据</param>
        /// <param name="str_Private_Key">私钥</param>
        /// <returns></returns>
        public static string RASHashAndSign(string str_DataToSign, string str_Private_Key)
        {
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] DataToSign = ByteConverter.GetBytes(str_DataToSign);
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.FromXmlString(str_Private_Key);
                byte[] signedData = RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
                string str_SignedData = Convert.ToBase64String(signedData);
                return str_SignedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 验证RAS 签名
        /// </summary>
        /// <param name="str_DataToVerify">明文</param>
        /// <param name="str_SignedData">签名数据</param>
        /// <param name="str_Public_Key">公钥</param>
        /// <returns></returns>
        public static bool RASVerifySignedHash(string str_DataToVerify, string str_SignedData, string str_Public_Key)
        {
            byte[] SignedData = Convert.FromBase64String(str_SignedData);

            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            byte[] DataToVerify = ByteConverter.GetBytes(str_DataToVerify);
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSAalg.FromXmlString(str_Public_Key);
                //RSAalg.ImportCspBlob(Convert.FromBase64String(str_Public_Key));
                return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }



        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="strContent">待编码的字符</param>
        /// <param name="ms">输出流</param>
        ///<returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>
        public static bool GetQRCode(string strContent, MemoryStream ms)
        {
            ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M; //误差校正水平 
            string Content = strContent;//待编码内容
            QuietZoneModules QuietZones = QuietZoneModules.Two; //空白区域 
            int ModuleSize = 12;//大小
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (encoder.TryEncode(Content, out qr))//对内容进行编码，并保存生成的矩阵
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
