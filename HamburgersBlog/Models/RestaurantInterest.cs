using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;

namespace HamburgersBlog.Models
{
    public class RestaurantInterest
    {
        private static readonly RestaurantInterest m_instance = null;

        public static RestaurantInterest Instance
        {
            get
            {
                return m_instance;
            }
        }

        static RestaurantInterest()
        {
            m_instance = new RestaurantInterest();
        }

        private RestaurantInterest()
        {
        }

        internal int getInterestInRestaurant(HttpRequestBase request, HttpResponseBase response, int restaurantID)
        {
            Hashtable restaurantInterest = null;
            int interestInRestaurant = 0;

            // If there is a cookie already, use it
            HttpCookie myCookie = request.Cookies["UserInterestInRestaurant"];
            if (myCookie != null && myCookie.Value != null)
            {
                restaurantInterest = (Hashtable)DeserializeFromBase64String(myCookie.Value);
                if (restaurantInterest[restaurantID] != null)
                {
                    interestInRestaurant = (int)restaurantInterest[restaurantID];
                }
            }

            return interestInRestaurant;
        }

        public void AddUserInterestInRestaurant(HttpRequestBase request, HttpResponseBase response, int restaurantID, int interestScore)
        {
            Hashtable restaurantInterest = null;

            // If there is a cookie already, use it
            HttpCookie myCookie = request.Cookies["UserInterestInRestaurant"];
            if (myCookie != null && myCookie.Value != null)
            {
                restaurantInterest = (Hashtable)DeserializeFromBase64String(myCookie.Value);
            }
            else // If not, create a new one
            {
                myCookie = new HttpCookie("UserInterestInRestaurant");

                // Set the cookie expiration date.
                myCookie.Expires = DateTime.Now.AddYears(50); // For a cookie to effectively never expire

                restaurantInterest = new Hashtable();
            }

            // If the restaurant already exist increase it by 1
            if (restaurantInterest[restaurantID] != null)
            {
                restaurantInterest[restaurantID] = (int)restaurantInterest[restaurantID] + interestScore;
            }
            else
            {
                // If not initialize it to 1;
                restaurantInterest[restaurantID] = interestScore;
            }

            // Set the cookie value.
            myCookie.Value = SerializeToBase64String(restaurantInterest);


            // Add the cookie.
            response.Cookies.Add(myCookie);

            response.Write("<p> The cookie has been written.");
        }

        public string SerializeToBase64String(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            long length = memoryStream.Length;
            byte[] bytes = memoryStream.GetBuffer();

            string infoData = Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None);

            string encodedData = infoData;
            return encodedData;
        }

        public object DeserializeFromBase64String(string content)
        {
            byte[] memortyData = Convert.FromBase64String(content);
            int length = memortyData.Length;

            MemoryStream memoryStream = new MemoryStream(memortyData, 0, length);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object obj = binaryFormatter.Deserialize(memoryStream);

            return obj;
        }
    }
}