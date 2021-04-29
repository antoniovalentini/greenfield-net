using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DH.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }

    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, byte[] encMessage, byte[] iv)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, encMessage, iv);
        }

        public async Task ExchangeKey(string user, byte[] publickey)
        {
            await Clients.All.SendAsync("ExchangeKey", user, publickey);
        }
    }
}
