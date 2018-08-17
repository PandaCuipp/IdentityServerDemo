using HqFt.WeChatBooking.Toolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string url = "http://localhost:6235/api/MuTone/CardSettingSearch";
            //string url2 = "http://203.86.25.4:9004/api/Employee/CardSettingSearch?phone=13602695201&token=23F0E4C4FDDAEE25A8E63CE39D3A93BA&from=HuiYuanXiTong";
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("phoneNumber", "13602695201");
            //dic.Add("apiToken", "3c82bf8bef950be1daba429dd10a4189");
            //string param = JsonConvert.SerializeObject(dic);
            //var reqGet = Get(url2);
            //Console.WriteLine(reqGet.Result);

            //var httpContext = HttpContext.Current?.Request;
            //string url = httpContext?.Url.ToString();

            string token = "2A9FC2389E0C203A41397716D26C1CAEFC4C90AF52EC4F06BB2B18416F96A343A4089D6957DEBE21C809522E01065E5ECDD01AEA5124D95FD306447DAC6907B6";
            string username = ValidateApiToken(token);

            //string str = "【方特旅游】【莱芜市易游假期旅游服务有限公司】 团队预订信息：泰安方特欢乐世界，标准团，订单号：1101 1875 4572 2189 13，票种：导游票1张、全价票10张、免票1张，有效期：2018-01-01。当日凭短信至【团体售票处】购票。";
            string str = "【方特旅游】确认码：1102 0176 7736 0291 53,您已成功通过“方特旅游株洲官方微信”预定株洲方特欢乐世界（全价票1日2园）购买数量:（1）张；备注：<p>1、全价票适用于成人或身高≥1.4米的儿童；<br /></p><p>2、一日内可以游玩株洲方特欢乐世界+株洲方特梦幻王国，每个乐园限进入一次；</p><p>3、不支持刷身份证，需凭身份证前往景区3号和7号窗口取票入园。 </p>；（儿童票1日2园）购买数量:（1）张；备注：<p>1、儿童票适用于1.2米≤身高＜1.4米的儿童；<br /></p><p>2、一日内可以游玩株洲方特欢乐世界+株洲方特梦幻王国，每个乐园限进入一次；</p><p>3、不支持刷身份证，需凭身份证前往景区3号和7号窗口取票入园。 </p>；，2018-01-14当日有效。入园方式：刷身份证或二维码入园；凭身份证取票入园。查看二维码:http://ftwx.fangte.com/images/QRCode/leyou20180111114027562168022.jpg";
            //Console.WriteLine(Math.Ceiling(1.0 * str.Length / 70));
            Console.WriteLine(str.Length);
            Console.ReadKey();
        }

        public static async Task<string> Get(string url)
        {
            var responseJson = await HttpHelper.GetAsync(url);
            return responseJson;
        }

        /// <summary>
        /// 校验token
        /// </summary>
        /// <param name="Token">待验证的apiToken</param>
        /// <param name="UserName">返回通过token解析得到的请求用户名</param>
        /// <returns></returns>
        public static string ValidateApiToken(string Token)
        {
            string UserName = string.Empty;
            Token = (Token ?? "").Trim();

            try
            {
                if (string.IsNullOrEmpty(Token))
                {
                    return "Token为空";
                }

                string decryptStr = DESEncrypt.Decrypt(Token);

                if (string.IsNullOrEmpty(decryptStr))
                {
                    return "解密失败";
                }

                string[] TokenArray = decryptStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (null == TokenArray || TokenArray.Length != 2)
                {
                    return "token格式错误";
                }

                UserName = TokenArray[0];
                
            }
            catch (Exception)
            {
               
            }
            return UserName;
        }
    }
}
