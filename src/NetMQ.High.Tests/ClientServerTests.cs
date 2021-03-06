﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NetMQ.High.Tests
{
    [TestFixture]
    class ClientServerTests
    {        
        class Handler : IAsyncHandler
        {            
            public Handler()
            {                
            }

            public async Task<object> HandleRequestAsync(ulong messageId, uint connectionId, string service,object body)
            {
                ConnectionId = connectionId;
                return "Welcome";
            }

            public void HandleOneWay(ulong messageId, uint connectionId, string service, object body)
            {
                                    
            }

            public uint ConnectionId { get; private set; }
        }     

        [Test]
        public void RequestResponse()
        {
            int i = 0;

            var serverHandler = new Handler();
            using (AsyncServer server = new AsyncServer(serverHandler))
            {
                server.Bind("tcp://*:6666");
                using (Client client = new Client("tcp://localhost:6666"))
                {
                    // client to server
                    var reply = (string) client.SendRequestAsync("Hello", "World").Result;
                    Assert.That(reply == "Welcome");                    
                }
            }    
        }
    }
}
