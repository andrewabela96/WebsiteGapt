using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GaptWebsite.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GaptWebsite.Controllers
{
    public class NewsController : ApiController
    {
        private NewsFeedEntity db = new NewsFeedEntity();

        // GET: api/News
        public  FbWrapper GetNews()
        {
            //request
            string url = "https://graph.facebook.com/v2.6/116437555088006/events";
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string since = unixTimestamp.ToString();
            string appId = System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookAppId"];
            string appSecret= System.Web.Configuration.WebConfigurationManager.AppSettings["FacebookSecret"];
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url+"?since="+since+ "&access_token="+appId+"|"+appSecret);
            request.Method = "GET";
            String test = String.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                test = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }
            //deserialize
            FbWrapper wrapper = JsonConvert.DeserializeObject<FbWrapper>(test);
            //sort fb
            QuickSort_Iterative(wrapper.data, 0, wrapper.data.Count - 1);
            //convert date
            for (int i = 0; i < wrapper.data.Count; i++)
            {
                string fbdate = wrapper.data[i].start_time.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                wrapper.data[i].start_time = Convert.ToDateTime(fbdate);
            }
            //sort ict
            var ict = db.News.OrderBy(n=> n.Dtime);
            FbWrapper ictWrapper = new FbWrapper();
            //change into same format
            foreach (News temp in ict)
            {
                string ictdate = temp.Dtime.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                temp.Dtime = Convert.ToDateTime(ictdate);
                //remove old
                if(temp.Dtime>DateTime.Now)
                {
                    FbData ictnews = new FbData(temp.Title, temp.Ndescription, temp.Dtime, temp.Category, new Place(temp.Location));
                    ictWrapper.data.Add(ictnews);
                }
      
            }
            //merge 2 sorted lists
            FbWrapper sorted = merge(wrapper.data, ictWrapper.data);
            return sorted;
        }

        static public int Partition(List<FbData> data, int left, int right)
        {
            FbData pivot = data[left];
            while (true)
            {
                while (data[left].start_time < pivot.start_time)
                    left++;

                while (data[right].start_time > pivot.start_time)
                    right--;

                if (left < right)
                {
                    FbData temp = data[right];
                    data[right] = data[left];
                    data[left] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        struct QuickPosInfo
        {
            public int left;
            public int right;
        };

        static public void QuickSort_Iterative(List<FbData> data, int left, int right)
        {

            if (left >= right)
                return; // Invalid index range

            List<QuickPosInfo> list = new List<QuickPosInfo>();

            QuickPosInfo info;
            info.left = left;
            info.right = right;
            list.Insert(list.Count, info);

            while (true)
            {
                if (list.Count == 0)
                    break;

                left = list[0].left;
                right = list[0].right;
                list.RemoveAt(0);

                int pivot = Partition(data, left, right);

                if (pivot > 1)
                {
                    info.left = left;
                    info.right = pivot - 1;
                    list.Insert(list.Count, info);
                }

                if (pivot + 1 < right)
                {
                    info.left = pivot + 1;
                    info.right = right;
                    list.Insert(list.Count, info);
                }
            }
        }

        public static FbWrapper merge(List<FbData> fbIn, List<FbData> ictIn)
        {

            List<FbData> sortedList = new List<FbData>();
            int i = 0, j = 0, k = 0;
            while (i < fbIn.Count && j < ictIn.Count)
            {
                if (fbIn[i].start_time < ictIn[j].start_time)
                {
                    sortedList.Add(fbIn[i]);
                    i++;
                }
                else
                {
                    sortedList.Add(ictIn[j]);
                    j++;
                }
                k++;
            }

            while (i < fbIn.Count)
            {
                sortedList.Add(fbIn[i]);
                i++;
                k++;
            }

            while (j < ictIn.Count)
            {
                sortedList.Add(ictIn[j]);
                j++;
                k++;
            }

            return new FbWrapper(sortedList);
        }

        // GET: api/News/5
        [ResponseType(typeof(News))]
        public IHttpActionResult GetNews(int id)
        {
            News news = db.News.Find(id);
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        // PUT: api/News/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNews(int id, News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != news.Id)
            {
                return BadRequest();
            }

            db.Entry(news).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/News
        [ResponseType(typeof(News))]
        public IHttpActionResult PostNews(News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.News.Add(news);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = news.Id }, news);
        }

        // DELETE: api/News/5
        [ResponseType(typeof(News))]
        public IHttpActionResult DeleteNews(int id)
        {
            News news = db.News.Find(id);
            if (news == null)
            {
                return NotFound();
            }

            db.News.Remove(news);
            db.SaveChanges();

            return Ok(news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsExists(int id)
        {
            return db.News.Count(e => e.Id == id) > 0;
        }
    }
}