using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;

namespace com.mercaderia.bono.Notificaciones.SMS
{
    public class NotificacionSMSProxy
    {
            public BonusSMS GetMessageMSMQ()
            {
                throw new NotImplementedException();
            }

            public BonusSMS GetMessageMSMQOUT()
            {
                throw new NotImplementedException();
            }


            public BonusSMS PutMessageMSMQDraft()
            {
                throw new NotImplementedException();
            }

            public BonusSMS PutMessageMSMQIn()
            {
                throw new NotImplementedException();
            }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public string SendMessageBonus(BonusMessageConfig messageConfig)
            {
                try
                {
                    BonusSMS smsDto = new BonusSMS();
                    string strresponse = ConsumirServicio(messageConfig);
                    RootObject obj = JsonConvert.DeserializeObject<RootObject>(strresponse);
                    return strresponse;
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
            }

            protected string ConsumirServicio(BonusMessageConfig messageConfig)
            {
                try
                {
                    BonusSMS MensajeSMS = messageConfig.message;
                    string contentResultante = string.Empty;
                    SmsSendDto mensajeenviar = new SmsSendDto();
                    mensajeenviar.from = MensajeSMS.From.ToString();
                    mensajeenviar.to = MensajeSMS.PhoneNumber.ToString();
                    mensajeenviar.text = MensajeSMS.message.ToString();

                    HttpMessageHandler handler = new HttpClientHandler(){ };
                    var httpClient = new HttpClient(handler)
                    {
                        BaseAddress = new Uri(messageConfig.serviceURL),
                        Timeout = new TimeSpan(0, 2, 0)
                    };

                    httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                    var method = new HttpMethod("POST");

                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("jybpanama:Panamajyb20");
                    string val = System.Convert.ToBase64String(plainTextBytes);

                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json ");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, messageConfig.serviceURL);
                    string request = "{\"from\":\"" + mensajeenviar.from + "\",\"to\": \"" + mensajeenviar.to + "\",\"text\":\"" + mensajeenviar.text + "\"}";
                    requestMessage.Content = new StringContent(request, Encoding.UTF8,"application/json");
                    HttpResponseMessage response = httpClient.SendAsync(requestMessage).Result;
                    using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result, System.Text.Encoding.GetEncoding("UTF-8")))
                    {
                        contentResultante = stream.ReadToEnd();
                    }

                    return contentResultante;
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }

            }
        }
}
