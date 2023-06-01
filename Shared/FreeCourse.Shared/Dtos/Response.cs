using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    #region #region Response Dto Açıklaması 
    //Bir api'ye istek yapıldığında başarılı veya başarısız durum döner bu durumların tanımlandığı dto alanı
    #endregion
    public class Response<T>
    {
        public T Data { get;  set; }

        [JsonIgnore]
        #region #region Bu datayı neden ignore'ladık?
        //Bu data'yı json tarafta ignore ettik durum kodunu zaten görebiliyoruz. Ancak sistem içerisinde buna ayrıca ibizm ihtiyacımız var çünkü response'un dönüş tipini belirlerken (yani error mu yoksa success mi gibi)   bu koddan faydalanacağız. ancak Bu prop'un repsonse' içerisinde ayrıca olmasına gerek yok 
        #endregion
        public int StatusCode { get;  set; }
        [JsonIgnore]
        public bool IsSuccessful { get;  set; }

        public List<string> Errors { get; set; }

        #region #region static methods
        //static methodlar direkt classın içerisinde nesne oluşumuna olanak sağlar.
        #endregion

        //Static Factory Method

        public static Response<T> Success(T data, int statusCode) //Cevabın Başarılı olup üzerine bize data döndürdüğü senaryo
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode) //Bu durum da durumun başarılı ama data dönmeyecek duurmlar update,delete gibi
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode) //Bu birden fazla hatanın olduğu senaryoda response dönecek o yüzden hatalar list string içinde
        {
            return new Response<T>
            {
                StatusCode = statusCode,
                Errors = errors,
                IsSuccessful = false
            };
        }
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };


        } //tek bir hata var ise bu
    }
}
