using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using MobileService.DataObjects;
using MobileService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;
using Owin.Security.Providers.Instagram;
using Owin.Security.Providers.Instagram.Provider;
using Owin;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace MobileService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            options.LoginProviders.Add(typeof(TwitterLoginProvider));

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());

            IAppBuilder app = null;
            InstagramAuthenticationOptions option = new InstagramAuthenticationOptions()
            {
                ClientId = "InstagramClientId",
                ClientSecret = "InstagramClientSecret",
                AuthenticationType = "Instagram",
                Provider = new InstagramAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        //check user exist
                        return Task.FromResult(0);
                    }
                }
            };
            option.Scope.Add("likes");
            option.Scope.Add("comments");
            app.UseInstagramInAuthentication(option);
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}

