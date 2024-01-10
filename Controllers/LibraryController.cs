using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lab4.Controllers
{
    public class LibraryController : ApiController
    {

        // GET api/values
        // возвратить список всех книг или найденных после поиска
        public IEnumerable<Library.BookSearchResult> Get()
        {
            if (WebApiApplication.ReturnAll) { 
                WebApiApplication.foundBooksResult = WebApiApplication.library.ReturnAll();
            }
            return WebApiApplication.foundBooksResult;
        }
        // GET api/values/5
        // возвратить книгу по индексу i
        public string Get(int i)
        {
            if (WebApiApplication.ReturnAll)
            {
                return WebApiApplication.library.ToString(i);
            }
            else
            {
                if (i >= 0 && i < WebApiApplication.foundBooksResult.Count())
                {
                    return WebApiApplication.foundBooksResult.ElementAt(i).ToString();
                }
            }
            return "";
        }
        // POST api/values
        // Список параметров value, первый параметр - команда
        public IHttpActionResult Post([FromBody] List<String> value)
        {
            if (value.Count == 0)
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }
            else
            if (value[0] == "all" && value.Count == 1)//показать все книги
            {
                WebApiApplication.ReturnAll = true;
            }
            else
            if (value[0] == "add" && value.Count == 6)//Новая книга
            {
                if (WebApiApplication.library.Add(new Book(value[1], value[2], value[3],Convert.ToDateTime(value[4]), value[5], value[6])))
                {
                    WebApiApplication.ReturnAll = true;
                }
            }
            else
            if (value[0] == "title" && value.Count == 2)//поиск по названию
            {
                WebApiApplication.ReturnAll = false;
                Predicate<Book> titlePredicate = book => book.Title.Contains(value[1]);
                WebApiApplication.foundBooksResult = WebApiApplication.library.Search(titlePredicate);
            }
            else
            if (value[0] == "author" && value.Count == 2)//поиск по автору
            {
                WebApiApplication.ReturnAll = false;
                Predicate<Book> authorPredicate = book => book.Author.Contains(value[1]);
                WebApiApplication.foundBooksResult = WebApiApplication.library.Search(authorPredicate);
            }
            else
            if (value[0] == "keywords" && value.Count == 2)//поиск по ключевым словам
            {
                WebApiApplication.ReturnAll = false;
                string[] searchKeyWords = value[1].Split();
                WebApiApplication.foundBooksResult = WebApiApplication.library.SearchKeywords(searchKeyWords);
            }
            else
            if (value[0] == "savexml" && value.Count == 1)
            {
                WebApiApplication.library.SaveToXML();
            }
            else
            if (value[0] == "loadxml" && value.Count == 1)
            {
                if (WebApiApplication.library.LoadFromXML())
                {
                    WebApiApplication.ReturnAll = true;
                }
            }
            else
            if (value[0] == "savejson" && value.Count == 1)
            {
                WebApiApplication.library.SaveToJSON();
            }
            else
            if (value[0] == "loadjson" && value.Count == 1)
            {
                if (WebApiApplication.library.LoadFromJSON())
                {
                    WebApiApplication.ReturnAll = true;
                }
            }
            else
            if (value[0] == "savesqlite" && value.Count == 1)
            {
                WebApiApplication.library.SaveToSQLite();
            }
            else
            if (value[0] == "loadsqlite" && value.Count == 1)
            {
                if (WebApiApplication.library.LoadFromSQLite())
                {
                    WebApiApplication.ReturnAll = true;
                }
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }
            return this.StatusCode(HttpStatusCode.OK);
        }

        // PUT api/values/5
        // изменить книгу с индексом i, строка value с разделителем
        public IHttpActionResult Put(int i, [FromBody] string value)
        {
            if (value.Length>0)
            {
                if (WebApiApplication.library.Update(i, value))
                {
                    return this.StatusCode(HttpStatusCode.OK);
                }
                else
                {
                    return this.StatusCode(HttpStatusCode.NotFound);
                }
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }
        }

        // DELETE api/values/5
        // удалить книгу с индексом i
        public IHttpActionResult Delete(int i)
        {
            if (WebApiApplication.library.Remove(i))
            {
                return this.StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
