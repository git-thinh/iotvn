// Copyright (C) 2016 by Barend Erasmus and donated to the public domain

using SimpleHttpServer;
using SimpleHttpServer.Models;
using SimpleHttpServer.RouteHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Threading.Tasks;

namespace iotvn
{
    static class Routes
    {

        public static List<Route> GET
        {
            get
            {
                return new List<Route>()
                {
                    new Route()
                    {
                        Callable = HomeIndex,
                        UrlRegex = "^\\/$",
                        Method = "GET"
                    },
                    new Route()
                    {
                        Callable = new FileSystemRouteHandler() { 
                            BasePath = @"D:\Projects\themes",
                            ShowDirectories = true
                        }.Handle,
                        UrlRegex = "^\\/Static\\/(.*)$",
                        Method = "GET"
                    }
                };

            }
        }

        private static HttpResponse HomeIndex(HttpRequest request)
        {
            return new HttpResponse()
            {
                ContentAsUTF8 = "Hello",
                ReasonPhrase = "OK",
                StatusCode = "200"
            };

        }
    }

    class tcpServer_test2
    {
        public static void run()
        {
            HttpServer httpServer = new HttpServer(8080, Routes.GET);

            Thread thread = new Thread(new ThreadStart(httpServer.Listen));
            thread.Start();
        }
    }
}
