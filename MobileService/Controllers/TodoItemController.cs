using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using MobileService.DataObjects;
using MobileService.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace MobileService.Controllers
{   
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request, Services);
        }

        // GET tables/TodoItem
        public async Task<MobileServiceUser> GetAllTodoItems()
        {   
            MobileServiceClient client = new MobileServiceClient("http://localhost:51389/", "brucechen");
            var token = new JObject();
            token.Add("access_token", string.Empty);
            var result =await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, token).ContinueWith<MobileServiceUser>(t =>
            {
                if (t.Exception == null)
                {
                    return t.Result;
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("Error logging in: " + t.Exception);
                    return null;
                }
            });
            //this.User.

            return result;
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}