using System.Collections.Generic;

namespace NoKates.WebAdmin.Models
{
    public class Node
    {
        public string id { get; set; }
        public string name { get; set; }
        public Data data { get; set; }
        public List<Node> children { get; set; } = new List<Node>();

        public void AddChild(Node c)
        {
                children.Add(new Node
                {
                    id = $"{name}_{children.Count+1}",
                        name = c.name,
                    children = new List<Node>(),
                        data =  new Data()
                });
        }

      
    }
}
