using System.Collections.Generic;
using System.Linq;
using NoKates.WebAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NoKates.Common.Infrastructure.Attributes;
using NoKates.Common.Infrastructure.Helpers;
using NoKates.Common.Models;

namespace NoKates.WebAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependencyController : ControllerBase
    {
        [HttpGet]
        [NoAuth]
        public Node GetAllDependencies()
        {
          //  return Get();
          return new Node
          {
              name = "Service_A",
              data = new Data{},
              id = "0",
              children = new List<Node>
              {
                  new Node
                  {
                      name = "Service_B",
                      data = new Data{},
                      id = "0_1",
                      children = new List<Node>
                      {
                          new Node
                          {
                              name = "Procedure_A",
                              data = new Data{},
                              id = "0_1_0",
                              children = new List<Node>
                              {
                                  new Node
                                  {
                                      name = "Table_A",
                                      data = new Data{},
                                      id = "0_2",
                                      children = new List<Node>
                                      {

                                      }

                                  }

                              }

                          },
                          new Node
                          {
                              name = "Procedure_B",
                              data = new Data{},
                              id = "0_1_1",
                              children = new List<Node>
                              {
                                  new Node
                                  {
                                      name = "Table_B",
                                      data = new Data{},
                                      id = "0_1_1_1",
                                      children = new List<Node>
                                      {

                                      }

                                  }

                              }

                          }
        }

                  },

                  new Node
                  {
                      name = "Table_A",
                      data = new Data{},
                      id = "0_2",
                      children = new List<Node>
                      {

                      }

                  },
                   new Node
                                  {
                                      name = "Table_B",
                                      data = new Data{},
                                      id = "0_1_1_1",
                                      children = new List<Node>
                                      {

                                      }

                                  }
        }

          };

        }

        //ServiceFlow
        [HttpGet("Request/{requestId}")]
        [NoAuth]
        public Node GetRequestFlow(string requestId)
        {
            var token = Request.Cookies["AccessToken"];
                var requestMetrics = HttpHelper.Get<List<RequestMetric>>($"{GlobalConfig.RequestMetricUrl}{requestId}", token);

            var ServiceFlow = MapRequestFlow2(requestMetrics);
            return ServiceFlow;
        }

        public Node Get()
        {
            var json = System.IO.File.ReadAllText(@"E:\SqlObjects.json");
            var objs= JsonConvert.DeserializeObject<List<SqlReference>>(json);
            var eoObjs = objs.Select(ConvertToEO).ToList();

            foreach (var sqlReference in objs)
            {
                var source = eoObjs.FirstOrDefault(o => o.id == sqlReference.Id);
                var reference = eoObjs.FirstOrDefault(o => o.id == sqlReference.ReferencedId);
                if (source != null && reference != null)
                    source.children.Add(reference);
            }

            var result = new Node
            {
                name = "Root",
                id = "1",
                data = new Data{relation = "Root"},
                
            };
            eoObjs.Where(o => eoObjs.All(e => !e.children.Contains(o))).ToList().ForEach(result.AddChild);
            return result;

        }

        
        private Node ConvertToEO(SqlReference arg)
        {
            var obj = new Node
            {
                id = arg.Id,
                name = arg.Name,
                data = new Data{relation = string.Empty},
              
            };

            return obj;
        }





        private Node MapRequestFlow2(List<RequestMetric> metrics)
        {
            var root = metrics.FirstOrDefault(m => m.RequestSource == "External" ||
                                                   m.RequestStart == metrics.Min(m2 => m2.RequestStart));

            var result = new Node
            {
                id = root.EndpointDesignation,
                name = root.ServiceName,
                data = new Data()
                {
                    relation = JsonConvert.SerializeObject(root, Formatting.Indented)
                },
                children = GetSubRequests2(root?.ServiceName, metrics, 0)
            };

            return result;

        }

        private List<Node> GetSubRequests2(string requestSource, List<RequestMetric> metrics, int level)
        {
            if (level > 8)
                return new List<Node>();

            var requests = metrics.Where(m => m.RequestSource == requestSource);
            if (!requests?.Any() ?? false) return new List<Node>();

            var newList = metrics.Where(r => !requests.Contains(r)).ToList();

            return requests.Select(request => new Node
            {
                id = request.EndpointDesignation,
                name = request.ServiceName,
                data = new Data()
                {
                    relation = JsonConvert.SerializeObject(request, Formatting.Indented)
                },
                children = GetSubRequests2(request?.ServiceName, metrics, level+1)
            }).ToList();

        }
    }

    public class SqlReference
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ReferencedId { get; set; }
        public string Referenced_Object_Name { get; set; }

    }
}
